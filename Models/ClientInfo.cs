﻿#nullable disable

namespace Models
{
    public partial class ClientInfo
    {
        public int? Id { get; set; }
        public string Login { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public int? IdImage { get; set; }
        public string Resume { get; set; }
    }
}
