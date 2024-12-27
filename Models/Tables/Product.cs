using System;
using System.Collections.Generic;

namespace MuseMusic.Models.Tables
{
    public partial class Product
    {
        public Product()
        {
            Accessories = new HashSet<Accessory>();
            CartDetails = new HashSet<CartDetail>();
            OrderDetails = new HashSet<OrderDetail>();
            Vinyls = new HashSet<Vinyl>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int? AdminsellerId { get; set; }

        public virtual Adminseller? Adminseller { get; set; }
        public virtual ICollection<Accessory> Accessories { get; set; }
        public virtual ICollection<CartDetail> CartDetails { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Vinyl> Vinyls { get; set; }
    }
}
