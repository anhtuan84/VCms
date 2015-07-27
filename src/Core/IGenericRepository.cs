using Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public interface IRepository<T> where T : AbsEntity
    {
        T Get(String Id);
        T Update(String Id, T Entity);
        T Create(T Entity);
        T CreateOrUpdate(T Entity);
        void delete(String Id);
        IList<T> FindAll(Int64 startIndex, Int64 maxResult);
        SearchResult<T> Search(SearchParameters sp);
        DbSet<T> DbSet { get; }
    }
}
