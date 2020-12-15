using System;
using System.Collections.Generic;

#nullable disable

namespace AdaruServer.Models
{
    public partial class Client
    {
        public Client()
        {
            ChatIdSourceNavigations = new HashSet<Chat>();
            ChatIdTargetNavigations = new HashSet<Chat>();
            Messages = new HashSet<Message>();
            PerformerTags = new HashSet<PerformerTag>();
            ReviewIdSourceNavigations = new HashSet<Review>();
            ReviewIdTargetNavigations = new HashSet<Review>();
            TaskIdCustomerNavigations = new HashSet<Task>();
            TaskIdPerformerNavigations = new HashSet<Task>();
        }

        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public int IdRole { get; set; }
        public int? IdImage { get; set; }

        public virtual Image IdImageNavigation { get; set; }
        public virtual UserRole IdRoleNavigation { get; set; }
        public virtual Profile Profile { get; set; }
        public virtual ICollection<Chat> ChatIdSourceNavigations { get; set; }
        public virtual ICollection<Chat> ChatIdTargetNavigations { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<PerformerTag> PerformerTags { get; set; }
        public virtual ICollection<Review> ReviewIdSourceNavigations { get; set; }
        public virtual ICollection<Review> ReviewIdTargetNavigations { get; set; }
        public virtual ICollection<Task> TaskIdCustomerNavigations { get; set; }
        public virtual ICollection<Task> TaskIdPerformerNavigations { get; set; }
    }
}
