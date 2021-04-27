using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using EmployeeToken.API.Models;

namespace EmployeeToken.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/Employees")]
    public class EmployeesController : ApiController
    {
        private readonly ProjectsEntities db = new ProjectsEntities();

        // GET: api/Employees
        [Route("GetAll")]
        public IQueryable<Employee> GetEmployees()
        {
            return db.Employees;
        }

        // GET: api/Employees/5        
        [Route("GetEmployeeBy/{id}", Name = "GetEmployeeBy")]
        [ResponseType(typeof(Employee))]
        public async Task<IHttpActionResult> GetEmployee(int id)
        {
            Employee employee = await db.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [Route("SearchBy/{name}")]
        public IQueryable<Employee> GetEmployees(string name)
        {
            var employees = db.Employees.Where(x => x.Name.StartsWith(name));
            return employees;
        }

        // PUT: api/Employees/5
        [Route("UpdateEmployee/{id}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> UpdateEmployee(int id, Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employee.Id)
            {
                return BadRequest();
            }

            db.Entry(employee).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Employees
        [Route("AddNew")]
        [HttpPost]
        [ResponseType(typeof(Employee))]
        public async Task<IHttpActionResult> AddNewEmployee(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Employees.Add(employee);
            await db.SaveChangesAsync();

             return CreatedAtRoute("GetEmployeeBy", new { id = employee.Id }, employee);
            //return CreatedAtRoute("DefaultApi",)
        }

        // DELETE: api/Employees/5
        [Route("DeleteEmployee/{id}")]
        [HttpDelete]
        [ResponseType(typeof(Employee))]
        public async Task<IHttpActionResult> DeleteEmployee(int id)
        {
            Employee employee = await db.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            db.Employees.Remove(employee);
            await db.SaveChangesAsync();

            return Ok(employee);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmployeeExists(int id)
        {
            return db.Employees.Count(e => e.Id == id) > 0;
        }
    }
}