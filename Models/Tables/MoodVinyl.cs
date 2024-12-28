using System;
using System.Collections.Generic;

namespace MuseMusic.Models.Tables
{
    public partial class MoodVinyl
    {
        public int Id { get; set; }
        public int? VinylId { get; set; }
        public int? MoodId { get; set; }

        public virtual Mood? Mood { get; set; }
        public virtual Vinyl? Vinyl { get; set; }
    }
}
