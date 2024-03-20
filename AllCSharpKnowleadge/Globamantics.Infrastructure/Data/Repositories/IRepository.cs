using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globamantics.Infrastructure.Data.Repositories
{
    public interface IRepository<T>
    {
        Task<T> GetAsync(Guid id);
        Task<T> FindByAsync(string value);
        Task<IEnumerable<T>> AllAsync(string title = "");
        Task AddAsync(T item);
        Task SaveChangeAsync();
    }
}