using System.Threading.Tasks;
using System.Web.Http;
using EmployeeToken.API.Models;
using EmployeeToken.API.Repositories;


namespace TokenBased.API.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private AuthRepository repo = null;

        public AccountController()
        {
            repo = new AuthRepository();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (repo != null)
                {
                    repo.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        //POST : api/Account/Register
        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await repo.RegisterUser(userModel);
            if (result == null)
            {
                return InternalServerError();
            }

            return Ok();
        }
    }
}
