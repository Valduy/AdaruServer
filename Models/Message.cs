using System;

#nullable disable

namespace Models
{
    public partial class Message
    {
        public int Id { get; set; }
        public int IdChat { get; set; }
        public int IdSender { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }

        public virtual Chat IdChatNavigation { get; set; }
        public virtual Client IdSenderNavigation { get; set; }
    }
}
