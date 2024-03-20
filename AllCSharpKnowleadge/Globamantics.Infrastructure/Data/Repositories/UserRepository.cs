using Globamantics.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Globamantics.Infrastructure.Data.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly GlobomanticsDbContext Context;

        public UserRepository(GlobomanticsDbContext context)
        {
            Context = context;
        }

        public async Task AddAsync(User item)
        {
            var existingUSer = await Context.Users.SingleOrDefaultAsync(u => u.Id == item.Id);
            if (existingUSer is null) 
            {
                var userToAdd = DomainToDataMapping.MapUser(item);
                await Context.Users.AddAsync(userToAdd);
            }
            else
            {
                existingUSer.Name = item.Name;
                Context.Users.Update(existingUSer);
            }
        }

        public async Task<IEnumerable<User>> AllAsync(string title = "")
        {
            return await Context.Users.Select(x => DataToDomainMapping.MapUser(x)).ToArrayAsync();
        }

        public async Task<User> FindByAsync(string name)
        {
            var existingUser = await Context.Users.SingleAsync(x => x.Name == name);

            return DataToDomainMapping.MapUser(existingUser);
        }

        public async Task<User> GetAsync(Guid id)
        {
            var existingUser = await Context.Users.SingleAsync(x => x.Id == id);

            return DataToDomainMapping.MapUser(existingUser);
        }

        public async Task SaveChangeAsync()
        {
            await Context.SaveChangesAsync();
        }
    }
}
