﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdaruServer.ViewModels;
using DBRepository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Task = Models.Task;

namespace AdaruServer.Controllers
{
    [Route("api/[controller]")]
    public class TasksController : Controller
    {
        private ITaskRepository _taskRepository;
        private IClientRepository _clientRepository;
        private IStatusRepository _statusRepository;

        public TasksController(
            ITaskRepository taskRepository, 
            IClientRepository clientRepository, 
            IStatusRepository statusRepository)
        {
            _taskRepository = taskRepository;
            _clientRepository = clientRepository;
            _statusRepository = statusRepository;
        }

        // api/tasks/all
        [HttpGet("all")]
        public async Task<List<TaskViewModel>> GetTasks()
        {
            var tasks = await _taskRepository.GetAllTasks();
            return await CreateTasksViewModelsAsync(tasks);
        }

        // api/tasks/new
        [HttpGet("new")]
        public async Task<List<TaskViewModel>> GetNewTasks()
        {
            var tasks = await _taskRepository.GetNewTasks();
            return await CreateTasksViewModelsAsync(tasks);
        }

        // api/tasks/customer?id=1
        [HttpGet("customer")]
        public async Task<List<TaskViewModel>> GetCustomerTasks(int id)
        {
            var tasks = await _taskRepository.GetCustomerTasks(id);
            return await CreateTasksViewModelsAsync(tasks);
        }

        public async Task<List<TaskViewModel>> GetPerformerTasks(int id)
        {
            return null;
        }

        private async Task<List<TaskViewModel>> CreateTasksViewModelsAsync(List<Task> tasks)
        {
            var result = new List<TaskViewModel>();

            foreach (var t in tasks)
            {
                result.Add(new TaskViewModel
                {
                    Task = t,
                    Tags = await _taskRepository.GetTaskTags(t.Id),
                    Status = (await _statusRepository.GetTaskStatus(t.IdStatus)).Status
                });
            }

            return result;
        }
    }
}
