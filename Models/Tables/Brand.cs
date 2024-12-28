using System;
using System.Collections.Generic;

namespace MuseMusic.Models.Tables
{
    public partial class Brand
    {
        public Brand()
        {
            Accessories = new HashSet<Accessory>();
            Recordplayers = new HashSet<Recordplayer>();
            Vinyls = new HashSet<Vinyl>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Website { get; set; }

        public virtual ICollection<Accessory> Accessories { get; set; }
        public virtual ICollection<Recordplayer> Recordplayers { get; set; }
        public virtual ICollection<Vinyl> Vinyls { get; set; }
    }
}
