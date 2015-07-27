using Core;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Areas.Admin.Models;
namespace Web.Areas.Admin.Controllers
{
    [BoAuthorization(Url = "/Admin/Auth")]
    public class BaseController : Controller
    {
        
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {

            base.OnActionExecuted(filterContext);
            var result = filterContext.Result as ViewResult;
            if (result != null)
            {
                result.MasterName = "_Layout";
            }
        }


        [ChildActionOnly]
        public PartialViewResult MainMenu()
        {
            var types = EntityHelper.ScanEntities();
            dynamic model = new ExpandoObject();
            model.entities = from e in types
                             select e.Value;
            return PartialView(model);

         }
    }
}
