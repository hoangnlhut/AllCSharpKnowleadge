 using _1WarehouseManagementSystem.Infrastructure;
using WarehouseManagementSystem.Infrastructure.Data;

namespace WarehouseManagementSystem.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // setup dependency injection
            builder.Services.AddTransient<WarehouseContext>();
            builder.Services.AddTransient<IRepository<Order>, OrderRepository>();
            builder.Services.AddTransient<IRepository<Customer>, CustomerRepository>();
            builder.Services.AddTransient<IRepository<ShippingProvider>, ShippingProviderRepository>();
            builder.Services.AddTransient<IRepository<Item>, ItemRepository>();
            builder.Services.AddTransient<IRepository<Warehouse>, WarehouseRepository>();
            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}