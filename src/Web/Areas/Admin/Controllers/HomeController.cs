using Core;
using Models;
using Models.Dao;
using Models.Domains;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        
        public ActionResult Index()
        {
            
            return View();
        }
                
    }
}
