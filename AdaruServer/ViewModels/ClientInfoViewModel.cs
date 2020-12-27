using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdaruServer.ViewModels
{
    public class ClientInfoViewModel : ClientViewModel
    {
        public string Resume { get; set; }
        public decimal? Raiting { get; set; }
        public long? Expirience { get; set; }
    }
}
