using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdaruServer.ViewModels;
using DBRepository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Task = Models.Task;

namespace AdaruServer.Controllers
{
    [Route("api")]
    public class TaskController : Controller
    {
        private ITaskRepository _taskRepository;

        public TaskController(ITaskRepository taskRepository) 
            => _taskRepository = taskRepository;
        
        [Route("tasks/all")]
        [HttpGet]
        // TODO: потенциально, возможно, здесь может быть исключение, если задач нет...
        public async Task<List<TaskViewModel>> GetTasks()
        {
            var tasks = await _taskRepository.GetAllTasks();
            return await CreateTasksViewModelsAsync(tasks);
        }

        [Route("tasks/new")]
        [HttpGet]
        public async Task<List<TaskViewModel>> GetNewTasks()
        {
            var tasks = await _taskRepository.GetNewTasks();
            return await CreateTasksViewModelsAsync(tasks);
        }

        [Route("tasks/customer")]
        [HttpGet]
        public async Task<List<TaskViewModel>> GetCustomerTasks(int id)
        {
            var tasks = await _taskRepository.GetCustomerTasks(id);
            return await CreateTasksViewModelsAsync(tasks);
        }
        
        private async Task<List<TaskViewModel>> CreateTasksViewModelsAsync(List<Task> tasks)
        {
            var result = new List<TaskViewModel>();

            foreach (var t in tasks)
            {
                result.Add(new TaskViewModel
                {
                    Task = t,
                    Tags = await _taskRepository.GetTaskTags(t.Id)
                });
            }

            return result;
        }
    }
}
