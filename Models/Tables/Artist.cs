using System;
using System.Collections.Generic;

namespace MuseMusic.Models.Tables
{
    public partial class Artist
    {
        public Artist()
        {
            ArtistVinyls = new HashSet<ArtistVinyl>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Artisitmageturl { get; set; }

        public virtual ICollection<ArtistVinyl> ArtistVinyls { get; set; }
    }
}
