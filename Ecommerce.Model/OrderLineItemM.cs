using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Model
{
    public class OrderLineItemM
    {
        [Key]
        public int OrderLineItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public ProductM? product { get; set; }
        public int Quantity { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal Total { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
