using System;
using System.Collections.Generic;

#nullable disable

namespace AdaruServer.Models
{
    public partial class Tag
    {
        public Tag()
        {
            ImageTags = new HashSet<ImageTag>();
            PerformerTags = new HashSet<PerformerTag>();
            TaskTags = new HashSet<TaskTag>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ImageTag> ImageTags { get; set; }
        public virtual ICollection<PerformerTag> PerformerTags { get; set; }
        public virtual ICollection<TaskTag> TaskTags { get; set; }
    }
}
