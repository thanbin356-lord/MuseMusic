using System;
using System.Collections.Generic;

namespace MuseMusic.Models.Tables
{
    public partial class Product
    {
        public Product()
        {
            Accessories = new HashSet<Accessory>();
            ImageUrls = new HashSet<ImageUrl>();
            OrderDetails = new HashSet<OrderDetail>();
            Recordplayers = new HashSet<Recordplayer>();
            Vinyls = new HashSet<Vinyl>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int? AdminsellerId { get; set; }
        public int? Quantity { get; set; }
        public int? BrandId { get; set; }

        public virtual Adminseller? Adminseller { get; set; }
        public virtual Brand? Brand { get; set; }
        public virtual ICollection<Accessory> Accessories { get; set; }
        public virtual ICollection<ImageUrl> ImageUrls { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Recordplayer> Recordplayers { get; set; }
        public virtual ICollection<Vinyl> Vinyls { get; set; }
    }
}
