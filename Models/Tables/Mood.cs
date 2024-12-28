using System;
using System.Collections.Generic;

namespace MuseMusic.Models.Tables
{
    public partial class Mood
    {
        public Mood()
        {
            MoodVinyls = new HashSet<MoodVinyl>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<MoodVinyl> MoodVinyls { get; set; }
    }
}
