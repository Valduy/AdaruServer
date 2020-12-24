using System;

#nullable disable

namespace Models
{
    public partial class TaskInfo
    {
        public int? IdClient { get; set; }
        public string Login { get; set; }
        public string Username { get; set; }
        public int? IdTask { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }
        public DateTime? Time { get; set; }
    }
}
