using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
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
        IEnumerable<EmployeeViewModel> emps = null;
        public EmployeesController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["api"]);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<bool> GetAll()
        {
            //Apply token to bearer authorization
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GlobalVariables.BearerToken);

            //Calling GetAll method from API
            var result = await client.GetAsync("Employees/GetAll");

            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                Request.GetOwinContext().Authentication.SignOut(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie);                
            }

            if (result.IsSuccessStatusCode)
            {
                emps = await result.Content.ReadAsAsync<IEnumerable<EmployeeViewModel>>();
                return true;
            }
            return false;
        }

        // GET: Employees
        public async Task<ActionResult> Index()
        {
            var status = await GetAll();
            if (status)
            {
                return View(emps);
            }
            ModelState.AddModelError(string.Empty, "Server Error");
            return View();
        }

        public async Task<ActionResult> Search(string search)
        {
            //Apply token to bearer authorization
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GlobalVariables.BearerToken);

            if (string.IsNullOrWhiteSpace(search))
            {
                var status = await GetAll();
                if (status)
                {
                    return PartialView("_Employee", emps);
                }
                ModelState.AddModelError(string.Empty, "Server Error");
                return View();
            }

            var result = await client.GetAsync($"Employees/SearchBy/{search}");

            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                Request.GetOwinContext().Authentication.SignOut(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie);
            }

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

        public async Task<ActionResult> AddEditEmployee(int id = 0)
        {
            if (id != 0)
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GlobalVariables.BearerToken);

                var result = await client.GetAsync($"Employees/GetEmployeeBy/{id}");
                if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Request.GetOwinContext().Authentication.SignOut(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie);
                }
                if (result.IsSuccessStatusCode)
                {
                    var employee = await result.Content.ReadAsAsync<AddEmployeeViewModel>();
                    return View(employee);
                }               
            }
            return View(new AddEmployeeViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> AddEditEmployee(int id,AddEmployeeViewModel employee)
        {
            if(!ModelState.IsValid)
            {
                return View(employee);
            }

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GlobalVariables.BearerToken);

            if (employee.Id == 0)
            {                
                var result = await client.PostAsJsonAsync("Employees/AddNew", employee);
                if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Request.GetOwinContext().Authentication.SignOut(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie);
                }
                if (result.StatusCode == HttpStatusCode.Created)
                {
                    return RedirectToAction("Index");
                }
            }
            var resultEdit = await client.PutAsJsonAsync($"Employees/UpdateEmployee/{id}", employee);
            if (resultEdit.StatusCode == HttpStatusCode.NoContent)
            {
                return RedirectToAction("Index");
            }
            return View(employee);
        }
    }
}