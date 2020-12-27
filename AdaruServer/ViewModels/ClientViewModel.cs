using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdaruServer.ViewModels
{
    public class ClientViewModel
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public int? IdImage { get; set; }
    }
}
