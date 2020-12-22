using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdaruServer.ViewModels
{
    public class ReviewViewModel
    {
        public string Content { get; set; }
        public short Mark { get; set; }
        public DateTime Time { get; set; }
        public ClientViewModel Sender { get; set; }
    }
}
