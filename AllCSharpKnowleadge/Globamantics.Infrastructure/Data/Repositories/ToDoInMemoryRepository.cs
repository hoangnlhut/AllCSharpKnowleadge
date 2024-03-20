using Globamantics.Domain;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globamantics.Infrastructure.Data.Repositories
{
    //we can use this for Bug , Feature and ToDoTask
    public class ToDoInMemoryRepository<T> : IRepository<T> where T : ToDo
    {
        private ConcurrentDictionary<Guid, T> Items { get; } = new();

        public Task AddAsync(T item)
        {
            Items.TryAdd(item.Id, item);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<T>> AllAsync(string title = "")
        {
            var items = Items.Values.ToArray();
            return Task.FromResult<IEnumerable<T>>(items);
        }

        public Task<T> FindByAsync(string value)
        {
            var result = Items.Values.First(item => item.Title == value);
            return Task.FromResult(result);
        }

        public Task<T> GetAsync(Guid id)
        {
            return Task.FromResult(Items[id]);
        }

        public Task SaveChangeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
