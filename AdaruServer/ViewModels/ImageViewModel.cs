using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdaruServer.ViewModels
{
    public class ImageViewModel
    {
        public string Image { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}
