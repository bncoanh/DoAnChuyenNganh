using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagementCourse.IReposiory
{
   public interface IGenericRepository<T> where T:class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Func<T, bool> predicate);
        T GetByID(int id);
        int Create(T item);
        int CreateRange(IEnumerable<T> items);
        int Update(T item);
        int Delete(int id);
        int Confirm(T[] item);
        int RemoveRange(IEnumerable<T> items);
        int Remove(T item);
        Task<int> CreateAsync(T item);

    }
}
