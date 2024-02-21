﻿using _1WarehouseManagementSystem.Infrastructure;
using System;
using System.Collections.Generic;

namespace WarehouseManagementSystem.Infrastructure.Data
{
    public class Customer
    {
        private byte[]? logo;

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
        public byte[]? Logo 
        {
            get
            {
                if (logo is null)
                {
                    logo = LogoService.GetFor(string.Empty);
                }
                return logo;
            }
            set
            {
                logo = value;
            }
        }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
