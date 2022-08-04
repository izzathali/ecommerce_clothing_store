using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using AspNetCoreHero.ToastNotification.Abstractions;
using Ecommerce.Model;
using Ecommerce.IBL;

namespace BAUHINIA_Ecommerce.Controllers
{
    public class CategoryController : Controller
    {
        IMemoryCache memoryCache;
        private UserM Usr = new UserM();
        private readonly INotyfService _notyf;

        private readonly ICategoryRepository iCategoryRepository;
        public CategoryController(IMemoryCache memoryCache, INotyfService notyf,ICategoryRepository _iCategoryRepository)
        {
            //db = context;
            this.memoryCache = memoryCache;
            Usr = memoryCache.Get("LoggedUser") as UserM;
            _notyf = notyf;
            iCategoryRepository = _iCategoryRepository;
        }

        // GET: Category
        public async Task<IActionResult> Index()
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }

            ViewBag.Users = Usr;

            return View(await iCategoryRepository.GetAllCategoriesAsync());
        }

        // GET: Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            if (id == null || await iCategoryRepository.GetAllCategoriesAsync() == null)
            {
                return RedirectToAction("PageNotFound","Error");
            }

            var category = await iCategoryRepository.GetCategoryByCategoryId(id);

            if (category == null)
            {
                return RedirectToAction("PageNotFound","Error");
            }

            return View(category);
        }

        // GET: Category/Create
        public IActionResult Create()
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName")] CategoryM category)
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            if (ModelState.IsValid)
            {
                //db.Add(category);
                //await db.SaveChangesAsync();

                await iCategoryRepository.AddCategory(category);

                _notyf.Success("Category Saved Successfully !!",3);
                //return RedirectToAction(nameof(Index));
                return RedirectToAction(nameof(Create));
            }
            return View(category);
        }

        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            if (id == null || await iCategoryRepository.GetAllCategoriesAsync() == null)
            {
                return RedirectToAction("PageNotFound","Error");
            }

            //var category = await db.Categories.FindAsync(id);
            var category = await iCategoryRepository.GetCategoryByCategoryId(id);

            if (category == null)
            {
                return RedirectToAction("PageNotFound","Error");
            }
            return View(category);
        }

        // POST: Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName")] CategoryM category)
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            if (id != category.CategoryId)
            {
                return RedirectToAction("PageNotFound","Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //db.Update(category);
                    //await db.SaveChangesAsync();

                    await iCategoryRepository.UpdateCategory(category);

                    _notyf.Success("Category Updated Successfully !!", 3);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryId))
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
            return View(category);
        }

        // GET: Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }

            if (id == null || await iCategoryRepository.GetAllCategoriesAsync() == null)
            {
                return RedirectToAction("PageNotFound","Error");
            }

            //var category = await db.Categories
            //    .FirstOrDefaultAsync(m => m.CategoryId == id);

            var category = await iCategoryRepository.GetCategoryByCategoryId(id);

            if (category == null)
            {
                return RedirectToAction("PageNotFound","Error");
            }

            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            if (await iCategoryRepository.GetAllCategoriesAsync() == null)
            {
                return Problem("Entity set 'BauhiniaEcomDbContext.Categories'  is null.");
            }
            //var category = await db.Categories.FindAsync(id);
            //if (category != null)
            //{
            //    db.Categories.Remove(category);
            //}

            //await db.SaveChangesAsync();

            await iCategoryRepository.DeleteCategory(id);

            _notyf.Success("Category Deleted Successfully !!", 3);
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return iCategoryRepository.CategoryExists(id);
        }
    }
}
