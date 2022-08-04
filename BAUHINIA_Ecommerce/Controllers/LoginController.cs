using Ecommerce.IBL;
using Ecommerce.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace BAUHINIA_Ecommerce.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        IMemoryCache memoryCache;
        private readonly IUserRepository iUserRepository;
        public LoginController(ILogger<LoginController> logger, IMemoryCache memoryCache,IUserRepository userRepository)
        {
            _logger = logger;
            this.memoryCache = memoryCache;
            iUserRepository = userRepository;
        }

        //Loggin
        public IActionResult Index(string? Error)
        {
            UserM u = new UserM();
            memoryCache.Set("LoggedUser", u);

            string Msg = "";
            if (Error == "True")
            {
                Msg = "Invalid Username or Password";
            }
            ViewBag.InvalidLogin = Msg;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("Username,Password")] UserM user)
        {
            if (ModelState.IsValid)
            {
                var User_ = await iUserRepository.CheckUsernameAndPassword(user);

                if (User_ != null)
                {
                    UserM u = new UserM();
                    u.UserId = User_.UserId;
                    u.Username = User_.Username;
                    u.FirstName = User_.FirstName;
                    u.LastName = User_.LastName;
                    u.AccessLevel = User_.AccessLevel;
                    u.UserType = User_.UserType;

                    memoryCache.Set("LoggedUser", u);

                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {

                    return RedirectToAction(nameof(Index), "Login", new { Error = "True" });
                }

            }
            return View(User);
        }

        //Register
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("FirstName,LastName,Username,Password")] UserM user)
        {
            if (ModelState.IsValid)
            {
                user.UserType = "Staff";
                user.AccessLevel = "Report";

                await iUserRepository.AddUser(user);

                return RedirectToAction(nameof(Index));
            }

            return View(User);
        }
    }
}
