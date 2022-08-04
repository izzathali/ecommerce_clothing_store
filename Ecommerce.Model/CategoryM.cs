using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Model
{
    public class CategoryM
    {
        [Key]
        public int CategoryId { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
    }
}
