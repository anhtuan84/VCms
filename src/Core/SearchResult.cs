using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class SearchResult<T> where T:AbsEntity
    {
        public Int64 TotalItem { get; private set; }
        public List<T> Items { get; private set; }

       
        public Boolean IsEmpty { 
            get{
                return ObjectUtils.IsEmpty(Items) || Items.Count ==0;
            }
        }

        public SearchResult(List<T> items ){
            if(ObjectUtils.IsEmpty(items))
            {
                Items = new List<T>();
            }else{
                Items = items;
            }

            TotalItem = items.Count;

        }

        public SearchResult(Int64 totalItem, List<T> items)
        {
            TotalItem = totalItem;
            Items = ObjectUtils.IsEmpty(items) ? new List<T>() : items;
        }


    }
}
