using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Ecommerce.Model;
using Ecommerce.IBL;
using Ecommerce.ViewModel;

namespace BAUHINIA_Ecommerce.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private List<ShoppingCartVM> listOfShoppingCartModels;
        IMemoryCache memoryCache;
        private UserM Usr = new UserM();

        private readonly ICustomerRepository iCustomerRepository;

        public CustomerController(ILogger<HomeController> logger, IMemoryCache memoryCache, ICustomerRepository _iCustomerRepository)
        {
            _logger = logger;
            //db = context;
            listOfShoppingCartModels = new List<ShoppingCartVM>();
            this.memoryCache = memoryCache;
            Usr = memoryCache.Get("LoggedUser") as UserM;
            iCustomerRepository = _iCustomerRepository;
        }
        public void GetShoppingCarts()
        {

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("CartCounter")))
            {
                listOfShoppingCartModels = memoryCache.Get("CartItem") as List<ShoppingCartVM>;

            }
            ViewBag.shoppingCarts = listOfShoppingCartModels;

        }
        // GET: Customer
        public async Task<IActionResult> Index()
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }

            return View(await iCustomerRepository.GetAllCustomers());
        }

        // GET: Customer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }

            if (id == null || await iCustomerRepository.GetAllCustomers() == null)
            {
                return RedirectToAction("PageNotFound","Error");
            }


            CustomerM customer = await iCustomerRepository.GetCustomerByCustomerId(id);

            if (customer == null)
            {
                return RedirectToAction("PageNotFound","Error");
            }

            return View(customer);
        }

        // GET: Customer/Create
        public IActionResult Create()
        {
            GetShoppingCarts();
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,CustomerName,Email,DeliveryAddress,Password,PhoneNo1,PhoneNo2")] CustomerM customer)
        {
            if (ModelState.IsValid)
            {
                await iCustomerRepository.AddCustomer(customer);

                return RedirectToAction(nameof(Login));
            }
            return View(customer);
        }
        //Loggin
        public IActionResult Login(string? Error)
        {
            if (Error == "True")
            {
                string Msg = "Invalid Username or Password";
                ViewBag.InvalidLogin = Msg;
            }
            CustomerM cus = new CustomerM();
            memoryCache.Set("LoggedCustomer", cus);

            GetShoppingCarts();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Email,Password")] CustomerM _customer)
        {
            if (ModelState.IsValid)
            {
                var customer = await iCustomerRepository.GetCustomerByEmailAndPassword(_customer);

                if (customer != null)
                {
                    CustomerM cus = new CustomerM();
                    cus = customer;

                    memoryCache.Set("LoggedCustomer", cus);

                    return RedirectToAction("Checkout", "Home");
                }
                else
                {

                    return RedirectToAction(nameof(Login), "Customer", new { Error = "True" });
                }

            }
            return View(_customer);
        }
        // GET: Customer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            if (id == null || await iCustomerRepository.GetAllCustomers() == null)
            {
                return RedirectToAction("PageNotFound","Error");
            }

            var customer = await iCustomerRepository.GetCustomerByCustomerId(id);

            if (customer == null)
            {
                return RedirectToAction("PageNotFound","Error");
            }
            return View(customer);
        }

        // POST: Customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,CustomerName,Email,DeliveryAddress,Password,PhoneNo1,PhoneNo2")] CustomerM customer)
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            if (id != customer.CustomerId)
            {
                return RedirectToAction("PageNotFound","Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await iCustomerRepository.UpdateCustomer(customer);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
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
            return View(customer);
        }

        // GET: Customer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            if (id == null || await iCustomerRepository.GetAllCustomers() == null)
            {
                return RedirectToAction("PageNotFound","Error");
            }

            //var customer = await db.Customers
            //    .FirstOrDefaultAsync(m => m.CustomerId == id);
            var customer = await iCustomerRepository.GetCustomerByCustomerId(id);

            if (customer == null)
            {
                return RedirectToAction("PageNotFound","Error");
            }

            return View(customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            if (await iCustomerRepository.GetAllCustomers() == null)
            {
                return Problem("Entity set 'BauhiniaEcomDbContext.Customers'  is null.");
            }

            await iCustomerRepository.DeleteCustomer(id);

            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return iCustomerRepository.CustomerExists(id);
        }


    }
}
