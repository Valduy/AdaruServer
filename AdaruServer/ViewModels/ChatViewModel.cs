using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace AdaruServer.ViewModels
{
    public class ChatViewModel
    {
        public int Id { get; set; }
        public ClientViewModel Target { get; set; }
        public IEnumerable<MessageViewModel> Messages { get; set; }
    }
}
