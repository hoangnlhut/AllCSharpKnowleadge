using Globamantics.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globamantics.Infrastructure.Data.Repositories
{
    public abstract class ToDoRepository<T> : IRepository<T> where T : TodoTask
    {
        protected GlobomanticsDbContext Context { get; }

        public ToDoRepository(GlobomanticsDbContext context) {
            Context = context;
        }

        public abstract Task AddAsync(T item);
        public abstract Task<T> GetAsync(Guid id);

        public virtual async Task<IEnumerable<T>> AllAsync(string title = "")
        {
            var result = await Context.ToDoTasks
                .Where(t => !t.IsDeleted)
                .Include(t => t.CreatedBy)
                .Include(t => t.Parent)
                .Select(x => DataToDomainMapping.MapTodoFromData<Data.Models.ToDoTask, T>(x))
                .ToArrayAsync();

            if (!string.IsNullOrWhiteSpace(title) && title != "*")
            {
                return result.Where(t => !t.IsCompleted &&
                     t.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
            }

            return result;
        }

        public virtual async Task<T> FindByAsync(string title)
        {
            var task = await Context.ToDoTasks.SingleAsync(x => x.Title == title);

            return DataToDomainMapping.MapTodoFromData<Data.Models.ToDoTask, T>(task);
        }

        public async Task SaveChangeAsync()
        {
            try
            {
                await Context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected async Task SetParentAsync(Data.Models.ToDo toBeAdded, ToDo item)
        {
            Data.Models.ToDoTask? existingParent = null;
            if (item.Parent is not null) 
            {
                existingParent = await Context.ToDoTasks.FirstOrDefaultAsync(
                    i => i.Id == item.Parent.Id
                    );
            }

            if (existingParent is not null )
            {
                toBeAdded.Parent = existingParent;
            }
            else if(item.Parent is not null)
            {
                var parentToAdd = DomainToDataMapping.MapToDoFromDomain<ToDo, Data.Models.ToDoTask>(item.Parent);
                toBeAdded.Parent = parentToAdd;
                await Context.AddAsync(parentToAdd);
            }
        }
    }
}
