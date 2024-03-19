using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Globamantics.Domain;
using Globamantics.Infrastructure.Data.Repositories;
using Globomantics.Windows.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globomantics.Windows.ViewModels
{
    public class FeatureViewModel : BaseToDoViewModel<Feature>
    {
        private readonly IRepository<Feature> repository;

        private string? description;
        public string Description
        {
            get => description;
            set
            {
                description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public FeatureViewModel(IRepository<Feature> repository) : base() 
        {
            this.repository = repository;

            SaveCommand = new RelayCommand(async () =>
            {
                await SaveAsync();
            });
        }

        public override async Task SaveAsync()
        {
            // guard 
            if (string.IsNullOrWhiteSpace(Title))
            {
                ShowError.Invoke($"{nameof(Title)} cannot be empty");

                return;
            }

            if (Model is null)
            {
                Model = new Feature(Title, Description, "UI?", 1, App.CurrentUser, App.CurrentUser)
                {
                    DueDate = DateTimeOffset.Now.AddDays(10),
                    Parent = Parent,
                    IsCompleted = IsCompleted,
                };
            }
            else
            {
                Model = Model with
                {
                    Title = Title,
                    Description = Description,
                    Parent = Parent,
                    IsCompleted = IsCompleted,
                };
            }
            await repository.AddAsync(Model);
            await repository.SaveChangeAsync();

            //Send message that the item is saved
            WeakReferenceMessenger.Default.Send<ToDoSavedMessage>(new(Model));
        }

        //reason to override : ensure that the description on our ViewModel is updated when we call this method;
        public override void UpdateModel(ToDo model)
        {
            if (model is not Feature feature) return;
            base.UpdateModel(feature);

            Description = feature.Description;
        }
    }
}
