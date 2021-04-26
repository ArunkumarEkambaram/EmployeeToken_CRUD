using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace EmployeeToken.MVC
{
    public class GlobalVariables
    {
        public static string BearerToken
        {
            get { return ((ClaimsPrincipal)System.Web.HttpContext.Current.User).FindFirst("AcessToken").Value; }
        }


    }
}