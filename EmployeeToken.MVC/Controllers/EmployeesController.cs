using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EmployeeToken.MVC.ViewModels;

namespace EmployeeToken.MVC.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly HttpClient client = null;
        public EmployeesController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["api"]);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
        // GET: Employees
        public async Task<ActionResult> Index()
        {
            ViewBag.Heading = "List of Employees";
            ////Apply token to bearer authorization
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GlobalVariables.BearerToken);

            //Calling GetAll method from API
            var result = await client.GetAsync("Employees/GetAll");

            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                Request.GetOwinContext().Authentication.SignOut(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie);
               // return RedirectToAction("Login", "Account");
            }

            if (result.IsSuccessStatusCode)
            {
                var employees = await result.Content.ReadAsAsync<IEnumerable<EmployeeViewModel>>();
                return View(employees);
            }
            else
            {
                ViewBag.Heading = "";
                ModelState.AddModelError(string.Empty, "Server Error");
                return View();
            }                        
        }
    
        public async Task<ActionResult> Search(string search)
        {
            //Apply token to bearer authorization
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GlobalVariables.BearerToken);

            if(string.IsNullOrWhiteSpace(search))
            {
                var result1 = await client.GetAsync("Employees/GetAll");

                if (result1.IsSuccessStatusCode)
                {
                    var employees = await result1.Content.ReadAsAsync<IEnumerable<EmployeeViewModel>>();
                    return PartialView("_Employee", employees);
                }
            }

            var result = await client.GetAsync($"Employees/SearchBy/{search}");
            if (result.IsSuccessStatusCode)
            {
                var employees = await result.Content.ReadAsAsync<IEnumerable<EmployeeViewModel>>();
                return PartialView("_Employee", employees);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server Error");
                return null;
            }
        }
    }
}