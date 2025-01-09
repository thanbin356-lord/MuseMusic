using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuseMusic.Models.ManagerModels
{
    public class CustomerViewModel
    {
        public List<Customer> Customers { get; set; }
    }

    public class Customer
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int OrderCount { get; set; }
    }
}