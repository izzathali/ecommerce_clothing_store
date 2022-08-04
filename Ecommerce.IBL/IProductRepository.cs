using Ecommerce.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.IBL
{
    public interface IProductRepository
    {
        public Task<IEnumerable<ProductM>> GetAllProducts();
        public IEnumerable<ProductM> GetProductTop(int Top);

        public Task<ProductM> GetProductByProductId(int? productId);
        public ProductM GetSingleProductByProductId(string productId);

        public Task<int> AddProudct(ProductM product);
        public Task<int> UpdateProduct(ProductM product);
        public Task<int> DeleteProduct(int productId);
        public bool ProductExists(int id);
    }
}
