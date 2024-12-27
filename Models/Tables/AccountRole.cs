using System;
using System.Collections.Generic;

namespace MuseMusic.Models.Tables
{
    public partial class AccountRole
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int RoleId { get; set; }

        public virtual Account Account { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
    }
}
