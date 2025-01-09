using System;
using System.Collections.Generic;

namespace MuseMusic.Models.Tables
{
    public partial class Brand
    {
        public Brand()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Website { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
