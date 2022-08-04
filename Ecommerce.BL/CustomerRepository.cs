using Ecommerce.Data;
using Ecommerce.IBL;
using Ecommerce.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.BL
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly EcommerceDbContext db;
        public CustomerRepository(EcommerceDbContext db)
        {
            this.db = db;
        }

        public async Task<int> AddCustomer(CustomerM customerM)
        {
            db.Customers.Add(customerM);
            return await db.SaveChangesAsync();

        }

        public bool CustomerExists(int id)
        {
            return (db.Customers?.Any(e => e.CustomerId == id)).GetValueOrDefault();
        }

        public async Task DeleteCustomer(int customerId)
        {
            var customer = await db.Customers.FindAsync(customerId);

            if (customer != null)
            {
                customer.IsDeleted = true;
                db.Customers.Update(customer);
            }

            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<CustomerM>> GetAllCustomers()
        {
            return await (from cus in db.Customers.Where(i=>i.IsDeleted == false)
                    select new CustomerM
                    {
                        CustomerId = cus.CustomerId,
                        CustomerName = cus.CustomerName,
                        DeliveryAddress = cus.DeliveryAddress,
                        Email = cus.Email,
                        PhoneNo1 = cus.PhoneNo1,
                        PhoneNo2 = cus.PhoneNo2
                    }
                                ).ToListAsync();
        }

        public async Task<CustomerM> GetCustomerByCustomerId(int? CustomerId)
        {
            return await db.Customers.Where(i => i.IsDeleted == false).FirstOrDefaultAsync(i => i.CustomerId == CustomerId);

        }

        public async Task<CustomerM> GetCustomerByEmailAndPassword(CustomerM customerM)
        {
            return await db.Customers.Where(i => i.Email == customerM.Email && i.Password == customerM.Password && i.IsDeleted == false).FirstOrDefaultAsync();
        }

        public async Task<int> UpdateCustomer(CustomerM customerM)
        {
            db.Update(customerM);
            return await db.SaveChangesAsync();
        }
    }
}
