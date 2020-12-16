using System;
using System.Collections.Generic;

#nullable disable

namespace AdaruServer.Models
{
    public partial class UserRole
    {
        public UserRole()
        {
            Clients = new HashSet<Client>();
        }

        public int Id { get; set; }
        public string Role { get; set; }

        public virtual ICollection<Client> Clients { get; set; }
    }
}
