using System;
using System.Collections.Generic;

namespace MuseMusic.Models.Tables
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int AdminsellerId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? Status { get; set; }
        public int? PaymentId { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public DateTime? ExpectedStartDate { get; set; }
        public DateTime? ExpectedEndDate { get; set; }
        public decimal? Total { get; set; }

        public virtual Adminseller Adminseller { get; set; } = null!;
        public virtual Customer Customer { get; set; } = null!;
        public virtual Payment? Payment { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
