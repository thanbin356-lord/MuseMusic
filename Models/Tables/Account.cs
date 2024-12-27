using System;
using System.Collections.Generic;

namespace MuseMusic.Models.Tables
{
    public partial class Account
    {
        public Account()
        {
            AccountRoles = new HashSet<AccountRole>();
            Adminsellers = new HashSet<Adminseller>();
            Customers = new HashSet<Customer>();
        }

        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Email { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<AccountRole> AccountRoles { get; set; }
        public virtual ICollection<Adminseller> Adminsellers { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
    }
}
