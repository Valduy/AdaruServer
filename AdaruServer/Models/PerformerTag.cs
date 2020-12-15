using System;
using System.Collections.Generic;

#nullable disable

namespace AdaruServer.Models
{
    public partial class PerformerTag
    {
        public int IdPerformer { get; set; }
        public int IdTag { get; set; }

        public virtual Client IdPerformerNavigation { get; set; }
        public virtual Tag IdTagNavigation { get; set; }
    }
}
