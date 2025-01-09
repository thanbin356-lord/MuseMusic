using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using MuseMusic.Models.Tables;

namespace MuseMusic.Models.ManagerModels
{
    public class OrderViewModel
    {
        public List<Orders> Orders { get; set; }

        public Orders SelectedCustomerOrders { get; set; }

    }
    public class Orders
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime created_at { get; set; }
        public decimal total { get; set; }
        public string Status { get; set; }
        public string PaymentMethod { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int OrderCount { get; set; }
        

        public List<ListProduct> AllProducts { get; set; }
    }
    public class ListProduct
    {
        public string ProductName { get; set; }
        public List<string> ArtistNames { get; set; }
        public string BrandName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}