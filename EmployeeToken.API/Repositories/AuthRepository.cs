using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using EmployeeToken.API.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EmployeeToken.API.Repositories
{
    public class AuthRepository : IDisposable
    {
        private AuthDbContext context = null;
        private UserManager<IdentityUser> userManager = null;

        public AuthRepository()
        {
            context = new AuthDbContext();
            userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(context));
        }

        public void Dispose()
        {
            if (context != null)
            {
                context.Dispose();
            }
            if (userManager != null)
            {
                userManager.Dispose();
            }
        }

        //Register New User
        public async Task<IdentityResult> RegisterUser(UserModel userModel)
        {
            IdentityUser user = new IdentityUser
            {
                Email = userModel.Email,
                UserName = userModel.Email
            };

            return await userManager.CreateAsync(user, userModel.Password);
        }

        //Login with Existing User
        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            return await userManager.FindAsync(userName, password);
        }

    }

}