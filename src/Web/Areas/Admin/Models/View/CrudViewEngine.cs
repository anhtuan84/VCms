using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Areas.Admin.Models.View
{
    public class CrudViewEngine : RazorViewEngine
    {
        public  const String ActionKey = "action";
        public  const String ControllerKey = "controller";
        public const String EntityKey = "entity";

        public CrudViewEngine()
        {
            AreaViewLocationFormats =
                    new string[]
                    {                       
                        "~/Areas/{2}/Views/{1}/{0}.cshtml",
                        "~/Areas/{2}/Views/Crud/{0}.cshtml",
                        "~/Areas/{2}/Views/Shared/{0}.cshtml"
                    };

        
        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            var routeData = controllerContext.RequestContext.RouteData;
            if (routeData != null && routeData.Values.ContainsKey("entity"))
            {
                
                //Set view path
                string viewPath = String.Format("~/Areas/Admin/Views/{1}/{0}.cshtml", viewName, routeData.Values["entity"]);
                string masterPath = "~/Areas/Admin/Views/Shared/_Layout.cshtml";
                if (VirtualPathProvider.FileExists(viewPath))
                {
                    return new ViewEngineResult(
                                     this.CreateView(controllerContext, viewPath, masterPath), this);
                }
                
            }
           
            return base.FindView(controllerContext, viewName, masterName, useCache: false);
        }

       
      
    }
}