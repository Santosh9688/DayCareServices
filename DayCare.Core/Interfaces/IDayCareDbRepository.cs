using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Core.Interfaces
{
    public interface IDayCareDbRepository
    {
        Task<IEnumerable<T>> RetrieveAsync<T>(Dictionary<string, object> parameters, string storedProcName) where T : class, new();
        Task CreateAsync<T>(T entity, string storedProcName) where T : class, new();
        Task UpdateAsync<T>(T entity, string storedProcName) where T : class, new();
        Task DeleteAsync<T>(T entity, string storedProcName) where T : class, new();
    }
}
