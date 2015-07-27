using Microsoft.Practices.Unity;
using Models.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using Web.App_Start;
using Web.Areas.Admin.Models;


namespace Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            
            ViewEngines.Engines.Add(new Web.Areas.Admin.Models.View.CrudViewEngine());
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();                    
        }

       
        public IAuth Auth
        {
            
            get
            {
                return UnityConfig.GetConfiguredContainer().Resolve<IAuth>();
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                var usr = Auth.GetUserDetails(ticket.Name);

                var permissions = Auth.GetUserPermissions(ticket.Name);
                var usrDetails = new  { UserName = usr.UserName,
                                        IpAddress = usr.IpAddress,
                                        Permissions = permissions
                                        };

                UserIdentity identity = new UserIdentity(usrDetails);

                UserPrincipal principal = new UserPrincipal(identity);
                HttpContext.Current.User = principal;
            }
        }
    }
}