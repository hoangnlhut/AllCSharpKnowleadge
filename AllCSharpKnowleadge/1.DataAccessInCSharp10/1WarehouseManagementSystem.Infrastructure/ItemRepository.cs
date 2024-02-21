using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Infrastructure.Data;

namespace _1WarehouseManagementSystem.Infrastructure
{
    public class ItemRepository : GenericRepository<Item>
    {
        public ItemRepository(WarehouseContext contextWare) : base(contextWare)
        {
        }

        public override Item Update(Item entity)
        {
            Item toUpdate = Get(entity.Id);

            toUpdate.Price = entity.Price;
            toUpdate.Name = entity.Name;

            return base.Update(toUpdate);
        }
    }
}
