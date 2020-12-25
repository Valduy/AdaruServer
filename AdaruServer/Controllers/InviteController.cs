using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdaruServer.Extensions;
using AdaruServer.ViewModels;
using AutoMapper;
using DBRepository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Npgsql;
using Task = System.Threading.Tasks.Task;

namespace AdaruServer.Controllers
{
    [Route("api/[controller]")]
    public class InviteController : Controller
    {
        private IInviteRepository _inviteRepository;
        private ITaskRepository _taskRepository;
        private IClientRepository _clientRepository;
        private IRoleRepository _roleRepository;
        private IMapper _mapper;

        public InviteController(
            IInviteRepository inviteRepository,
            ITaskRepository taskRepository,
            IClientRepository clientRepository,
            IRoleRepository roleRepository,
            IMapper mapper)
        {
            _inviteRepository = inviteRepository;
            _taskRepository = taskRepository;
            _clientRepository = clientRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        // api/invite/add?task=1&performer=1
        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddInvite(int task, int performer)
        {
            var userId = int.Parse(User.GetName());
            var userTask = await _taskRepository.GetTask(task);

            if (userTask.IdCustomer != userId)
            {
                return BadRequest(new {message = "Попытка приглашения не в свою задачу."});
            }

            var invite = new Invite
            {
                IdTask = task,
                IdPerformer = performer
            };

            await _inviteRepository.AddInvite(invite);

            return Ok();
        }

        // api/invite/get
        [Authorize]
        [HttpGet("get")]
        public async Task<List<InviteViewModel>> GetInvite()
        {
            var id = int.Parse(User.GetName());
            var user = await _clientRepository.GetClient(id);
            var role = await _roleRepository.GetUserRole(user.IdRole);

            switch (role.Role)
            {
                case "performer":
                {
                    return (await _inviteRepository.GetInvitesToPerformer(id))
                        .Select(i => _mapper.Map<InviteViewModel>(i))
                        .ToList();
                }
                case "customer":
                {
                    return (await _inviteRepository.GetCustomerInvites(id))
                        .Select(i => _mapper.Map<InviteViewModel>(i))
                        .ToList();
                    }
                default:
                    throw new ArgumentException();
            }
        }

        // api/invite/agree
        [Authorize]
        [HttpPost("agree")]
        public async Task<IActionResult> AgreeInvite(int id)
        {
            var userId = int.Parse(User.GetName());
            var invite = await _inviteRepository.GetInvite(id);

            if (invite == null)
            {
                return BadRequest(new {message="Попытка согласиться на задачу, для которой нет приглашения."});
            }

            if (invite.IdPerformer != userId)
            {
                return BadRequest(new { message = "Попытка согласиться на задачу для другого исполнителя." });
            }
            
            var task = await _taskRepository.GetTask(id);
            task.IdPerformer = userId;
            await _taskRepository.UpdateTask(task);
            await _inviteRepository.DeleteInvite(invite);
            return Ok();
        }

        // api/invite/reject
        [Authorize]
        [HttpDelete("reject")]
        public async Task<IActionResult> RejectInvite(int id)
        {
            var userId = int.Parse(User.GetName());
            var invite = await _inviteRepository.GetInvite(id);

            if (invite == null)
            {
                return BadRequest(new { message = "Попытка отказаться от задачи, для которой нет приглашения." });
            }

            if (invite.IdPerformer != userId)
            {
                return BadRequest(new { message = "Попытка отакзаться от задачи для другого исполнителя." });
            }

            await _inviteRepository.DeleteInvite(invite);
            return Ok();
        }

        // api/invite/delete
        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteInvite(int id)
        {
            var userId = int.Parse(User.GetName());
            var invite = await _inviteRepository.GetInvite(id);

            if (invite == null)
            {
                return BadRequest(new { message = "Попытка удалить не существующее приглашение." });
            }

            var task = await _taskRepository.GetTask(id);

            if (task.IdCustomer != userId)
            {
                return BadRequest(new {message = "Попытка удалить приглашения для не своей задачи."});
            }

            await _inviteRepository.DeleteInvite(invite);
            return Ok();
        }
    }
}
