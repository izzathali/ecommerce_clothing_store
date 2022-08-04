using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using AspNetCoreHero.ToastNotification.Abstractions;
using Ecommerce.Model;
using Ecommerce.IBL;

namespace BAUHINIA_Ecommerce.Controllers
{
    public class ProductController : Controller
    {
        //private readonly EcommerceDbContext db;
        private readonly IWebHostEnvironment _hostEnvironment;
        IMemoryCache memoryCache;
        private UserM Usr = new UserM();
        private readonly INotyfService _notyf;
        private readonly IProductRepository iProductRepository;
        private readonly ICategoryRepository iCategoryRepository;
        public ProductController(IProductRepository _iProductRepository,ICategoryRepository _iCategoryRepository, IWebHostEnvironment hostEnvironment,IMemoryCache memoryCache, INotyfService notyf)
        {
            iProductRepository = _iProductRepository;
            iCategoryRepository = _iCategoryRepository;

            this._hostEnvironment = hostEnvironment;
            this.memoryCache = memoryCache;
            Usr = memoryCache.Get("LoggedUser") as UserM;
            _notyf = notyf;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            //User Usr = new User();
            //Usr = memoryCache.Get("LoggedUser") as User;
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }

            ViewBag.Users = Usr;

            //var bauhiniaEcomDbContext = db.Products.Include(p => p.category);
            //return View(await bauhiniaEcomDbContext.ToListAsync());

            return View(await iProductRepository.GetAllProducts());
        }
        public async Task<IActionResult> Stock()
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }

            return View(await iProductRepository.GetAllProducts());

        }
        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }

            if (id == null || await iProductRepository.GetAllProducts() == null)
            {
                return RedirectToAction("PageNotFound","Error");
            }

            var product = await iProductRepository.GetProductByProductId(id);

            if (product == null)
            {
                return RedirectToAction("PageNotFound","Error");
            }

            return View(product);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }

            //ViewData["CategoryId"] = new SelectList(db.Categories, "CategoryId", "CategoryName");
            ViewData["CategoryId"] = new SelectList(iCategoryRepository.GetAllCategories(), "CategoryId", "CategoryName");
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductCode,ProductName,ProductDescription,CategoryId,ProductPrice,ProductPicFile,Quantity")] ProductM product)
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Save image to wwwroot/image
                    string wwwRootPath = _hostEnvironment.WebRootPath;

                    string fileName = Path.GetFileNameWithoutExtension(product.ProductPicFile.FileName);
                    string extension = Path.GetExtension(product.ProductPicFile.FileName);
                    product.ProductPicUrl = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/ProductImg/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await product.ProductPicFile.CopyToAsync(fileStream);
                    }

                    //db.Add(product);
                    //await db.SaveChangesAsync();

                    await iProductRepository.AddProudct(product);

                    _notyf.Success("Product Saved Successfully !!", 3);
                    return RedirectToAction(nameof(Create));
                }
                catch (Exception ex)
                {

                }
            }
            //ViewData["CategoryId"] = new SelectList(db.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["CategoryId"] = new SelectList(iCategoryRepository.GetAllCategories(), "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }

            //if (id == null )
            if (id == null || await iProductRepository.GetAllProducts() == null)
            {
                return RedirectToAction("PageNotFound","Error");
            }

            var product = await iProductRepository.GetProductByProductId(id);

            if (product == null)
            {
                return RedirectToAction("PageNotFound","Error");
            }

            ViewData["CategoryId"] = new SelectList(iCategoryRepository.GetAllCategories(), "CategoryId", "CategoryName", product.CategoryId);

            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductCode,ProductName,ProductDescription,CategoryId,ProductPrice,ProductPicUrl,ProductPicFile,Quantity")] ProductM product)
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }

            if (id != product.ProductId)
            {
                return RedirectToAction("PageNotFound","Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Save image to wwwroot/image
                    string wwwRootPath = _hostEnvironment.WebRootPath;

                    string fileName = "";
                    if (product.ProductPicFile != null)
                    {
                        fileName = Path.GetFileNameWithoutExtension(product.ProductPicFile.FileName);
                        string extension = Path.GetExtension(product.ProductPicFile.FileName);
                        product.ProductPicUrl = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        string path = Path.Combine(wwwRootPath + "/ProductImg/", fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await product.ProductPicFile.CopyToAsync(fileStream);
                        }

                    }


                    //db.Update(product);
                    //await db.SaveChangesAsync();

                    await iProductRepository.UpdateProduct(product);

                    _notyf.Success("Product Updated Successfully !!", 3);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return RedirectToAction("PageNotFound","Error");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(iCategoryRepository.GetAllCategories(), "CategoryId", "CategoryId", product.CategoryId);
            return View(product);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }

            if (id == null || await iProductRepository.GetAllProducts() == null)
            {
                return RedirectToAction("PageNotFound","Error");
            }

            var product = await iProductRepository.GetProductByProductId(id);


            if (product == null)
            {
                return RedirectToAction("PageNotFound","Error");
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }

            if (await iProductRepository.GetAllProducts() == null)
            {
                return Problem("Entity set 'BauhiniaEcomDbContext.Products'  is null.");
            }
            //var product = await db.Products.FindAsync(id);

            await iProductRepository.DeleteProduct(id);


            _notyf.Success("Product Deleted Successfully !!", 3);

            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return iProductRepository.ProductExists(id);
        }
    }
}
