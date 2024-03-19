using Globamantics.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globamantics.Infrastructure.Data.Repositories
{
    public class FeatureRepository : ToDoRepository<Feature>
    {
        public FeatureRepository(GlobomanticsDbContext context) : base(context)
        {
        }

        public override async Task AddAsync(Feature feature)
        {
            var existingFeature = await Context.Features.FirstOrDefaultAsync(b => b.Id == feature.Id);

            var user = await Context.Users.SingleOrDefaultAsync(u => u.Id == feature.CreatedBy.Id);
            user ??= new() { Id = feature.CreatedBy.Id, Name = feature.CreatedBy.Name };

            if (existingFeature != null)
            {
                await UpdateAsync(feature, existingFeature, user);
            }
            else
            {
                await CreateAsync(feature, user);
            }
        }

        private async Task UpdateAsync(Domain.Feature feature, Models.Feature existingFeature, Models.User user)
        {
            existingFeature.IsCompleted = feature.IsCompleted;
            existingFeature.Component = feature.Component;
            existingFeature.Priority = feature.Priority;
            existingFeature.Description = feature.Description;
            existingFeature.Title = feature.Title;
            existingFeature.AssignedTo = user;
            existingFeature.CreatedBy = user;

            await SetParentAsync(existingFeature, feature);

            Context.Features.Update(existingFeature);
        }

        private async Task CreateAsync(Feature feature, Models.User user)
        {
            var featureToAdd = DomainToDataMapping.MapToDoFromDomain<Domain.Feature, Models.Feature>(feature);

            //check if the parent already exist or if this also has to be created 
            await SetParentAsync(featureToAdd, feature);

            featureToAdd.CreatedBy = user;
            featureToAdd.AssignedTo = user;

            await Context.AddAsync(featureToAdd);
        }

        public override async Task<Feature> GetAsync(Guid id)
        {
            var data = await Context.Features.SingleAsync(feature => feature.Id == id);

            return DataToDomainMapping.MapTodoFromData<Models.Feature, Domain.Feature>(data);
        }
    }
}
