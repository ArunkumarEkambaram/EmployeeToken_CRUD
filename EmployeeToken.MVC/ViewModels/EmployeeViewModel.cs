using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeToken.MVC.ViewModels
{
    public class EmployeeViewModel
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public string Location { get; set; }
        public DateTime? BirthDate { get; set; }
        public decimal? Salary { get; set; }
    }
}