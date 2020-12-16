using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdaruServer.Models;

namespace AdaruServer.ViewModels
{
    public class TaskViewModel
    {
        public Models.Task Task { get; set; }
        public ICollection<Tag> Tags { get; set; }
    }
}
