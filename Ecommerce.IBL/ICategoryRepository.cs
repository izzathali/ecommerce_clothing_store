using Ecommerce.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.IBL
{
    public interface ICategoryRepository
    {
        public Task<IEnumerable<CategoryM>> GetAllCategoriesAsync();
        public IEnumerable<CategoryM> GetAllCategories();

        public Task<CategoryM> GetCategoryByCategoryId(int? categoryId);
        public Task<int> AddCategory(CategoryM category);
        public Task<int> UpdateCategory(CategoryM category);
        public Task<int> DeleteCategory(int categoryId);
        public bool CategoryExists(int id);

    }
}
