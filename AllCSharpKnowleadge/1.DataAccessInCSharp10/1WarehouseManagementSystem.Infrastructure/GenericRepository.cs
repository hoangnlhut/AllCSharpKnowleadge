using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Infrastructure.Data;

namespace _1WarehouseManagementSystem.Infrastructure
{
    public abstract class GenericRepository<T> : IRepository<T> where T : class
    {
        protected WarehouseContext context;
        public GenericRepository(WarehouseContext contextWare)
        {
            context = contextWare;
        }
        public virtual T Add(T entity)
        {
           var addedEntity =  context.Add(entity).Entity;
           return addedEntity;
        }
        public virtual IEnumerable<T> All()
        {
            var all = context.Set<T>().ToList();
            return all;
        }
        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            var result = context.Set<T>().AsQueryable().Where(predicate).ToList();
            return result;
        }
        public virtual T Get(Guid id)
        {
            return context.Find<T>(id);
        }
        public virtual T Update(T entity)
        {
           return context.Update<T>(entity).Entity;
        }
        public virtual void SaveChange()
        {
            context.SaveChanges();
        }
    }
}
