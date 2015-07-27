using Core;
using Core.Dao;
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

namespace Web.Areas.Admin.Controllers
{
    public abstract class GenericCrudController<T> : BaseController where T : AbsEntity, new()
    {

        private IRepository<T> mRepository;
        public  virtual IRepository<T> Repository
        {
            protected get
            {
                return (IRepository<T>) new GenericDao<T>();
                //return mRepository;
            }
            set
            {
                mRepository = value;
            }
        }

        protected dynamic DefaultModel
        {
            get{
                dynamic model = new ExpandoObject();
                model.entityName = typeof(T).Name;
                model.entities = new ArrayList();
                model.IgnoreFields = IgnoreFields;
                model.EditableFiels = EditableFields;
                return model;
            }
        }
        public ActionResult Index()
        {

            var model = DefaultModel;           
            SearchParameters sp = SearchParameters.getBuilder()
                                                   .Build();
            SearchResult<T> result = Repository.Search(sp);
            if (!result.IsEmpty)
            {
                model.entities = result.Items;
            }            

            
            return View(model);      
        }

        
        public ActionResult Details(int id)
        {
            var model = DefaultModel;
            return View(model);
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
        protected List<dynamic> EditableFields
        {
            get
            {
                var props = typeof(T)
                        .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                        .Where(e => e.CanRead && e.CanWrite && !IgnoreFields.Contains(e.Name))                      
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
        }
        public ActionResult Create()
        {
            var model = DefaultModel;
            model.entity = new T();
            return View(model);
        }
                

        [HttpPost]
        public ActionResult Create(T entity)
        {
            try
            {
                Repository.Create(entity);
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                var model = DefaultModel;
                return View(model);
            }
        }

        //
        // GET: /Admin/Default1/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Admin/Default1/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
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

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Admin/Default1/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
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
