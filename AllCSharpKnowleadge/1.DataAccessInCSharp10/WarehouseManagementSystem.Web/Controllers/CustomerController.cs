using _1WarehouseManagementSystem.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagementSystem.Infrastructure.Data;

namespace WarehouseManagementSystem.Web.Controllers
{
    public class CustomerController : Controller
    {
        private IRepository<Customer> _customerRepository;
        public CustomerController(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public IActionResult Index(Guid? id)
        {
            if(id == null)
            {
                var customers = _customerRepository.All();
                return View(customers);
            }
            else
            {
                var customer = _customerRepository.Get(id.Value);
            }
            return View();
        }
    }
}
