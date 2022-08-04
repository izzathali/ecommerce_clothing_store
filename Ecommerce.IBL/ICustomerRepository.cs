using Ecommerce.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.IBL
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<CustomerM>> GetAllCustomers();
        Task<CustomerM> GetCustomerByCustomerId(int? CustomerId);
        Task<CustomerM> GetCustomerByEmailAndPassword(CustomerM customerM);
        Task<int> AddCustomer(CustomerM customerM);
        Task<int> UpdateCustomer(CustomerM customerM);
        Task DeleteCustomer(int customerId);

        bool CustomerExists(int id);
    }
}
