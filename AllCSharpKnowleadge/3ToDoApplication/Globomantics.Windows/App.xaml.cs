using Globamantics.Domain;
using Globamantics.Infrastructure.Data;
using Globamantics.Infrastructure.Data.Repositories;
using Globomantics.Windows.Factory;
using Globomantics.Windows.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Windows;

namespace Globomantics.Windows;

public partial class App : Application
{
    public static User CurrentUser { get; set; } = default!;
    public IServiceProvider ServiceProvider { get; init; } 
    public IConfiguration Configuration { get; init; }

    public App()
    {
        var builder = new ConfigurationBuilder();

        Configuration = builder.Build();

        var serviceCollection = new ServiceCollection();

        serviceCollection.AddDbContext<GlobomanticsDbContext>(ServiceLifetime.Scoped);

        //serviceCollection.AddSingleton<IRepository<Bug>, ToDoInMemoryRepository<Bug>>();
        //serviceCollection.AddSingleton<IRepository<Feature>, ToDoInMemoryRepository<Feature>>();
        //serviceCollection.AddSingleton<IRepository<TodoTask>, ToDoInMemoryRepository<TodoTask>>();
        serviceCollection.AddSingleton<IRepository<Bug>, BugRepository>();
        serviceCollection.AddSingleton<IRepository<Feature>, FeatureRepository>();
        serviceCollection.AddSingleton<IRepository<TodoTask>, ToDoTaskRepository>();
        serviceCollection.AddSingleton<IRepository<User>, UserRepository>();


        serviceCollection.AddTransient<ToDoViewModelFactory>();
        serviceCollection.AddTransient<FeatureViewModel>();
        serviceCollection.AddTransient<BugViewModel>();
        serviceCollection.AddTransient<MainViewModel>();
        serviceCollection.AddTransient<MainWindow>();
        serviceCollection.AddTransient(_ => ServiceProvider!);
        
        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    private async void OnStartup(object sender, StartupEventArgs e)
    {
        var context = ServiceProvider.GetRequiredService<GlobomanticsDbContext>();

        //this connexts to the database and applies the changes
        await context.Database.MigrateAsync();

        //seed some data
        var user = context.Users.FirstOrDefault();
        if (user == null)
        {
            user = new Globamantics.Infrastructure.Data.Models.User { Name = "Hoang" };
            context.Users.Add(user);
            context.SaveChanges();
        }

        CurrentUser = DataToDomainMapping.MapUser(user);


        var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();

        mainWindow?.Show();
    }
}
