using Globamantics.Domain;
using Globomantics.Windows.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globomantics.Windows.Factory
{
    public class ToDoViewModelFactory
    {
        public static IEnumerable<string> TodoTypes = new[]
        {
            nameof(Bug),
            nameof(Feature)
        };
        private readonly IServiceProvider serviceProvider;

        public ToDoViewModelFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IToDoViewModel CreateViewModel(string type, IEnumerable<ToDo> tasks, ToDo? model = default)
        {
            IToDoViewModel viewModel = type switch
            {
                // this is locator pattern;
                nameof(Bug) => serviceProvider.GetService<BugViewModel>(),
                nameof(Feature) => serviceProvider.GetService<FeatureViewModel>(),
                _ => throw new NotImplementedException()
            };

            ArgumentNullException.ThrowIfNull(viewModel);

            if (tasks is not null &&  tasks.Any())
            {
                viewModel.AvailableParentTasks = tasks;
            }

            if(model is not null)
            {
                viewModel.UpdateModel(model);
            }

            return viewModel;
        }
    }
}
 