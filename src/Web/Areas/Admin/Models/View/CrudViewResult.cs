using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Areas.Admin.Models.View
{
    public class CrudViewResult<T> : ViewResult where T:AbsEntity
    {
        protected override ViewEngineResult FindView(ControllerContext context)
        {
            string name = "";

            ViewEngineResult result = ViewEngines.Engines.FindView(context, name, null);

            if (result.View != null)
            {
                return result;
            }

            return base.FindView(context);
        }
    }
}