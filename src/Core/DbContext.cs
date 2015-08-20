using Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    [DbConfigurationType(typeof(DbConfig))]
    public class DbContext : System.Data.Entity.DbContext, IDbContext
    {
        [ThreadStatic]
        protected static DbContext current;

        public static DbContext Current
        {
            get
            {
                if (current == null)
                    current = new DbContext("DefaultConnection");

                return current;
            }
        }

        public DbContext(String nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            Database.SetInitializer<DbContext>(null);
            //System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<DbContext>());
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            
            var entityMethod = typeof(DbModelBuilder).GetMethod("Entity");
            
            
            foreach (var entity in EntityHelper.ScanEntities())
            {
                entityMethod.MakeGenericMethod(entity.Value)
                      .Invoke(modelBuilder, new object[] { });
            }
            
        }

        private List<Type> GetClasses(Type baseType)
        {
            return Assembly.GetCallingAssembly().GetTypes().Where(type => type.IsSubclassOf(baseType)).ToList();
        }
    }

}
