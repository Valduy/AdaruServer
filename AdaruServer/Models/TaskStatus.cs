using System;
using System.Collections.Generic;

#nullable disable

namespace AdaruServer.Models
{
    public partial class TaskStatus
    {
        public TaskStatus()
        {
            Tasks = new HashSet<Task>();
        }

        public int Id { get; set; }
        public string Status { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
    }
}
