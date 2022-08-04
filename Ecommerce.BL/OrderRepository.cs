using Ecommerce.Data;
using Ecommerce.IBL;
using Ecommerce.Model;
using Ecommerce.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.BL
{
    public class OrderRepository : IOrderRepository
    {
        private readonly EcommerceDbContext db;
        public OrderRepository(EcommerceDbContext _db)
        {
            db = _db;
        }

        public async Task<int> AddOrder(OrderM order)
        {
            db.Orders.Add(order);
            return await db.SaveChangesAsync();
        }

        public async Task<int> AddOrderLineItem(OrderLineItemM orderLineItemM)
        {
            db.OrderLineItems.Add(orderLineItemM);
            return await db.SaveChangesAsync();
        }

        public async Task<int> DeleteOrder(int orderId)
        {
            var order = await db.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.IsDeleted = true;
                db.Orders.Update(order);
            }
            return await db.SaveChangesAsync();

        }

        public async Task<IEnumerable<OrderM>> GetAllOrders()
        {
            return await db.Orders.Where(i=>i.IsDeleted == false).Include(o => o.customer).ToListAsync();
        }

        public async Task<IEnumerable<MonthlyIncomeVM>> GetMonthlyIncome()
        {
            var model = await db.Orders.Where(i => i.IsDeleted == false)
               .GroupBy(o => new
               {
                   Month = o.OrderDate.Month,
                   Year = o.OrderDate.Year
               })
               .Select(g => new MonthlyIncomeVM
               {
                   Month = g.Key.Month,
                   Year = g.Key.Year,
                   NoOfOrders = g.Count(),
                   NoOfProduct = g.Sum(i => i.NoOfProducts),
                   TotalAmount = g.Sum(i => i.TotalAmount)
               })
               .OrderByDescending(a => a.Year)
               .ThenByDescending(a => a.Month)
               .ToListAsync();

            return model;
        }

        public async Task<OrderM> GetOrderByOrderId(int? orderId)
        {
            return await db.Orders.Where(i => i.IsDeleted == false)
                .Include(o => o.customer)
                .FirstOrDefaultAsync(d => d.OrderId == orderId);
        }

        public async Task<IEnumerable<OrderLineItemM>> GetOrderLineItemsByOrderId(int? orderId)
        {
            return await db.OrderLineItems.Include(o => o.product).Where(m => m.OrderId == orderId && m.IsDeleted == false).ToListAsync();
        }

        public bool OrderExists(int orderId)
        {
            return (db.Orders?.Any(e => e.OrderId == orderId)).GetValueOrDefault();
        }

        public async Task<int> UpdateOrder(OrderM orderM)
        {
            db.Orders.Update(orderM);
            return await db.SaveChangesAsync();
        }
    }
}
