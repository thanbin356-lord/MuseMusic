using System;
using System.Collections.Generic;

namespace MuseMusic.Models.Tables
{
    public partial class Vinyl
    {
        public Vinyl()
        {
            ArtistVinyls = new HashSet<ArtistVinyl>();
            CategoriesVinyls = new HashSet<CategoriesVinyl>();
            MoodVinyls = new HashSet<MoodVinyl>();
        }

        public int Id { get; set; }
        public string? DiskId { get; set; }
        public int? ProductId { get; set; }
        public int? Years { get; set; }
        public string? Tracklist { get; set; }
        public int? BrandId { get; set; }
        public string? BrandName { get; set; }  // Store the brand name directly here

        public virtual Brand? Brand { get; set; }
        public virtual Product? Product { get; set; }
        public virtual ICollection<ArtistVinyl> ArtistVinyls { get; set; }
        public virtual ICollection<CategoriesVinyl> CategoriesVinyls { get; set; }
        public virtual ICollection<MoodVinyl> MoodVinyls { get; set; }
    }
}
