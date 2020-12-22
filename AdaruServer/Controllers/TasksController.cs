﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AdaruServer.Extensions;
using AdaruServer.ViewModels;
using AutoMapper;
using DBRepository;
using DBRepository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Task = Models.Task;

namespace AdaruServer.Controllers
{
    [Route("api/[controller]")]
    public class TasksController : Controller
    {
        private ITaskRepository _taskRepository;
        private IClientRepository _clientRepository;
        private IRoleRepository _roleRepository;
        private IStatusRepository _statusRepository;
        private IMapper _mapper;

        public TasksController(
            ITaskRepository taskRepository, 
            IClientRepository clientRepository,
            IRoleRepository roleRepository,
            IStatusRepository statusRepository,
            IMapper mapper)
        {
            _taskRepository = taskRepository;
            _clientRepository = clientRepository;
            _roleRepository = roleRepository;
            _statusRepository = statusRepository;
            _mapper = mapper;
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

        // api/tasks/performer?id=1
        [HttpGet("performer")]
        public async Task<List<TaskViewModel>> GetPerformerTasks(int id)
        {
            var tasks = await _taskRepository.GetPerformerTasks(id);
            return await CreateTasksViewModelsAsync(tasks);
        }
        
        // api/tasks/add
        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddTask([FromBody]AddTaskViewModel task)
        {
            try
            {
                var userId = User.GetName();
                var newTask = _mapper.Map<Models.Task>(task);
                newTask.IdCustomer = int.Parse(userId);
                newTask.IdStatus = (await _statusRepository.GetTaskStatus("new")).Id;
                newTask.Time = DateAndTime.Now;
                await _taskRepository.AddTask(newTask);
            }
            catch (RepositoryException ex)
            {
                return BadRequest(new {message = ex.Message});
            }

            return Ok();
        }

        [Authorize]
        [HttpPost("add/tags")]
        public async Task<IActionResult> AddTagsToTask(int id, [FromBody]IEnumerable<string> tags)
        {
            try
            {
                // TODO
            }
            catch (RepositoryException ex)
            {
                BadRequest(new { message = ex.Message });
            }

            return Ok();
        }

        // api/tasks/status?id=1
        [Authorize]
        [HttpPost("status")]
        public async Task<IActionResult> ChangeTaskStatus(int id, [FromBody]string status)
        {
            var userId = User.GetName();
            var task = await _taskRepository.GetTask(id);

            if (task == null)
            {
                return BadRequest(new {message="Такой задачи нет."});
            }
            if (task.IdCustomer != int.Parse(userId))
            {
                return BadRequest(new {message="Попытка изменить не свою задачу"});
            }

            try
            {
                await _taskRepository.ChangeTaskStatus(id, status);
            }
            catch (RepositoryException ex)
            {
                return BadRequest(new {message = ex.Message});
            }

            return Ok();
        }

        private async Task<List<TaskViewModel>> CreateTasksViewModelsAsync(List<Task> tasks)
        {
            var result = new List<TaskViewModel>();

            foreach (var t in tasks)
            {
                var client = await _clientRepository.GetClient(t.IdCustomer);
                var task = _mapper.Map<TaskViewModel>(t);
                task.Tags = (await _taskRepository.GetTaskTags(t.Id)).Select(tag => tag.Name).ToList();
                task.Status = (await _statusRepository.GetTaskStatus(t.IdStatus)).Status;
                task.Customer = _mapper.Map<ClientViewModel>(client);
                task.Customer.Role = (await _roleRepository.GetUserRole(client.IdRole)).Role;
                result.Add(task);
            }

            return result;
        }
    }
}
