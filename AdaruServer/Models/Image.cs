using System;
using System.Collections.Generic;

#nullable disable

namespace AdaruServer.Models
{
    public partial class Image
    {
        public Image()
        {
            Clients = new HashSet<Client>();
            ImageTags = new HashSet<ImageTag>();
            ProfileImages = new HashSet<ProfileImage>();
        }

        public int Id { get; set; }
        public string Path { get; set; }

        public virtual ICollection<Client> Clients { get; set; }
        public virtual ICollection<ImageTag> ImageTags { get; set; }
        public virtual ICollection<ProfileImage> ProfileImages { get; set; }
    }
}
