using System;
using System.Collections.Generic;

namespace MuseMusic.Models.Tables
{
    public partial class Role
    {
        public Role()
        {
            AccountRoles = new HashSet<AccountRole>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<AccountRole> AccountRoles { get; set; }
    }
}
