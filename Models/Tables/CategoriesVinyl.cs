using System;
using System.Collections.Generic;

namespace MuseMusic.Models.Tables
{
    public partial class CategoriesVinyl
    {
        public int Id { get; set; }
        public int? VinylId { get; set; }
        public int? CategoryId { get; set; }

        public virtual Category? Category { get; set; }
        public virtual Vinyl? Vinyl { get; set; }
    }
}
