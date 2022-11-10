using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Services.Interfaces
{
    public interface ICRUDOperations<T> where T : class
    {
        IQueryable<T> Get();
        Task<T> PostAsync(T obj);
        Task<T> UpdateAsync(int id, T obj);
        Task<T> DeleteAsync(int id);

    }
}
