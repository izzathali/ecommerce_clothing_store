using Ecommerce.Model;
using Ecommerce.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace BAUHINIA_Ecommerce.ViewComponents
{
    [ViewComponent(Name = "Customer")]
    public class CustomerViewComponent : ViewComponent
    {
        IMemoryCache memoryCache;
        public CustomerViewComponent(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public IViewComponentResult Invoke()
        {
            CustomerM cus = new CustomerM();
            cus = memoryCache.Get("LoggedCustomer") as CustomerM;

            List<ShoppingCartVM> listOfShoppingCartModels = new List<ShoppingCartVM>();
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("CartCounter")))
            {
                listOfShoppingCartModels = memoryCache.Get("CartItem") as List<ShoppingCartVM>;

            }

            ViewBag.shoppingCarts = listOfShoppingCartModels;

            return View(cus);
        }
    }

}

