﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using Task = Models.Task;

namespace AdaruServer.ViewModels
{
    public class TaskViewModel
    {
        public Task Task { get; set; }
        public ICollection<Tag> Tags { get; set; }
    }
}
