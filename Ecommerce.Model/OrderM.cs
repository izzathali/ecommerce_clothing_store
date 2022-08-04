using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Model
{
    public class OrderM
    {
        [Key]
        public int OrderId { get; set; }
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public CustomerM? customer { get; set; }
        [Display(Name = "No Of Products")]
        public int NoOfProducts { get; set; }
        [Display(Name = "Total")]
        public decimal TotalAmount { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
