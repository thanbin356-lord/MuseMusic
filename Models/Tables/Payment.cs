using System;
using System.Collections.Generic;

namespace MuseMusic.Models.Tables
{
    public partial class Payment
    {
        public Payment()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Method { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; }
    }
}
