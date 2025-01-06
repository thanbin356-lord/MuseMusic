using System;
using System.Collections.Generic;

namespace MuseMusic.Models.Tables
{
    public partial class ImageUrl
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Url { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public bool? IsPrimary { get; set; }

        public virtual Product Product { get; set; } = null!;
    }
}
