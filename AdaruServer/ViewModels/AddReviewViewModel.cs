using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdaruServer.ViewModels
{
    public class AddReviewViewModel
    {
        public string Content { get; set; }
        public short Mark { get; set; }
        public int IdTarget { get; set; }
    }
}
