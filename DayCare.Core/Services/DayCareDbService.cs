using DayCare.Core;
using DayCare.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Core.Services
{
    public class DayCareDbService : IDayCareDbRepository
    {
        public async Task<IEnumerable<T>> RetrieveAsync<T>(Dictionary<string, object> parameters, string storedProcName) where T : class, new()
        {
            return await DbContext.DayCareDB.RetrieveAsync<T>(parameters, storedProcName);
        }
        public async Task CreateAsync<T>(T entity, string storedProcName) where T : class, new()
        {
            await DbContext.DayCareDB.CreateAsync<T>(entity, storedProcName);
        }
        public async Task UpdateAsync<T>(T entity, string storedProcName) where T : class, new()
        {
            await DbContext.DayCareDB.UpdateAsync<T>(entity, storedProcName);
        }
        public async Task DeleteAsync<T>(T entity, string storedProcName) where T : class, new()
        {
            await DbContext.DayCareDB.DeleteAsync<T>(entity, storedProcName);
        }
    }
}
