using System;
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
    public class TaskController : Controller
    {
        private ITaskRepository _taskRepository;
        private IClientRepository _clientRepository;
        private IRoleRepository _roleRepository;
        private IStatusRepository _statusRepository;
        private IMapper _mapper;

        public TaskController(
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

        // api/task/all
        [HttpGet("all")]
        public async Task<List<TaskViewModel>> GetTasks()
        {
            var tasks = await _taskRepository.GetAllTasks();
            return await CreateTasksViewModelsAsync(tasks);
        }

        // api/task/new
        [HttpGet("new")]
        public async Task<List<TaskViewModel>> GetNewTasks()
        {
            var tasks = await _taskRepository.GetNewTasks();
            return await CreateTasksViewModelsAsync(tasks);
        }

        // api/task/customer?id=1
        [HttpGet("customer")]
        public async Task<List<TaskViewModel>> GetCustomerTasks(int id)
        {
            var tasks = await _taskRepository.GetCustomerTasks(id);
            return await CreateTasksViewModelsAsync(tasks);
        }

        // api/task/performer?id=1
        [HttpGet("performer")]
        public async Task<List<TaskViewModel>> GetPerformerTasks(int id)
        {
            var tasks = await _taskRepository.GetPerformerTasks(id);
            return await CreateTasksViewModelsAsync(tasks);
        }

        // api/task/my?id=1
        [Authorize]
        [HttpGet("my")]
        public async Task<List<TaskViewModel>> GetMyTasks()
        {
            var tasks = await _taskRepository.GetCustomerTasks(int.Parse(User.GetName()));
            return await CreateTasksViewModelsAsync(tasks);
        }

        // api/task/add
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
                newTask.Time = DateTime.Now;
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
                var task = await _taskRepository.GetTask(id);

                if (task.IdCustomer != int.Parse(User.GetName()))
                {
                    return BadRequest(new { message = "Попытка изменить не свою задачу" });
                }

                await _taskRepository.AddTagsToTask(task, tags);
            }
            catch (RepositoryException ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            return Ok();
        }

        // api/task/status?id=1
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
                task.IdStatus = (await _statusRepository.GetTaskStatus("status")).Id;
                await _taskRepository.UpdateTask(task);
            }
            catch (RepositoryException ex)
            {
                return BadRequest(new {message = ex.Message});
            }
            catch (NullReferenceException)
            {
                return BadRequest(new { message = "Такого статуса задачи не существует." });
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
