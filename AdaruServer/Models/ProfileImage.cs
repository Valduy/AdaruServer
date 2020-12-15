using System;
using System.Collections.Generic;

#nullable disable

namespace AdaruServer.Models
{
    public partial class ProfileImage
    {
        public int IdProfile { get; set; }
        public int IdImage { get; set; }

        public virtual Image IdImageNavigation { get; set; }
        public virtual Profile IdProfileNavigation { get; set; }
    }
}
