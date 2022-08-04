using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Model
{
    public class UserM
    {
        [Key]
        public int UserId { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;
        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;
        [Column(TypeName = "nvarchar(200)")]
        public string Username { get; set; } = string.Empty;
        [Column(TypeName = "nvarchar(200)")]
        public string Password { get; set; } = string.Empty;
        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "User Type")]
        public string UserType { get; set; } = string.Empty;
        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Access Level")]
        public string AccessLevel { get; set; } = string.Empty;
        [Column(TypeName = "nvarchar(MAX)")]
        [Display(Name = "Profile")]
        public string? UserPrfUrl { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
