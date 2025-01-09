using System;
using System.Collections.Generic;

namespace MuseMusic.Models.Tables
{
    public partial class Accessory
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }

        public virtual Product? Product { get; set; }
    }
}
