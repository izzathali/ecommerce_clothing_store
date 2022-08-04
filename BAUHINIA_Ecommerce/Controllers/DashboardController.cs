using AspNetCoreHero.ToastNotification.Abstractions;
using Ecommerce.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace BAUHINIA_Ecommerce.Controllers
{
    public class DashboardController : Controller
    {
        IMemoryCache memoryCache;
        private UserM Usr = new UserM();
        private readonly INotyfService _notyf;

        public DashboardController(IMemoryCache memoryCache, INotyfService notyf)
        {
            this.memoryCache = memoryCache;
            Usr = memoryCache.Get("LoggedUser") as UserM;
            _notyf = notyf;
        }
        public IActionResult Index()
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
    }
}
