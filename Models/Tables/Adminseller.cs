using System;
using System.Collections.Generic;

namespace MuseMusic.Models.Tables
{
    public partial class Adminseller
    {
        public Adminseller()
        {
            Blogs = new HashSet<Blog>();
            Orders = new HashSet<Order>();
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public int AccountId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }

        public virtual Account Account { get; set; } = null!;
        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
