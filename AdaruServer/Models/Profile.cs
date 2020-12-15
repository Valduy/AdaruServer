using System;
using System.Collections.Generic;

#nullable disable

namespace AdaruServer.Models
{
    public partial class Profile
    {
        public Profile()
        {
            ProfileImages = new HashSet<ProfileImage>();
        }

        public int IdClient { get; set; }
        public string Resume { get; set; }

        public virtual Client IdClientNavigation { get; set; }
        public virtual ICollection<ProfileImage> ProfileImages { get; set; }
    }
}
