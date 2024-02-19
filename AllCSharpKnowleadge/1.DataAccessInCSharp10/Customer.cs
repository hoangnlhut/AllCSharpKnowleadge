using System;
using System.Collections.Generic;

namespace _1.DataAccessInCSharp10
{
    public class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; }
    }
}
