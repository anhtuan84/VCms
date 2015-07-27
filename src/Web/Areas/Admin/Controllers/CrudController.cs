using Core;
using Models;
using Models.Dao;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Web.Areas.Admin.Models.View;

namespace Web.Areas.Admin.Controllers
{
    public class CrudController : BaseController
    {

        protected dynamic DefaultModel(Type type)
        {

            dynamic model = new ExpandoObject();
            model.entityName = type.Name;
            model.entities = new ArrayList();
            model.IgnoreFields = IgnoreFields;
            model.EditableFiels = EditableFields(type);
            return model;

        }

        protected List<String> IgnoreFields
        {
            get
            {
                var props = typeof(AbsEntity)
                               .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                               .Where(p => p.CanRead && p.CanWrite)
                               .Select(p => p.Name)
                               .ToList<String>();
                return props;
            }

        }
        protected List<dynamic> EditableFields(Type type)
        {
            var Ignores = IgnoreFields;
            var props = type
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(e => e.CanRead && e.CanWrite && !Ignores.Contains(e.Name))
                    .ToList();

            var lst = new List<dynamic>();
            foreach (var p in props)
            {
                dynamic row = new ExpandoObject();
                row.Name = p.Name;
                row.Type = p.PropertyType;
                lst.Add(row);
            }

            return lst;

        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Only works on ViewResults...
            ViewResultBase viewResult = filterContext.Result as ViewResultBase;
            
            base.OnActionExecuting(filterContext);
            if (filterContext.RouteData.Values.ContainsKey(CrudViewEngine.EntityKey) &&
                filterContext.RouteData.Values.ContainsKey(CrudViewEngine.ActionKey))
            {
                string entityName = filterContext.RouteData.Values[CrudViewEngine.EntityKey] as String;
                string action = filterContext.RouteData.Values[CrudViewEngine.ActionKey] as String;
                string controller = entityName + "Controller";
                Type[] types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
                Type type = types.Where(t => t.Name == controller).SingleOrDefault();
                if (!ObjectUtils.IsEmpty(type))
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        controller = entityName,
                        action = action
                    }));
                }
            }

        }


        public ActionResult Index(String entity)
        {
            dynamic model = new ExpandoObject();
            model.entityName = entity;
            model.entities = null;
            Type entityType = EntityHelper.FindEntityType(entity);
            if (!ObjectUtils.IsEmpty(entityType))
            {
                IList entities = EntityHelper.FindAll(entityType);
                model.entities = entities;
            }
            if (ObjectUtils.IsEmpty(model.entities))
            {
                model.entities = new ArrayList();
            }

            return View(model);
        }

        //
        // GET: /Admin/Default1/Details/5

        public ActionResult Details(String entity, int id)
        {
            Type entityType = EntityHelper.FindEntityType(entity);
            var model = DefaultModel(entityType);
            return View(model);
        }

        //
        // GET: /Admin/Default1/Create

        public ActionResult Create(String entity)
        {
            Type entityType = EntityHelper.FindEntityType(entity);
            var model = DefaultModel(entityType);
            model.entity = Activator.CreateInstance(entityType);
            return View(model);
        }

        //
        // POST: /Admin/Default1/Create

        [HttpPost]
        public ActionResult Create(String entity, FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                Type entityType = EntityHelper.FindEntityType(entity);
                var model = DefaultModel(entityType);
                return View(model);
            }
        }

        //
        // GET: /Admin/Default1/Edit/5

        public ActionResult Edit(String entity, String id)
        {
            return View();
        }

        //
        // POST: /Admin/Default1/Edit/5

        [HttpPost]
        public ActionResult Edit(String entity, String id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Admin/Default1/Delete/5

        public ActionResult Delete(String entity, String id)
        {
            return View();
        }

        //
        // POST: /Admin/Default1/Delete/5

        [HttpPost]
        public ActionResult Delete(String entity, String id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
