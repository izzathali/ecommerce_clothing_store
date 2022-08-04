using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Ecommerce.Model;
using Ecommerce.IBL;
using Ecommerce.ViewModel;

namespace BAUHINIA_Ecommerce.Controllers
{
    public class OrderController : Controller
    {
        IMemoryCache memoryCache;
        private UserM Usr = new UserM();
        private readonly IOrderRepository iOrderRepository;
        private readonly ICategoryRepository iCategoryRepository;
        public OrderController(IMemoryCache memoryCache,IOrderRepository _iOrderRepository, ICategoryRepository iCategoryRepository)
        {
            iOrderRepository = _iOrderRepository;
            this.memoryCache = memoryCache;
            Usr = memoryCache.Get("LoggedUser") as UserM;
            this.iCategoryRepository = iCategoryRepository;
        }

        // GET: Order
        public async Task<IActionResult> Index()
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }


            return View(await iOrderRepository.GetAllOrders());

        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }

            if (id == null || await iOrderRepository.GetAllOrders() == null)
            {
                return RedirectToAction("PageNotFound","Error");
            }

      
            var order = await iOrderRepository.GetOrderByOrderId(id);

            var orderLineItems = await iOrderRepository.GetOrderLineItemsByOrderId(id);

            ViewBag.OrderLineItems = orderLineItems;

            if (order == null)
            {
                return RedirectToAction("PageNotFound","Error");
            }

            return View(order);
        }

        // GET: Order/Create
        public async Task<IActionResult> Create()
        {
            CustomerM customer = memoryCache.Get("LoggedCustomer") as CustomerM;

            List<ShoppingCartVM> listOfShoppingCartModels = new List<ShoppingCartVM>();

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("CartCounter")))
            {
                listOfShoppingCartModels = memoryCache.Get("CartItem") as List<ShoppingCartVM>;

            }
            OrderM order = new OrderM();


            if (customer != null && listOfShoppingCartModels != null)
            {
                order.OrderDate = DateTime.Now;
                order.CustomerId = customer.CustomerId;
                decimal TotProducts = listOfShoppingCartModels.Select(m => m.Quantity).Sum();
                order.NoOfProducts = Convert.ToInt32(TotProducts);
                decimal total = listOfShoppingCartModels.Select(m => m.Total).Sum();
                order.TotalAmount = total;


                await iOrderRepository.AddOrder(order);


                foreach (ShoppingCartVM item in listOfShoppingCartModels)
                {
                    OrderLineItemM orderLineItem = new OrderLineItemM();
                    orderLineItem.OrderId = order.OrderId;
                    orderLineItem.ProductId = item.ProductId;
                    orderLineItem.ProductPrice = item.UnitPrice;
                    orderLineItem.Quantity = Convert.ToInt32(item.Quantity);
                    orderLineItem.Total = item.Total;

                    await iOrderRepository.AddOrderLineItem(orderLineItem);
                }

                listOfShoppingCartModels = new List<ShoppingCartVM>();

                HttpContext.Session.SetString("CartCounter", "0");
                memoryCache.Set("CartItem", listOfShoppingCartModels);

            }


            return View(order);
        }

        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,OrderDate,CustomerId")] OrderM order)
        {
            if (ModelState.IsValid)
            {
                await iOrderRepository.AddOrder(order);

                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(iCategoryRepository.GetAllCategories(), "CustomerId", "CustomerId", order.CustomerId);
            return View(order);
        }
        // GET: Monthly Income
        public async Task<IActionResult> Income()
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }

            var model = await iOrderRepository.GetMonthlyIncome();

            ViewBag.Incomes = model;
            return View();
        }


        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            if (id == null || await iOrderRepository.GetAllOrders() == null)
            {
                return RedirectToAction("PageNotFound","Error");
            }

            var order = await iOrderRepository.GetOrderByOrderId(id);

            if (order == null)
            {
                return RedirectToAction("PageNotFound","Error");
            }
            ViewData["CustomerId"] = new SelectList(iCategoryRepository.GetAllCategories(), "CustomerId", "CustomerId", order.CustomerId);
            return View(order);
        }

        // POST: Order/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,OrderDate,CustomerId")] OrderM order)
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }

            if (id != order.OrderId)
            {
                return RedirectToAction("PageNotFound","Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await iOrderRepository.UpdateOrder(order);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
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
            ViewData["CustomerId"] = new SelectList(iCategoryRepository.GetAllCategories(), "CustomerId", "CustomerId", order.CustomerId);
            return View(order);
        }

        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }

            if (id == null || await iOrderRepository.GetAllOrders() == null)
            {
                return RedirectToAction("PageNotFound","Error");
            }

            var order = await iOrderRepository.GetOrderByOrderId(id);

            if (order == null)
            {
                return RedirectToAction("PageNotFound","Error");
            }

            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (Usr == null || Usr.UserId == 0)
            {
                return RedirectToAction("Index", "Login");
            }

            if (await iOrderRepository.GetAllOrders() == null)
            {
                return Problem("Entity set 'BauhiniaEcomDbContext.Orders'  is null.");
            }

            await iOrderRepository.DeleteOrder(id);

            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return iOrderRepository.OrderExists(id);
        }
    }
}
