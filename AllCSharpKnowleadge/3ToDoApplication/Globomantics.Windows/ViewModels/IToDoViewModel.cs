using Globamantics.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Globomantics.Windows.ViewModels
{
    public interface IToDoViewModel : IViewModel
    {
        IEnumerable<ToDo>? AvailableParentTasks { get; set; }

        ICommand DeleteCommand { get; }
        ICommand SaveCommand { get; }
        Task SaveAsync();
        void UpdateModel(ToDo model);
    }
}
