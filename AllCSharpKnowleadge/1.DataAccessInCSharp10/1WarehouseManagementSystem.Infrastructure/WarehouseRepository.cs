using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Infrastructure.Data;

namespace _1WarehouseManagementSystem.Infrastructure
{
    public class WarehouseRepository : GenericRepository<Warehouse>
    {
        public WarehouseRepository(WarehouseContext contextWare) : base(contextWare)
        {
        }

        public override Warehouse Update(Warehouse entity)
        {
            Warehouse toUpdate = Get(entity.Id);

            toUpdate.Location = entity.Location;

            return base.Update(toUpdate);
        }
    }
}
