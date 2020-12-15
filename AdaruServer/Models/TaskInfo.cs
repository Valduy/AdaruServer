using System;
using System.Collections.Generic;

#nullable disable

namespace AdaruServer.Models
{
    public partial class TaskInfo
    {
        public int? Id { get; set; }
        public string Login { get; set; }
        public string Username { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }
    }
}
