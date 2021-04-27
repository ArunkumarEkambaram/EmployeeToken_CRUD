using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmployeeToken.MVC.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Location { get; set; }
        public DateTime? BirthDate { get; set; }
        public decimal? Salary { get; set; }
    }

    public class AddEmployeeViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name cannot be left blank")]
        [RegularExpression("(.*[a-z]){3}", ErrorMessage = "Name should be atleast 3 character length")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Position cannot be left blank")]
        public string Position { get; set; }

        [Required(ErrorMessage = "Location cannot be left blank")]
        [RegularExpression("(.*[a-z]){3}", ErrorMessage = "Location should be atleast 3 character length")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Birth Date cannot be left blank")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? BirthDate { get; set; }

        [Required(ErrorMessage = "Salary cannot be left blank")]
        [Range(5000.00, 200000.00, ErrorMessage = "Salary amount between 5,000 and 2,00,000")]
        public decimal? Salary { get; set; }
    }
}