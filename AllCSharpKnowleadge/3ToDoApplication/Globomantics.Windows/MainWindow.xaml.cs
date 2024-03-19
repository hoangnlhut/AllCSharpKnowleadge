using CommunityToolkit.Mvvm.Messaging;
using Globamantics.Domain;
using Globomantics.Windows.Factory;
using Globomantics.Windows.Messages;
using Globomantics.Windows.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Globomantics.Windows;

public partial class MainWindow : Window
{
    private readonly MainViewModel mainViewModel;
    private readonly ToDoViewModelFactory toDoViewModelFactory;

    public MainWindow(MainViewModel mainViewModel, ToDoViewModelFactory toDoViewModelFactory)
    { 
        InitializeComponent();

        this.mainViewModel = mainViewModel;
        this.toDoViewModelFactory = toDoViewModelFactory;
        DataContext = mainViewModel;

        mainViewModel.ShowSaveFileDialog = () => OpenCreateFileDialog();
        mainViewModel.ShowOpenFileDialog = () => OpenFileDialog(".json", "JSON (.json)|*.json", true);
        mainViewModel.ShowError = (message) => {
            MessageBox.Show(message);
        };
        mainViewModel.ShowAlert = (message) => {
            MessageBox.Show(message);
        };

        TodoType.ItemsSource = ToDoViewModelFactory.TodoTypes;

        //listen message of ToDoSaved and TodoDeleted
        WeakReferenceMessenger.Default.Register<ToDoSavedMessage>(this, (sender, message) =>
        {
            CreateTodoControlContainer.Children.Clear();
        });
        
        WeakReferenceMessenger.Default.Register<ToDoDeletedMessage>(this, (sender, message) =>
        {
            CreateTodoControlContainer.Children.Clear();
         });
    }

    protected override async void OnActivated(EventArgs e)
    {
        base.OnActivated(e);

        try
        {
            await mainViewModel.InitializeAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    private UserControl CreateUserControl(string type, 
        // TODO: Change object to domain object type
        ToDo? model = default)
    {
        // idea: should create an instance of the appropriate ViewModel and pass it into the appropriate UserControl
        // we using factory pattern to deal with this matter
        IToDoViewModel viewModel = toDoViewModelFactory.CreateViewModel(
            type, 
            mainViewModel.Unfinished.ToArray(),
            model
            );   

        viewModel.ShowError = (message) => { MessageBox.Show(message); };
        viewModel.ShowAlert = (message) => { MessageBox.Show(message); };
        viewModel.ShowOpenFileDialog = () => OpenFileDialog(".jpg", "Images (.jpg)|*.jpg", true);

        return ToDoUserControlFactory.CreateUserControl(viewModel);
    }

    private void Search_OnClick(object sender, RoutedEventArgs e)
    {
    }

    #region Boilerplate - Will not change during the course
    private void TodoType_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (TodoType.SelectedIndex == -1) return;

        CreateTodoControlContainer.Children.Clear();

        var control = CreateUserControl(TodoType.SelectedValue.ToString() ?? "");

        CreateTodoControlContainer.Children.Add(control);
    }
    private void TodoItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var list = sender as ListView;

        if (list?.SelectedValue is null) return;

        CreateTodoControlContainer.Children.Clear();

        var control = CreateUserControl(
            list.SelectedValue.GetType().Name,
            list.SelectedValue as ToDo);

        CreateTodoControlContainer.Children.Add(control);

        CompletedItems.UnselectAll();

        UnfinishedItems.UnselectAll();
    }
    private IEnumerable<string> OpenFileDialog(string extension, string filter, bool multiselect)
    {
        var dialog = new OpenFileDialog
        {
            DefaultExt = extension,
            Filter = filter,
            Multiselect = multiselect
        };

        _ = dialog.ShowDialog();

        return dialog.FileNames;
    }
    private string OpenCreateFileDialog()
    {
        var dialog = new SaveFileDialog
        {
            DefaultExt = ".json",
            Filter = "JSON (.json)|*.json"
        };

        _ = dialog.ShowDialog();

        return dialog.FileName;
    }
    private void Hyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e)
    {
        Process.Start(new ProcessStartInfo { FileName = e.Uri.AbsoluteUri, UseShellExecute = true });

        e.Handled = true;
    }
    private void Close_OnClick(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }
    #endregion
}
