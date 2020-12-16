using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdaruServer.DBRepositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Task = AdaruServer.Models.Task;

namespace AdaruServer.Controllers
{
    //[Route("api/[controller]")]
    public class TaskController : Controller
    {
        private ITaskRepository _taskRepository;

        public TaskController(ITaskRepository taskRepository) 
            => _taskRepository = taskRepository;

        [Route("tasks")]
        [HttpGet]
        public async Task<List<Models.Task>> GetTasks()
        {
            return await _taskRepository.GetAllTasks();
        }
        //public List<Models.Task> GetTasks()
        //{
        //    return _taskRepository.Get();
        //}
    }
}
