using Ecommerce.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Data
{
    public class EcommerceDbContext : DbContext
    {
        public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options) : base(options)
        {

        }

        public DbSet<CategoryM> Categories { get; set; }
        public DbSet<ProductM> Products { get; set; }
        public DbSet<CustomerM> Customers { get; set; }
        public DbSet<UserM> Users { get; set; }
        public DbSet<OrderM> Orders { get; set; }
        public DbSet<OrderLineItemM> OrderLineItems { get; set; }

        public DbSet<ReviewM> Reviews { get; set; }
    }
}
