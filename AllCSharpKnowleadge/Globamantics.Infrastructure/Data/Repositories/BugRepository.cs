using Globamantics.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globamantics.Infrastructure.Data.Repositories
{
    public class BugRepository : ToDoRepository<Bug>
    {
        public BugRepository(GlobomanticsDbContext context) : base(context)
        {
        }

        public override async Task AddAsync(Bug bug)
        {
            var existingBug = await Context.Bugs.FirstOrDefaultAsync(x => x.Id == bug.Id);

            var user = await Context.Users.FirstOrDefaultAsync(x => x.Id == bug.CreatedBy.Id);
            user ??= new() { Id = bug.CreatedBy.Id, Name = bug.CreatedBy.Name};

            if (existingBug is not null)
            {
                await UpdateAsync(bug, existingBug, user);
            }
            else
            {
                await CreateAsync(bug, user);
            }
        }

        private async Task CreateAsync(Bug bug, Models.User user)
        {
            var bugToAdd = DomainToDataMapping.MapToDoFromDomain<Domain.Bug, Models.Bug>(bug);

            //check if the parent already exist or if this also has to be created 
            await SetParentAsync(bugToAdd, bug);

            bugToAdd.CreatedBy = user;
            await Context.AddAsync(bugToAdd);
        }

        private async Task UpdateAsync(Bug bug, Models.Bug existingBug, Models.User user)
        {
            existingBug.IsCompleted = bug.IsCompleted;
            existingBug.AffectedVersion = bug.AffectedVersion;
            existingBug.AffectedUsers = bug.AffectedUsers;
            existingBug.Description = bug.Description;
            existingBug.Title = bug.Title;
            existingBug.Severity = (Data.Models.Severity)bug.Severity;

            await SetParentAsync(existingBug, bug);

            Context.Bugs.Update(existingBug);
        }

        public override async Task<Bug> GetAsync(Guid id)
        {
            var data = await Context.Bugs.SingleAsync(bug => bug.Id == id);

            return DataToDomainMapping.MapTodoFromData<Models.Bug, Domain.Bug>(data);
        }

    }
}
