using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Globamantics.Domain;
using Globamantics.Infrastructure.Data.Repositories;
using Globomantics.Windows.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Globomantics.Windows.ViewModels
{
    public class BugViewModel : BaseToDoViewModel<Bug>
    {
        private readonly IRepository<Bug> repository;

        #region all the data is be available on the bug
        private string? description;
        private string? affectedVersion;
        private int affectedUsers;
        private DateTimeOffset dueDate;
        private Severity severity;
        #endregion

        public Severity Severity
        {
            get => severity;
            set
            {
                severity = value;
                OnPropertyChanged(nameof(Severity));
            }
        }
        public DateTimeOffset DueDate
        {
            get => dueDate;
            set
            {
                dueDate = value;
                OnPropertyChanged(nameof(DueDate));
            }
        }
        public int AffectedUsers
        {
            get => affectedUsers;
            set
            {
                affectedUsers = value;
                OnPropertyChanged(nameof(AffectedUsers));
            }
        }
        public string Description
        {
            get => description;
            set
            {
                description = value;
                OnPropertyChanged(nameof(Description));
            }
        }
        public string AffectedVersion
        {
            get => affectedVersion;
            set
            {
                affectedVersion = value;
                OnPropertyChanged(nameof(AffectedVersion));
            }
        }

        public IEnumerable<Severity> SeverityLevels { get; } = new[]
        {
            Severity.Critical,
            Severity.Annoying,
            Severity.Major,
            Severity.Minor,
        };
        //using ObservableCollection: whenever you add items to this specific collection, 
        // it will tell anyone that's consuming the collection that a new item has been added,
        // which is useful when building UI app.
        public ObservableCollection<byte[]> Screenshots { get; set; } = new();
        public ICommand AttachScreenshotCommand { get; set; }

        public BugViewModel(IRepository<Bug> repository) : base()
        {
            this.repository = repository;

            SaveCommand = new RelayCommand(async () =>
            {
                await SaveAsync();
            });

            AttachScreenshotCommand = new RelayCommand(async () =>
            {
                var filenames = ShowOpenFileDialog?.Invoke();

                if (filenames is null || !filenames.Any())
                {
                    return;
                }

                foreach (var item in filenames)
                {
                    Screenshots.Add(File.ReadAllBytes(item));
                }
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
                Model = new Bug(Title, Description ?? "No Description", Severity, AffectedVersion, AffectedUsers, App.CurrentUser, App.CurrentUser, Screenshots.ToArray())
                {
                    DueDate = DueDate,
                    Parent = Parent,
                    IsCompleted = IsCompleted,
                };
            }
            else
            {
                Model = Model with
                {
                    Title = Title,
                    Description = Description ?? "No Description",
                    Severity = Severity,
                    AffectedVersion = AffectedVersion,
                    AffectedUsers = AffectedUsers,
                    DueDate = DueDate,
                    Parent = Parent,
                    IsCompleted = IsCompleted,
                    Images = Screenshots.ToArray()
                };
            }
            await repository.AddAsync(Model);
            await repository.SaveChangeAsync();

            //Send message that the item is saved
            WeakReferenceMessenger.Default.Send<ToDoSavedMessage>(new(Model));

        }

        //reason to override : ensure that the description, AffectedVersion,AfftectedUsers, Severity, Screenshots, DueDate  on our ViewModel is updated when we call this method;
        public override void UpdateModel(ToDo model)
        {
            if (model is not Bug bug) return;
            base.UpdateModel(bug);

            Description = bug.Description;
            AffectedVersion = bug.AffectedVersion;
            AffectedUsers = bug.AffectedUsers;
            Severity = bug.Severity;
            Screenshots = new(bug.Images);
            DueDate = bug.DueDate;
        }
    }
}
