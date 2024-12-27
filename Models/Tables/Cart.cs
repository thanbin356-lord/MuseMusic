using System;
using System.Collections.Generic;

namespace MuseMusic.Models.Tables
{
    public partial class Cart
    {
        public Cart()
        {
            CartDetails = new HashSet<CartDetail>();
        }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual ICollection<CartDetail> CartDetails { get; set; }
    }
}
