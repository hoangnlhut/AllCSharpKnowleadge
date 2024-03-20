 using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Globamantics.Domain;
using Globamantics.Infrastructure.Data.Repositories;
using Globomantics.Windows.Json;
using Globomantics.Windows.Messages;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Globomantics.Windows.ViewModels;

public class MainViewModel : ObservableObject,
    IViewModel
{
    private string statusText = "Everything is OK!";
    private string searchText = "";
    private bool isLoading;
    private bool isInitialized;
    private readonly IRepository<User> userRepository;
    private readonly IRepository<TodoTask> toDoRepository;

    public string StatusText
    {
        get => statusText;
        set
        {
            statusText = value;

            OnPropertyChanged(nameof(StatusText));
        }
    }

    public string SearchText
    {
        get => searchText;
        set
        {
            searchText = value;
            OnPropertyChanged(nameof(SearchText));
        }
    }
    public bool IsLoading
    {
        get => isLoading;
        set
        {
            isLoading = value;

            OnPropertyChanged(nameof(IsLoading));
        }
    }

    public ICommand ExportCommand { get; set; }
    public ICommand SearchCommand { get; set; }
    public ICommand ImportCommand { get; set; }

    public Action<string>? ShowAlert { get; set; }
    public Action<string>? ShowError { get; set; }
    public Func<IEnumerable<string>>? ShowOpenFileDialog { get; set; }
    public Func<string>? ShowSaveFileDialog { get; set; }
    public Func<string, bool>? AskForConfirmation { get; set; }

    public ObservableCollection<ToDo> Completed { get; set; } = new();
    public ObservableCollection<ToDo> Unfinished { get; set; } = new();

    public MainViewModel(IRepository<User> userRe, IRepository<TodoTask> toDoTaskRe)
    {
        WeakReferenceMessenger.Default.Register<ToDoSavedMessage>(this, (sender, messsage) =>
        {
            var item = messsage.Value;
            if (item.IsCompleted)
            {
                var existing = Unfinished.FirstOrDefault(i => i.Id == item.Id);
                if (existing != null)
                {
                    Unfinished.Remove(existing);
                }
                ReplaceOrAdd(Completed, item);
            }
            else
            {
                var existing = Completed.FirstOrDefault(i => i.Id == item.Id);
                if (existing != null)
                {
                    Completed.Remove(existing);
                }
                ReplaceOrAdd(Unfinished, item);
            }

        });
        WeakReferenceMessenger.Default.Register<ToDoDeletedMessage>(this, (sender, message) =>
        {
            var item = message.Value;

            var unfinishedItem = Unfinished.FirstOrDefault(i => i.Id == item.Id);
            if (unfinishedItem is not null)
            {
                Unfinished.Remove(unfinishedItem);
            }

        });
        userRepository = userRe;
        toDoRepository = toDoTaskRe;

        ExportCommand = new RelayCommand(async () =>
        {
            await ExportAsync();
        });

        ImportCommand = new RelayCommand(async () =>
        {
            await ImportAsync();
        });

        SearchCommand = new RelayCommand(async () =>
        {
            Unfinished.Clear();
            IEnumerable<TodoTask> items = await toDoRepository.AllAsync(SearchText);

            foreach (var item in items.Where(x => !x.IsCompleted && !x.IsDeleted))
            {
                Unfinished.Add(item);
            }
        });
    }
   
    private async Task ImportAsync()
    {
        var fileNames = ShowOpenFileDialog?.Invoke();
        if (fileNames is null || !fileNames.Any()) return;

        var fileName = fileNames.First();
        if (string.IsNullOrEmpty(fileName))
        {
            ShowError?.Invoke("No filename specified");
        }

        IsLoading = true;

        var json = await File.ReadAllTextAsync(fileName);

        var items = JsonConvert.DeserializeObject<IEnumerable<TodoTask>>(json, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            SerializationBinder = new SerializationBinder()
        });

        if (items is null) return;

        //save to Db
        foreach (var item in items)
        {
           await toDoRepository.AddAsync(item);
           if (item.IsCompleted)
           {
                Completed.Add(item);
           }
           else if(!item.IsDeleted)
           {
                Unfinished.Add(item);
           }
        }
        await toDoRepository.SaveChangeAsync();

        IsLoading = false;
        ShowAlert?.Invoke("Data Imported");
    }

    private  async Task ExportAsync()
    {
        var fileName = ShowSaveFileDialog?.Invoke();

        IsLoading = true;

        var items = await toDoRepository.AllAsync();

        //.net 6 no support to deserialize that collection using polymorphism
        // and we have to another approach to deal with it

        var json = JsonConvert.SerializeObject(items, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            SerializationBinder = new SerializationBinder()
        }) ;

        await File.WriteAllTextAsync(fileName, json);

        IsLoading = false;
        ShowAlert?.Invoke("Data Exported");
    }

    private static void ReplaceOrAdd(ObservableCollection<ToDo> collection, ToDo item)
    {
        var existingItem = collection.FirstOrDefault(i => i.Id == item.Id);
        if (existingItem != null)
        {
            var index = collection.IndexOf(existingItem);
            collection[index] = item;
        }
        else
        {
            collection.Add(item);
        }
    }

    public async Task InitializeAsync()
    {
        if (isInitialized) return;

        App.CurrentUser = await userRepository.FindByAsync("Hoang");

        var items = await toDoRepository.AllAsync();
        var itemsDue = items.Count(i => i.DueDate.ToLocalTime() > DateTimeOffset.Now);

        StatusText = $"Welcome {App.CurrentUser}! You have {itemsDue} items passed due date.";

        foreach (var item in items.Where(item => !item.IsDeleted))
        {
            if (item.IsCompleted)
            {
                Completed.Add(item);
            }
            else
            {
                Unfinished.Add(item);
            }
        }

        isInitialized = true;
    }
}