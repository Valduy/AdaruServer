using System;
using System.Collections.Generic;

#nullable disable

namespace AdaruServer.Models
{
    public partial class ClientInfo
    {
        public int? Id { get; set; }
        public string Login { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string Path { get; set; }
        public string Resume { get; set; }
    }
}
