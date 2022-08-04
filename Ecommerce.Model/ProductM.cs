using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Model
{
    public class ProductM
    {
        [Key]
        public int ProductId { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Product Code")]
        public string ProductCode { get; set; } = string.Empty;
        [Column(TypeName = "nvarchar(150)")]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; } = string.Empty;
        [Column(TypeName = "nvarchar(MAX)")]
        [Display(Name = "Description")]
        public string ProductDescription { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public CategoryM? category { get; set; }
        [Display(Name = "Price")]
        public decimal ProductPrice { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        [Display(Name = "Picture")]
        public string ProductPicUrl { get; set; } = string.Empty;
        [NotMapped]
        [Display(Name = "Upload File")]
        public IFormFile? ProductPicFile { get; set; }
        public int Quantity { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
