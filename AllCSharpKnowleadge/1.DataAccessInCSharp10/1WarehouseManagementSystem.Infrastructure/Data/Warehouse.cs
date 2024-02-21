using System;
using System.Collections.Generic;

namespace WarehouseManagementSystem.Infrastructure.Data
{
    public class Warehouse
    {
        public Warehouse()
        {
            Items = new HashSet<Item>();
        }

        public Guid Id { get; set; }
        public string Location { get; set; } = null!;

        public virtual ICollection<Item> Items { get; set; }
    }
}
