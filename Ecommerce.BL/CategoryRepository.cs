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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly EcommerceDbContext db;
        public CategoryRepository(EcommerceDbContext _db)
        {
            this.db = _db;
        }
        public async Task<int> AddCategory(CategoryM category)
        {
            db.Categories.Add(category);
            return await db.SaveChangesAsync();
        }

        public bool CategoryExists(int id)
        {
            return (db.Categories?.Any(e => e.CategoryId == id)).GetValueOrDefault();
        }

        public async Task<int> DeleteCategory(int categoryId)
        {
            var customer = db.Categories.Find(categoryId);

            if (customer != null)
            {
                customer.IsDeleted = true;
                db.Categories.Update(customer);
            }

            return await db.SaveChangesAsync();
        }

        public IEnumerable<CategoryM> GetAllCategories()
        {
            return db.Categories.Where(i => i.IsDeleted == false).ToList();
        }

        public async Task<IEnumerable<CategoryM>> GetAllCategoriesAsync()
        {
            return await db.Categories.Where(i => i.IsDeleted == false).ToListAsync();
        }

        public async Task<CategoryM> GetCategoryByCategoryId(int? categoryId)
        {
            return await db.Categories.Where(i => i.IsDeleted == false).FirstOrDefaultAsync(i => i.CategoryId == categoryId);
        }

        public async Task<int> UpdateCategory(CategoryM category)
        {
            db.Categories.Update(category);
            return await db.SaveChangesAsync();
        }

    }
}
