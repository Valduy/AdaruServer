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
        private ITaskInfoRepository _taskInfoRepository;
        private IClientRepository _clientRepository;
        private IRoleRepository _roleRepository;
        private IStatusRepository _statusRepository;
        private IMapper _mapper;

        public TaskController(
            ITaskRepository taskRepository, 
            ITaskInfoRepository taskInfoRepository,
            IClientRepository clientRepository,
            IRoleRepository roleRepository,
            IStatusRepository statusRepository,
            IMapper mapper)
        {
            _taskRepository = taskRepository;
            _taskInfoRepository = taskInfoRepository;
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

        // api/task/tags
        [HttpPost("tags")]
        public async Task<List<TaskViewModel>> GetTasksByTags([FromBody]IEnumerable<string> tags)
        {
            var tasks = await _taskInfoRepository.GetTasksByTags(tags);
            var result = new List<TaskViewModel>();

            foreach (var t in tasks)
            {
                var model = _mapper.Map<TaskViewModel>(t);
                model.Tags = (await _taskRepository.GetTaskTags(t.IdTask.Value)).Select(tag => tag.Name).ToList();
                model.Customer = _mapper.Map<ClientViewModel>(t);
                model.Customer.Role = "customer";
                result.Add(model);
            }

            return result;
        }

        // api/task/my
        [Authorize]
        [HttpGet("my")]
        public async Task<List<TaskViewModel>> GetMyTasks()
        {
            var id = int.Parse(User.GetName());
            var user = await _clientRepository.GetClient(id);
            var role = await _roleRepository.GetUserRole(user.IdRole);

            switch (role.Role)
            {
                case "performer":
                {
                    var tasks = await _taskRepository.GetPerformerTasks(id);
                    return await CreateTasksViewModelsAsync(tasks);
                }
                case "customer":
                {
                    var tasks = await _taskRepository.GetCustomerTasks(id);
                    return await CreateTasksViewModelsAsync(tasks);
                }   
                default:
                    throw new ArgumentException();
            }
        }

        // api/task/concrete?id=1
        [HttpGet("concrete")]
        public async Task<TaskInfoViewModel> GetTask(int id)
        {
            var task = await _taskRepository.GetTask(id);
            var customer = await _clientRepository.GetClient(task.IdCustomer);
            var model = _mapper.Map<TaskInfoViewModel>(task);
            model.Tags = (await _taskRepository.GetTaskTags(task.Id)).Select(tag => tag.Name).ToList();
            model.Status = (await _statusRepository.GetTaskStatus(task.IdStatus)).Status;
            model.Customer = _mapper.Map<ClientViewModel>(customer);
            model.Customer.Role = (await _roleRepository.GetUserRole(customer.IdRole)).Role;

            if (task.IdPerformer.HasValue)
            {
                var performer = await _clientRepository.GetClient(task.IdPerformer.Value);
                model.Performer = _mapper.Map<ClientViewModel>(performer);
                model.Performer.Role = (await _roleRepository.GetUserRole(performer.IdRole)).Role;
            }

            return model;
        }
        
        // api/task/add
        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddTask([FromBody]AddTaskViewModel task)
        {
            try
            {
                var newTask = _mapper.Map<Models.Task>(task);
                newTask.IdCustomer = int.Parse(User.GetName());
                newTask.IdStatus = (await _statusRepository.GetTaskStatus("new")).Id;
                newTask.Time = DateTime.Now;
                await _taskRepository.AddTask(newTask);
                await _taskRepository.AddTagsToTask(newTask, task.Tags);
            }
            catch (RepositoryException ex)
            {
                return BadRequest(new {message = ex.Message});
            }

            return Ok();
        }

        // api/task/tags?id=1
        [Authorize]
        [HttpPost("add/tags")]
        public async Task<IActionResult> AddTagsToTask(int id, [FromBody]IEnumerable<string> tags)
        {
            try
            {
                var task = await _taskRepository.GetTask(id);

                if (task.IdCustomer != int.Parse(User.GetName()))
                {
                    return BadRequest(new { message = "Попытка изменить не свою задачу." });
                }

                await _taskRepository.AddTagsToTask(task, tags);
            }
            catch (RepositoryException ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            return Ok();
        }

        // api/task/delete/tags?id=1
        [Authorize]
        [HttpDelete("delete/tags")]
        public async Task<IActionResult> DeleteTaskTags(int id, [FromBody] IEnumerable<string> tags)
        {
            var task = await _taskRepository.GetTask(id);

            if (task.IdCustomer != int.Parse(User.GetName()))
            {
                return BadRequest(new { message = "Попытка изменить не свою задачу." });
            }

            await _taskRepository.DeleteTaskTags(task, tags);
            return Ok();
        }

        // api/task/status?id=1
        [Authorize]
        [HttpPost("status")]
        public async Task<IActionResult> ChangeTaskStatus(int id, [FromBody]string status)
        {
            var task = await _taskRepository.GetTask(id);

            if (task == null)
            {
                return BadRequest(new {message="Такой задачи нет."});
            }
            if (task.IdCustomer != int.Parse(User.GetName()))
            {
                return BadRequest(new {message="Попытка изменить не свою задачу"});
            }

            try
            {
                task.IdStatus = (await _statusRepository.GetTaskStatus(status)).Id;
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
