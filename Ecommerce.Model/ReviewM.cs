using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Model
{
    public class ReviewM
    {
        [Key]
        public int ReviewId { get; set; }
        public string ReviewDescription { get; set; }
        public int Rate { get; set; }
        public int ProductId { get; set; }
        public ProductM product { get; set; }
        public int CustomerId { get; set; }
        public CustomerM customer { get; set; }
    }
}
