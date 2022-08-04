using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Ecommerce.Model;
using Ecommerce.IBL;
using Ecommerce.ViewModel;

namespace BAUHINIA_Ecommerce.Controllers
{
    public class UserController : Controller
    {
        //private readonly EcommerceDbContext _context;
        IMemoryCache memoryCache;
        private UserM Usr = new UserM();
        List<UserTypeVM> userTypes = new List<UserTypeVM>();
        List<AccessTypeVM> accessTypes = new List<AccessTypeVM>();

        private readonly IUserRepository iUserRepository;

        public UserController(IMemoryCache memoryCache,IUserRepository _iUserRepository)
        {
            //_context = context;
            iUserRepository = _iUserRepository;
            this.memoryCache = memoryCache;
            Usr = memoryCache.Get("LoggedUser") as UserM;

            userTypes.Add(new UserTypeVM { UserTypeName = "Staff" });
            userTypes.Add(new UserTypeVM { UserTypeName = "Admin" });

            accessTypes.Add(new AccessTypeVM { AccessLevel = "Report" });
            accessTypes.Add(new AccessTypeVM { AccessLevel = "Update" });
            accessTypes.Add(new AccessTypeVM { AccessLevel = "Update & Delete" });
            accessTypes.Add(new AccessTypeVM { AccessLevel = "Complete System" });

        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }

            return View(await iUserRepository.GetAllUsers());

        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            if (id == null || await iUserRepository.GetAllUsers() == null)
            {
                return RedirectToAction("PageNotFound","Error");
            }

            var user = await iUserRepository.GetUserByUserId(id);

            if (user == null)
            {
                return RedirectToAction("PageNotFound","Error");
            }

            return View(user);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }

            ViewData["UserType"] = new SelectList(userTypes, "UserTypeName", "UserTypeName");

            ViewData["AccessType"] = new SelectList(accessTypes, "AccessLevel", "AccessLevel");



            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,FirstName,LastName,Username,Password,UserType,AccessLevel,UserPrfUrl")] UserM user)
        {         

            if (ModelState.IsValid)
            {
                //_context.Add(user);
                //await _context.SaveChangesAsync();

                await iUserRepository.AddUser(user);

                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            if (id == null || await iUserRepository.GetAllUsers() == null)
            {
                return RedirectToAction("PageNotFound","Error");
            }

            ViewData["UserType"] = new SelectList(userTypes, "UserTypeName", "UserTypeName");

            ViewData["AccessType"] = new SelectList(accessTypes, "AccessLevel", "AccessLevel");

            //var user = await _context.Users.FindAsync(id);
            var user = await iUserRepository.GetUserByUserId(id);

            if (user == null)
            {
                return RedirectToAction("PageNotFound","Error");
            }
            return View(user);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,FirstName,LastName,Username,Password,UserType,AccessLevel,UserPrfUrl")] UserM user)
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            if (id != user.UserId)
            {
                return RedirectToAction("PageNotFound","Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(user);
                    //await _context.SaveChangesAsync();

                    await iUserRepository.UpdateUser(user);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
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
            return View(user);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            if (id == null || await iUserRepository.GetAllUsers() == null)
            {
                return RedirectToAction("PageNotFound","Error");
            }

            var user = await iUserRepository.GetUserByUserId(id);

            if (user == null)
            {
                return RedirectToAction("PageNotFound","Error");
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            if (await iUserRepository.GetAllUsers() == null)
            {
                return Problem("Entity set 'BauhiniaEcomDbContext.Users'  is null.");
            }

            await iUserRepository.DeleteUser(id);

            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
          return iUserRepository.UserExists(id);
        }
    }
}
