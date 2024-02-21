using _1WarehouseManagementSystem.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Net;
using System.Xml.Linq;
using WarehouseManagementSystem.Infrastructure.Data;
using WarehouseManagementSystem.Web.Models;

namespace WarehouseManagementSystem.Web.Controllers
{
    public class OrderController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var orders = _unitOfWork.OrderRepository.Find(order => order.CreatedAt > DateTime.UtcNow.AddDays(-1));
            return View(orders);
        }

        public IActionResult Create()
        {
            // create
            var items = _unitOfWork.ItemRepository.All();
            return View(items);
        }

        [HttpPost]
        public IActionResult Create(CreateOrderModel model)
        {
            #region Validate input
            #endregion

            var existedCustomer = _unitOfWork.CustomerRepository.Find(customer => customer.Name == model.Customer.Name).FirstOrDefault();

            if (existedCustomer is null)
            {
                existedCustomer = new Customer()
                {
                    Name = model.Customer.Name,
                    Address = model.Customer.Address,
                    PostalCode = model.Customer.PostalCode,
                    Country = model.Customer.Country,
                    PhoneNumber = model.Customer.PhoneNumber,
                };
            }
            else
            {
                existedCustomer.Name = model.Customer.Name;
                existedCustomer.Address = model.Customer.Address;
                existedCustomer.PostalCode = model.Customer.PostalCode;
                existedCustomer.Country = model.Customer.Country;
                existedCustomer.PhoneNumber = model.Customer.PhoneNumber;

                _unitOfWork.CustomerRepository.Update(existedCustomer);
            }
            

            var order = new Order()
            {
                LineItems = model.LineItems.Select(line => new LineItem
                {
                    Id = Guid.NewGuid(),
                    ItemId = line.ItemId,
                    Quantity = line.Quantity
                }).ToList(),

                Customer = existedCustomer,
                ShippingProviderId = _unitOfWork.ShippingProviderRepository.All().First().Id,
                CreatedAt = DateTimeOffset.UtcNow,
            };
            //create
            _unitOfWork.OrderRepository.Add(order);
            _unitOfWork.SaveChanges();

            return Ok("Order Created");
        }

        public IActionResult Error()
        {
            
            return View();
        }


    }
}
