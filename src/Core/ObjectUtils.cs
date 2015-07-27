using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class ObjectUtils
    {
        public static bool IsEmpty(object obj){
            if (obj == null)
            {
                return true;
            }else if(obj is String){
                return String.IsNullOrEmpty(obj as String);

            }
            else if (obj is ICollection)
            {
                return ((ICollection)obj).Count == 0;
            }

            return false;
        }
    }
}
