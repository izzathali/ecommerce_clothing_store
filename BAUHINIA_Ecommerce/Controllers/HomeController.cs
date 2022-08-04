using BAUHINIA_Ecommerce.Models;
using Ecommerce.IBL;
using Ecommerce.Model;
using Ecommerce.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace BAUHINIA_Ecommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private List<ShoppingCartVM> listOfShoppingCartModels;
        IMemoryCache memoryCache;
        private readonly IProductRepository iProductRepository;
        private readonly ICategoryRepository iCategoryRepository;
        public HomeController(ILogger<HomeController> logger, IMemoryCache memoryCache, IProductRepository iProductRepository, ICategoryRepository iCategoryRepository)
        {
            _logger = logger;
            listOfShoppingCartModels = new List<ShoppingCartVM>();
            this.memoryCache = memoryCache;
            this.iProductRepository = iProductRepository;
            this.iCategoryRepository = iCategoryRepository;
        }

        public IActionResult Index()
        {
            var products = iProductRepository.GetProductTop(4);

            return View(products);
        }
        public async Task<IActionResult> Shop()
        {
            IEnumerable<CategoryM> categories = iCategoryRepository.GetAllCategories();
            ViewBag.Categories = categories;

            var products = await iProductRepository.GetAllProducts();

            return View(products);

        }
        public async Task<IActionResult> Product(int? id)
        {
            var Prdoduct = await iProductRepository.GetProductByProductId(id);

            return View(Prdoduct);
        }

        [HttpPost]
        public JsonResult ShopAddCart(string ProductId, string Count)
        {
            ShoppingCartVM shoppingCartModel = new ShoppingCartVM();

            ProductM objProduct = iProductRepository.GetSingleProductByProductId(ProductId);

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("CartCounter")))
            {
                //var cartItem = memoryCache.Get("CartItem");
                listOfShoppingCartModels = memoryCache.Get("CartItem") as List<ShoppingCartVM>;

            }


            if (listOfShoppingCartModels.Any(model => model.ProductId.ToString() == ProductId))
            {
                shoppingCartModel = listOfShoppingCartModels.Single(model => model.ProductId.ToString() == ProductId);
                shoppingCartModel.Quantity = shoppingCartModel.Quantity + 1;
                shoppingCartModel.Total = shoppingCartModel.Quantity * shoppingCartModel.UnitPrice;


            }
            else
            {
                shoppingCartModel.ProductId = Convert.ToInt32(ProductId);
                shoppingCartModel.ImagePath = objProduct.ProductPicUrl;
                shoppingCartModel.ProductName = objProduct.ProductName;
                shoppingCartModel.Quantity = 1;
                shoppingCartModel.UnitPrice = objProduct.ProductPrice;
                shoppingCartModel.Total = objProduct.ProductPrice;

                listOfShoppingCartModels.Add(shoppingCartModel);

            }

            HttpContext.Session.SetString("CartCounter", listOfShoppingCartModels.Count.ToString());
            memoryCache.Set("CartItem", listOfShoppingCartModels);

            string count_ = listOfShoppingCartModels.Count.ToString();

            ShoppingCart();


            var result = new { ProductId = ProductId, Count = count_ };


            return Json(result);
        }
        public IActionResult CustomerViewComponent()
        {
            return ViewComponent("Customer");
        }

        public void GetShoppingCarts()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("CartCounter")))
            {
                listOfShoppingCartModels = memoryCache.Get("CartItem") as List<ShoppingCartVM>;

            }

            ViewBag.shoppingCarts = listOfShoppingCartModels;
        }
        public async Task<IActionResult> ShoppingCart()
        {

            GetShoppingCarts();

            return View();
        }
        [HttpPost]
        public JsonResult ManageProductCartQty(string ProductId, int Qty)
        {
            listOfShoppingCartModels = memoryCache.Get("CartItem") as List<ShoppingCartVM>;

            ShoppingCartVM? shoppingcart = listOfShoppingCartModels.Where(i => i.ProductId.ToString() == ProductId).FirstOrDefault();

            if (shoppingcart != null)
            {
                shoppingcart.Quantity = Qty;

                memoryCache.Set("CartItem", listOfShoppingCartModels);
            }

            return Json(new { ProductId, Qty });
        }
        public async Task<IActionResult> Checkout()
        {

            CustomerM customer = memoryCache.Get("LoggedCustomer") as CustomerM;

            if (customer == null || customer.CustomerId == 0)
            {
                return RedirectToAction("Login", "Customer");
            }
            else
            {
                GetShoppingCarts();
                return View(customer);
            }
            //return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}