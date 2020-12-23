using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdaruServer.ViewModels
{
    public class ClientInfoViewModel
    {
        public int? Id { get; set; }
        public string Login { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string Resume { get; set; }
        public decimal? Raiting { get; set; }
        public long? Expirience { get; set; }
        // TODO: картинка
    }
}
