using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdaruServer.ViewModels
{
    public class MessageViewModel
    {
        public int Id { get; set; }
        public int IdSender { get; set; }
        public string Username { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
    }
}
