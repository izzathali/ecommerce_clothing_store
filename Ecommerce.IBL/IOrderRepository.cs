using Ecommerce.Model;
using Ecommerce.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.IBL
{
    public interface IOrderRepository
    {
        public Task<IEnumerable<OrderM>> GetAllOrders();
        public Task<OrderM> GetOrderByOrderId(int? orderId);
        public Task<IEnumerable<OrderLineItemM>> GetOrderLineItemsByOrderId(int? orderId);
        public Task<IEnumerable<MonthlyIncomeVM>> GetMonthlyIncome();

        public Task<int> AddOrder(OrderM order);
        public Task<int> AddOrderLineItem(OrderLineItemM orderLineItemM);
        public Task<int> UpdateOrder(OrderM orderM);
        public Task<int> DeleteOrder(int orderId);
        public bool OrderExists(int orderId);

    }
}
