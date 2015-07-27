using Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq.Expressions;

namespace Core.Dao
{
    public class GenericDao<T>:IRepository<T> where T:AbsEntity
    {
        private IDbContext db;
        public IDbContext DbCtx
        {
            protected get
            {
                if (db == null)
                {
                    db = new Core.DbContext("DefaultConnection");                    
                }

                return db;
            }
            set
            {
                db = value;
            }

        }
        
        public T Get(String Id)
        {
             try
             {
                return this.DbCtx.Set<T>().Find(Id);
             }
             catch (DbEntityValidationException e)
             {
                 throw new DataException(e.Message, e.InnerException);
             }
             return null;
        }

        protected virtual void VerifyOnUpdate(T existedEntity, T newEntity)
        {

        }
        public T Update(String Id, T Entity)
        {
            try
            {          
                if(String.IsNullOrEmpty(Id) || !Id.Equals(Entity.Id)){
                    throw new InvalidDataException("Invalid Id");
                }

                Entity.ModifiedOn = DateTime.UtcNow;
                Entity.Deleted = 0;
                T existing = this.Get(Id);
               
                //TODO: do validation
                VerifyOnUpdate(existing, Entity);
                               
                this.DbCtx.Entry<T>(existing).CurrentValues.SetValues(Entity);   
               
                this.DbCtx.SaveChanges();
                return this.DbCtx.Set<T>().Find(Id);
            }
            catch (DbEntityValidationException e)
            {
                throw new DataException(e.Message, e.InnerException);
            }
            return null;
        }

        public T Create(T Entity)
        {
            try
            {
                Entity.Id = Guid.NewGuid().ToString("N");
                Entity.ModifiedOn = Entity.CreatedOn = DateTime.UtcNow;
                Entity.Deleted = 0;
                T createdEntity = this.DbCtx.Set<T>().Add(Entity);
                DbEntityEntry entities =this.DbCtx.Entry<T>(Entity);
                entities.State = EntityState.Added;
                this.DbCtx.SaveChanges();
                return createdEntity;
            }
            catch (DbEntityValidationException e)
            {
                throw new DataException(e.Message, e.InnerException);
            }
        }

       
        public T CreateOrUpdate(T Entity)
        {
            T newEntity = null;
            var id = Entity.Id;
            if (this.DbCtx.Set<T>().Any(e => e.Id == id))
            {
                newEntity =Update(Entity.Id, Entity);
            }
            else
            {
                newEntity = Create(Entity);
            }

            return newEntity;
        }

        public void delete(String Id)
        {
            try
            {
                var epoch = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
                T Entity = Get(Id);               
                Entity.Deleted = Convert.ToInt64(epoch);
                this.DbCtx.Entry<T>(Entity).CurrentValues.SetValues(Entity);
                this.DbCtx.SaveChanges();               
            }
            catch (DbEntityValidationException e)
            {
                throw new DataException(e.Message, e.InnerException);
            }
        }

        public IList<T> FindAll(Int64 startIndex, Int64 maxResult)
        {
            var entities = from entity in this.DbCtx.Set<T>()
                                    .OrderByDescending(e => e.ModifiedOn)
                           select entity;


            return entities.ToList();
        }

        public DbSet<T> DbSet
        {
            get{
                return this.DbCtx.Set<T>();
            }           
        }

        public SearchResult<T> Search(SearchParameters sp)
        {            
            var query = DbSet.AsQueryable();
           
            if (ObjectUtils.IsEmpty(sp.Query))
            {
                var total = DbSet.Count();
                var result = EntityHelper.OrderByName<T>(query,
                                                    sp.SortField,
                                                    sp.SortOrder == SortOrder.DESC)
                            .OrderByDescending(e => e.CreatedOn)
                            .ToList();
                return new SearchResult<T>(total, result);
            }
            else if (sp.Query is Expression<Func<T, bool>>)
            {
                Expression<Func<T, bool>> iWhere = sp.Query as Expression<Func<T, bool>>;
                var total = DbSet.Count(iWhere);
                var tQuery =     query.Where(iWhere)
                                .Skip(sp.StartIndex)
                                .Take(sp.MaxResult);

                var result = EntityHelper.OrderByName<T>(tQuery,
                                                        sp.SortField,
                                                        sp.SortOrder == SortOrder.DESC)
                                .OrderByDescending(e=>e.CreatedOn)
                                .ToList();
                return new SearchResult<T>(total,result);
            }
            else if (sp.Query is String)
            {
                
            }
            else if (sp.Query is ObjectQuery<T>)
            {

            }
            return new SearchResult<T>(null);
        }

        private IQueryable<T> Search(IQueryable<T> source, SearchParameters sp)
        {
            Expression<Func<T, bool>> iWhere = sp.Query as Expression<Func<T, bool>>;
            
            var tQuery = source.Where(iWhere)
                            .Skip(sp.StartIndex)
                            .Take(sp.MaxResult);

            var result = EntityHelper.OrderByName<T>(tQuery,
                                                    sp.SortField,
                                                    sp.SortOrder == SortOrder.DESC)
                            .OrderByDescending(e => e.CreatedOn);                           
             return result;
        }


       
    }
}
