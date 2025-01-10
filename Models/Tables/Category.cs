using System;
using System.Collections.Generic;

namespace MuseMusic.Models.Tables
{
    public partial class Category
    {
        public Category()
        {
            CategoriesVinyls = new HashSet<CategoriesVinyl>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? CategoryImage { get; set; }

        public virtual ICollection<CategoriesVinyl> CategoriesVinyls { get; set; }
    }
}
