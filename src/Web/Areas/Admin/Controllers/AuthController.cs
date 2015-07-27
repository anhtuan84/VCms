using Microsoft.Practices.Unity;
using Models.Domains;
using Models.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Web.Models;

namespace Web.Areas.Admin.Controllers
{
    public class AuthController : Controller
    {
       [Dependency]
        public IAuth Auth
        {
            set;
            private get;
        }
        
        public ActionResult Index()
        {
            return View("Login");
        }       


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
        
            try
            {
            
                Boolean authenticated = false;
                authenticated = Auth.Authenticate(model.UserName, model.Password, null, true);               
            
                if (authenticated)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);                   
                    return RedirectToAction("Index", "Home");               
                }
            }
            catch (Exception exception)
            {
                ViewBag.HasError = true;
                ModelState.AddModelError("", exception.Message);
            }          
            return View(model);
        }



    }
}
