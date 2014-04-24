using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using WebMatrix.WebData;

namespace LearningRace
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            WebSecurity.InitializeDatabaseConnection(LR.Data.DataContext.ConnectionStringName, "UserProfile", "UserId", "UserName", true);
            //return;

            if (!Roles.RoleExists("admin"))
            {
                Roles.CreateRole("admin");
            }
            if (!WebSecurity.UserExists("admin"))
            {
                WebSecurity.CreateUserAndAccount("admin", "123123");
            }
            if (!Roles.GetRolesForUser("admin").Contains("admin"))
            {
                Roles.AddUsersToRoles(new[] { "admin" }, new[] { "admin" });
            }
        }
    }
}