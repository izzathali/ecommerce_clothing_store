using Ecommerce.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace BAUHINIA_Ecommerce.ViewComponents
{
    [ViewComponent(Name = "DashboardSidebar")]
    public class DashboardSidebarViewComponent : ViewComponent
    {
        IMemoryCache memoryCache;
        public DashboardSidebarViewComponent(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache; ;

        }

        public IViewComponentResult Invoke()
        {
            UserM Usr = new UserM();
            Usr = memoryCache.Get("LoggedUser") as UserM;

            return View(Usr);
        }



    }
}
