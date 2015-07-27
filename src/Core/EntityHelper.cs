using Core.Dao;
using Models;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public static class EntityHelper
    {
        private static ConcurrentDictionary<string, Type> entityTypes;
        public static bool IsEntityExists(String entityName)
        {
            return FindEntityType(entityName) != null;
        }

        public static ConcurrentDictionary<string, Type> ScanEntities()
        {
            if (ObjectUtils.IsEmpty(entityTypes))
            {
                entityTypes = new ConcurrentDictionary<string, Type>();
                var types = new List<Type>();
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    var tmpTypes = assembly
                      .GetTypes()
                      .Where(t =>
                        t.IsSubclassOf(typeof(AbsEntity)));
                    if (!ObjectUtils.IsEmpty(tmpTypes))
                    {
                        foreach (var type in tmpTypes)
                        {
                            entityTypes[type.Name] = type;
                        }
                    }

                }

            }
            return entityTypes;
        }
        public static Type FindEntityType(String entityName)
        {
            ScanEntities();
            if (entityTypes.ContainsKey(entityName))
            {
                return entityTypes[entityName];
            }

            return null;
        }

        public static IList FindAll(Type type)
        {
            Type[] argTypes = new Type[] { typeof(Int64), typeof(Int64) };

            Object genericDao = GetGenericDao(type);
            MethodInfo method = genericDao.GetType()
                               .GetMethod("FindAll", argTypes);
            Object[] args = { 0, 1 };
            IList result = (IList)method.Invoke(genericDao, args);
            return result;
        }

        public static object DoResolve(string name)
        {
            // Type type = typeof(UnityContainer);
            // MethodInfo genericMethod = type.GetMethod("Resolve").MakeGenericMethod(typeof(ServiceClass<>).MakeGenericType(new Type[] { Type.GetType(name) }));
            //  object invoke = genericMethod.Invoke(MyContainer.Container, null);
            // return (object)invoke;

            return null;
        }

        public static Object GetGenericDao(Type type)
        {
            var Clazz = typeof(GenericDao<>);
            Type[] typeArgs = { type };
            var makeMe = Clazz.MakeGenericType(typeArgs);
            Object genericDao = Activator.CreateInstance(makeMe);
            return genericDao;
        }


        public static IQueryable<T> OrderByName<T>(this IQueryable<T> source,
                                                    string propertyName,
                                                    Boolean isDescending) where T:AbsEntity
        {

            if (source == null) throw new ArgumentNullException("source");
            if (propertyName == null) throw new ArgumentNullException("propertyName");

            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "x");

            PropertyInfo pi = type.GetProperty(propertyName);
            Expression expr = Expression.Property(arg, pi);
            type = pi.PropertyType;

            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            String methodName = isDescending ? "OrderByDescending" : "OrderBy";
            object result = typeof(Queryable).GetMethods().Single(
                method => method.Name == methodName
                        && method.IsGenericMethodDefinition
                        && method.GetGenericArguments().Length == 2
                        && method.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), type)
                .Invoke(null, new object[] { source, lambda });
            return (IQueryable<T>)result;
        }

        public static T Get<T>(String id) where T:AbsEntity{
           
            return default(T);
        }
    }
}
