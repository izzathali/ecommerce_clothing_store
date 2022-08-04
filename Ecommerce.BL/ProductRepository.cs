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
    public class ProductRepository : IProductRepository
    {
        private readonly EcommerceDbContext db;
        public ProductRepository(EcommerceDbContext _db)
        {
            db = _db;
        }

        public async Task<int> AddProudct(ProductM product)
        {
            db.Products.Add(product);
            return await db.SaveChangesAsync();
        }

        public async Task<int> DeleteProduct(int productId)
        {
            var product = db.Products.FirstOrDefault(x => x.ProductId == productId);

            if (product != null)
            {
                product.IsDeleted = true;
                db.Products.Update(product);
            }

            return await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductM>> GetAllProducts()
        {
            IEnumerable<ProductM> products = await db.Products.Where(i=>i.IsDeleted == false).Include(m => m.category).ToListAsync();

            return (products);
        }

        public async Task<ProductM> GetProductByProductId(int? productId)
        {
            var product = await db.Products.Where(i => i.IsDeleted == false).Include(m => m.category).FirstOrDefaultAsync(i => i.ProductId == productId);

            return (product);
        }

        public IEnumerable<ProductM> GetProductTop(int Top)
        {
            return db.Products.Where(i => i.IsDeleted == false).Include(m => m.category).Take(Top).ToList();
        }

        public ProductM GetSingleProductByProductId(string productId)
        {
            return db.Products.Where(i => i.IsDeleted == false).Single(model => model.ProductId.ToString() == productId);
        }

        public bool ProductExists(int id)
        {
            return (db.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }

        public async Task<int> UpdateProduct(ProductM product)
        {
            db.Products.Update(product);
            return await db.SaveChangesAsync();
        }
    }
}
