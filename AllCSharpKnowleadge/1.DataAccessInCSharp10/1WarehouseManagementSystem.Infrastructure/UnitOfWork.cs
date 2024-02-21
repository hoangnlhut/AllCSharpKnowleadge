using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Infrastructure.Data;

namespace _1WarehouseManagementSystem.Infrastructure
{
    public interface IUnitOfWork
    {
        IRepository<Customer> CustomerRepository { get; }
        IRepository<Order> OrderRepository { get; }
        IRepository<Item> ItemRepository { get; }
        IRepository<ShippingProvider> ShippingProviderRepository { get; }

        void SaveChanges();

    }
    public class UnitOfWork : IUnitOfWork
    {
        private WarehouseContext _context;
        public UnitOfWork(WarehouseContext Context)
        {
            _context = Context;
        }

        private IRepository<Customer> _customerRepository;
        public IRepository<Customer> CustomerRepository { 
            get 
            {
                if (_customerRepository == null)
                {
                    _customerRepository = new CustomerRepository(_context);
                }
                return _customerRepository;
            } 
        }
        private IRepository<Order> _orderRepository;
        public IRepository<Order> OrderRepository
        {
            get
            {
                if (_orderRepository == null)
                {
                    _orderRepository = new OrderRepository(_context);
                }
                return _orderRepository;
            }
        }

        private IRepository<Item> _itemRepository;
        public IRepository<Item> ItemRepository 
        { 
            get 
            {
                if (_itemRepository == null)
                {
                    _itemRepository = new ItemRepository(_context);
                }
                return _itemRepository;
            } 
        }

        private IRepository<ShippingProvider> _shippingProviderRepository;
        public IRepository<ShippingProvider> ShippingProviderRepository 
        { 
            get
            {
                if (_shippingProviderRepository == null)
                {
                    _shippingProviderRepository = new ShippingProviderRepository(_context);
                }
                return _shippingProviderRepository;
            }
        }

        public void SaveChanges() => _context.SaveChanges();
       
    }
}
