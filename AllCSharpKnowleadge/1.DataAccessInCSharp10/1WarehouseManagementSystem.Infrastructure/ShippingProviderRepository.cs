using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Infrastructure.Data;

namespace _1WarehouseManagementSystem.Infrastructure
{
    public class ShippingProviderRepository : GenericRepository<ShippingProvider>
    {
        public ShippingProviderRepository(WarehouseContext contextWare) : base(contextWare)
        {
        }

        public override ShippingProvider Update(ShippingProvider entity)
        {
            ShippingProvider toUpdate = Get(entity.Id);

            toUpdate.FreightCost = entity.FreightCost;
            toUpdate.Name = entity.Name;

            return base.Update(toUpdate);
        }
    }
}
