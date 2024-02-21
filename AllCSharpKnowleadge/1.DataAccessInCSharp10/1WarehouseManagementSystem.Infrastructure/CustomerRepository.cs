using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Infrastructure.Data;

namespace _1WarehouseManagementSystem.Infrastructure
{
    public class CustomerRepository : GenericRepository<Customer>
    {
        public CustomerRepository(WarehouseContext contextWare) : base(contextWare)
        {
        }

        public override Customer Update(Customer entity)
        {
            Customer update = Get(entity.Id);

            update.Address = entity.Address;
            update.PhoneNumber = entity.PhoneNumber;
            update.Name = entity.Name;
            update.Country = entity.Country;
            update.PostalCode = entity.PostalCode;

            return base.Update(update);
        }
    }
}
