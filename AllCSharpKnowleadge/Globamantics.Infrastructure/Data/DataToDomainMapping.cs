using Globamantics.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globamantics.Infrastructure.Data
{
    public class DataToDomainMapping
    {
        public static TTo MapTodoFromData<TFrom, TTo>(Data.Models.ToDo fromData)
            where TFrom : Data.Models.ToDo
            where TTo : Domain.ToDo
        {
            var model = fromData switch
            {
                Data.Models.Bug bug => MapBug(bug),
                Data.Models.Feature feature => MapFeature(feature),
                Data.Models.ToDoTask toDoTask => MapToDoTask(toDoTask),
                _ => throw new NotImplementedException()
            } as TTo;

            return model;
        }

        private static Domain.TodoTask MapToDoTask(ToDoTask toDoTask)
        {
            return new(toDoTask.Title, toDoTask.DueDate, MapUser(toDoTask.CreatedBy))
            {
                Id = toDoTask.Id
            };
        }

        private static Domain.Feature MapFeature(Feature feature)
        {
            return new(feature.Title,
                feature.Description,
                feature.Component,
                feature.Priority,
                MapUser(feature.CreatedBy),
                MapUser(feature.AssignedTo)
                )
            {
                Id = feature.Id,
                DueDate = feature.DueDate,
                IsCompleted = feature.IsCompleted,
            };
        }

        private static Domain.Bug MapBug(Bug bug)
        {
            return new(bug.Title,
                bug.Description,
                (Domain.Severity)bug.Severity,
                bug.AffectedVersion,
                bug.AffectedUsers,
                MapUser(bug.CreatedBy),
                MapUser(bug.AssignedTo),
                bug?.Images?.Select(image => Convert.FromBase64String(image.ImageData)).ToArray() ?? Enumerable.Empty<byte[]>().ToArray()
                )
            {
                Id = bug.Id,
                DueDate = bug.DueDate
            };
        }

        public static Domain.User MapUser(User user)
        {
            if (user is null) return null;
            
            return new(user.Name) { Id = user.Id};
        }
    }
}
