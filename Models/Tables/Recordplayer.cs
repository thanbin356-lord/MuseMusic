﻿using System;
using System.Collections.Generic;

namespace MuseMusic.Models.Tables
{
    public partial class Recordplayer
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int? BrandId { get; set; }

        public virtual Brand? Brand { get; set; }
        public virtual Product? Product { get; set; }
    }
}
