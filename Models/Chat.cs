using System.Collections.Generic;

#nullable disable

namespace Models
{
    public partial class Chat
    {
        public Chat()
        {
            Messages = new HashSet<Message>();
        }

        public int Id { get; set; }
        public int IdSource { get; set; }
        public int IdTarget { get; set; }

        public virtual Client IdSourceNavigation { get; set; }
        public virtual Client IdTargetNavigation { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}
