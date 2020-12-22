using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using Task = Models.Task;

namespace AdaruServer.ViewModels
{
    public class TaskViewModel
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }
        public DateTime Time { get; set; }
        public ClientViewModel Customer { get; set; }
        public ICollection<string> Tags { get; set; }
    }
}
