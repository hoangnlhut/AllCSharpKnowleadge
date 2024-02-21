using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Infrastructure.Data;

namespace _1WarehouseManagementSystem.Infrastructure
{
    public class OrderRepository : GenericRepository<Order>
    {
        public OrderRepository(WarehouseContext contextWare) : base(contextWare)
        {
        }

        public override IEnumerable<Order> Find(Expression<Func<Order, bool>> predicate)
        {
            return context.Orders
                .Include(order => order.LineItems)
                .ThenInclude(lineItem => lineItem.Item)
                .Where(predicate)
                .ToList();
        }

        public override Order Update(Order entity)
        {
            Order toUpdated = context.Orders
                .Include(order => order.LineItems)
                .ThenInclude(lineItem => lineItem.Item)
                .Single(order => order.Id == entity.Id);

            toUpdated.CreatedAt = entity.CreatedAt;
            toUpdated.LineItems = entity.LineItems;

            return base.Update(toUpdated);
        }
    }
}
