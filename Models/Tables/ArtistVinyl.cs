using System;
using System.Collections.Generic;

namespace MuseMusic.Models.Tables
{
    public partial class ArtistVinyl
    {
        public int Id { get; set; }
        public int? VinylId { get; set; }
        public int? ArtistId { get; set; }

        public virtual Artist? Artist { get; set; }
        public virtual Vinyl? Vinyl { get; set; }
    }
}
