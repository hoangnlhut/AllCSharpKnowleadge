using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagementSystem.Infrastructure.Data
{
    public class WarehouseContext : DbContext
    {
        public DbSet<Warehouse> Warehouse { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ShippingProvider> ShippingProviders { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<LineItem> LineItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-J76PCRA\\SQLEXPRESS;Initial Catalog=WAREHOUSEMANAGEMENT;Persist Security Info=True;User ID=sa;Password=123456hoang");
        }
    }
}
