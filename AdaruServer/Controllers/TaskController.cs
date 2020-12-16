using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdaruServer.DBRepositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Task = AdaruServer.Models.Task;

namespace AdaruServer.Controllers
{
    [Route("api")]
    public class TaskController : Controller
    {
        private ITaskRepository _taskRepository;

        public TaskController(ITaskRepository taskRepository) 
            => _taskRepository = taskRepository;
        
        [Route("tasks")]
        [HttpGet]
        public async Task<List<Models.Task>> GetTasks()
        {
            // TODO: потенциально, здесь может быть исключение, если задач нет...
            return await _taskRepository.GetAllTasks();
        }
    }
}
