using System;
using System.Collections.Generic;

namespace MuseMusic.Models.Tables
{
    public partial class Recordplayer
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public string? Motor { get; set; }
        public string? Speed { get; set; }

        public virtual Product? Product { get; set; }
    }
}
