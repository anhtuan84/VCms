using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Areas.Admin.Models
{
    public class BoAuthorization: AuthorizeAttribute
    {
        public string Url { get; set; }

        // redirect to login page with the original url as parameter.
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult(Url + "?returnUrl=" + filterContext.HttpContext.Request.Url.PathAndQuery);
        }
    }
}