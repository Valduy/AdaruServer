using System;
using System.Collections.Generic;

#nullable disable

namespace Models
{
    public partial class Task
    {
        public Task()
        {
            TaskTags = new HashSet<TaskTag>();
        }

        public int Id { get; set; }
        public int IdCustomer { get; set; }
        public int? IdPerformer { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public int IdStatus { get; set; }
        public DateTime Time { get; set; }

        public virtual Client IdCustomerNavigation { get; set; }
        public virtual Client IdPerformerNavigation { get; set; }
        public virtual TaskStatus IdStatusNavigation { get; set; }
        public virtual ICollection<TaskTag> TaskTags { get; set; }
    }
}
