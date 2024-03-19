using Globamantics.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globamantics.Infrastructure.Data.Repositories
{
    public class ToDoTaskRepository : ToDoRepository<TodoTask>
    {
        public ToDoTaskRepository(GlobomanticsDbContext context) : base(context)
        {
        }

        public override async Task AddAsync(TodoTask todoTask)
        {
            var toDoTaskToAdd = DomainToDataMapping.MapToDoFromDomain<Domain.TodoTask, Models.ToDoTask>(todoTask);
            await Context.AddAsync(toDoTaskToAdd);
        }

        public override async Task<TodoTask> GetAsync(Guid id)
        {
            var existing = await Context.ToDoTasks.SingleAsync(todo => todo.Id == id);

            return DataToDomainMapping.MapTodoFromData<Models.ToDoTask, Domain.TodoTask>(existing);
        }
    }
}
