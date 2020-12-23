using System;
using System.Collections.Generic;

namespace AdaruServer.ViewModels
{
    public class PerformerInfoViewModel : ClientInfoViewModel
    {
        public IEnumerable<string> Tags { get; set; }
    }
}
