using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmployeeToken.API.Models
{
    public class UserModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

       // [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must atleast 6 character length")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirm Password doesn't match")]
        public string ConfirmPassword { get; set; }
    }
}