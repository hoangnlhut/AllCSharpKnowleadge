using CommunityToolkit.Mvvm.ComponentModel;
using Globamantics.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Globomantics.Windows.Messages;

namespace Globomantics.Windows.ViewModels
{
    public abstract class BaseToDoViewModel<T> : ObservableObject, IToDoViewModel where T : ToDo
    {
        // this reflects all the data exposed to the UI such as the Title, IsCompleted and other value from the ToDo
        private T? model;
        private string? title;
        private bool isCompleted;
        private ToDo? parent;

        public T? Model { 
            get => model; 
            set
            {
                model = value;
                OnPropertyChanged(nameof(Model));
                OnPropertyChanged(nameof(IsExisting));
            }
        }

        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public bool IsCompleted
        {
            get => isCompleted;
            set
            {
                isCompleted = value;
                OnPropertyChanged(nameof(IsCompleted));
            }
        }

        public ToDo? Parent
        {
            get => parent;
            set
            {
                parent = value;
                OnPropertyChanged(nameof(Parent));
            }
        }

        // used to show or hide Delete button in UI
        public bool IsExisting => Model is not null;
         
        #region From ITodoViewModel and IViewModel
        public IEnumerable<ToDo>? AvailableParentTasks {get; set;}

        public ICommand DeleteCommand { get; }

        public ICommand SaveCommand { get; set; } = default!;

        public Action<string>? ShowAlert { get; set; }
        public Action<string>? ShowError { get; set; }
        public Func<IEnumerable<string>>? ShowOpenFileDialog { get; set; }
        public Func<string>? ShowSaveFileDialog { get; set; }
        public Func<string, bool>? AskForConfirmation { get; set; }
        #endregion

        public BaseToDoViewModel()
        {
            DeleteCommand = new RelayCommand(() =>
            {
                if (Model is not null) {
                    // change value in record
                    Model = Model with { IsDeleted = true };

                    //Send message that model is deleted
                    WeakReferenceMessenger.Default.Send<ToDoDeletedMessage>(new(Model));
                }
            });
        }

        public abstract Task SaveAsync();

        public virtual  void UpdateModel(ToDo model)
        {
            if (model is null)
            {
                return;
            }

            var parent = AvailableParentTasks?.SingleOrDefault(
                 t => model.Parent is not null && t.Id == model.Parent.Id
                 );

            Model = model as T;
            Title = model.Title;
            IsCompleted = model.IsCompleted;
            Parent = parent;
        }
    }
}
