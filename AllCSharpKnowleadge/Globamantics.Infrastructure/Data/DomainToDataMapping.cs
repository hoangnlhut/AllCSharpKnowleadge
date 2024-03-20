using Globamantics.Domain;
using Globamantics.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globamantics.Infrastructure.Data
{
    public class DomainToDataMapping
    {
        public static TTo MapToDoFromDomain<TFrom, TTo>(TFrom dataDomain)
            where TFrom : Domain.ToDo
            where TTo : Data.Models.ToDo
        {
            var model = dataDomain switch
            {
                Domain.Bug bug => MapBug(bug),
                Domain.Feature feature => MapFeature(feature),
                Domain.TodoTask toDoTask => MapToDoTask(toDoTask),
                _ => throw new NotImplementedException()
            } as TTo;

            return model;
        }

        private static Models.ToDoTask MapToDoTask(TodoTask toDoTask)
        {
            return new()
            {
                DueDate = toDoTask.DueDate,
                Id = toDoTask.Id,
                Title = toDoTask.Title,
                CreatedDate = toDoTask.CreatedDate,
                IsCompleted = toDoTask.IsCompleted,
                IsDeleted = toDoTask.IsDeleted,
                CreatedById = toDoTask.CreatedBy?.Id,
            };
        }

        public static Models.User MapUser(Domain.User createBy)
        {
            return new() { Id = createBy.Id, Name = createBy.Name };
        }

        private static Models.Feature MapFeature(Domain.Feature feature)
        {
            return new()
            {
                Description = feature.Description,
                Component = feature.Component,
                Priority = feature.Priority,
                DueDate = feature.DueDate,
                Id = feature.Id,
                Title = feature.Title,
                CreatedDate = feature.CreatedDate,
                IsCompleted = feature.IsCompleted,
                IsDeleted = feature.IsDeleted,
                AssignedToId = feature.AssignedTo?.Id,
                CreatedById = feature.CreatedBy?.Id,
            };
        }

        private static Models.Bug MapBug(Domain.Bug bug)
        {
            return new()
            {
                Description = bug.Description,
                Severity = (Models.Severity)bug.Severity,
                AffectedVersion = bug.AffectedVersion,
                AffectedUsers = bug.AffectedUsers,
                Images = bug.Images.Select(image => new Data.Models.Image {ImageData = Convert.ToBase64String(image)}).ToArray(),
                DueDate = bug.DueDate,
                Id = bug.Id,
                Title = bug.Title,
                CreatedDate = bug.CreatedDate,
                IsCompleted = bug.IsCompleted,
                IsDeleted = bug.IsDeleted,
                AssignedToId = bug.AssignedTo?.Id,
                CreatedById = bug.CreatedBy?.Id,
            };
        }
    }
}
