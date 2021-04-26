using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using EmployeeToken.API.Repositories;
using Microsoft.Owin.Security.OAuth;

namespace EmployeeToken.API.Providers
{
    public class OAuthServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            await Task.Run(() => context.Validated());
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Allow-Control-Allow-Origin", new[] { "*" });
            using (AuthRepository repo = new AuthRepository())
            {
                var user = await repo.FindUser(context.UserName, context.Password);
                if (user == null)
                {
                    context.SetError("Invalid Grant", "Username or Password is incorrect");
                    return;
                }
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim("User", context.UserName));
                identity.AddClaim(new Claim("Role", "User"));
                context.Validated(identity);
            }
        }
    }
}