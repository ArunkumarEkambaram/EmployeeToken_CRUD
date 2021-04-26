using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EmployeeToken.MVC.Startup))]
namespace EmployeeToken.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
