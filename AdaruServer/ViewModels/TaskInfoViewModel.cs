using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdaruServer.ViewModels
{
    public class TaskInfoViewModel : TaskViewModel
    {
        public ClientViewModel Performer { get; set; }
    }
}
