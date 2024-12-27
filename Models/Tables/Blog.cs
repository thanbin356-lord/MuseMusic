using System;
using System.Collections.Generic;

namespace MuseMusic.Models.Tables
{
    public partial class Blog
    {
        public int Id { get; set; }
        public string? Nameblog { get; set; }
        public DateTime? CreateAt { get; set; }
        public int AdminId { get; set; }
        public string? DescriBlog { get; set; }

        public virtual Adminseller Admin { get; set; } = null!;
    }
}
