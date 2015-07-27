using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public static class EntityExtensions
    {
        public static T Get<T>(this String id) where T: AbsEntity
        {
            return default(T);
        }
    }
}
