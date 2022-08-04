using Microsoft.AspNetCore.Mvc;

namespace BAUHINIA_Ecommerce.Controllers
{
    public class ErrorController : Controller
    {
        public ErrorController()
        {

        }
        [Route("404")]
        public IActionResult PageNotFound()
        {
            return View();
        }
    }
}
