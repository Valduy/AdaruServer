﻿using System;
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
using Task = System.Threading.Tasks.Task;

namespace AdaruServer.Controllers
{
    [Route("api/[controller]")]
    public class ChatController : Controller
    {
        private IChatRepository _chatRepository;
        private IMessageRepository _messageRepository;
        private IClientRepository _clientRepository;
        private IRoleRepository _roleRepository;
        private IMapper _mapper;

        public ChatController(
            IChatRepository chatRepository,
            IMessageRepository messageRepository,
            IClientRepository clientRepository,
            IRoleRepository roleRepository,
            IMapper mapper)
        {
            _chatRepository = chatRepository;
            _messageRepository = messageRepository;
            _clientRepository = clientRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        // api/chat/chats
        [Authorize]
        [HttpGet("chats")]
        public async Task<List<ClientViewModel>> GetChats()
        {
            var id = int.Parse(User.GetName());
            var chats = await _chatRepository.GetChats(id);
            var result = new List<ClientViewModel>();

            foreach (var c in chats)
            {
                var client = await _clientRepository.GetClient(c.IdTarget != id ? c.IdTarget : c.IdSource);
                var model = _mapper.Map<ClientViewModel>(client);
                model.Role = (await _roleRepository.GetUserRole(client.IdRole)).Role;
                result.Add(model);
            }

            return result;
        }

        // api/chat/client?id=1
        [Authorize]
        [HttpGet("client")]
        public async Task<ChatViewModel> GetChat(int id)
        {
            var chat = await _chatRepository.GetChat(int.Parse(User.GetName()), id);
            if (chat == null) return null;
            var result = _mapper.Map<ChatViewModel>(chat);
            var client = await _clientRepository.GetClient(chat.IdTarget == id ? chat.IdSource : id);
            result.Target = _mapper.Map<ClientViewModel>(client);
            result.Target.Role = (await _roleRepository.GetUserRole(client.IdRole)).Role;
            result.Messages = (await _messageRepository.GetMessages(chat.Id))
                .Select(m =>
                {
                    var message = _mapper.Map<MessageViewModel>(m);
                    message.Username = client.Username;
                    return message;
                });
            return result;
        }

        // api/chat/add
        [Authorize]
        [HttpPost("add")]
        public async Task AddMessage(int id, [FromBody]AddMessageViewModel model)
        {
            var userId = int.Parse(User.GetName());
            var chat = await _chatRepository.GetChat(userId, id);
            var message = _mapper.Map<Message>(model);

            if (chat == null)
            {
                chat = new Chat
                {
                    IdSource = userId,
                    IdTarget = id
                };
                await _chatRepository.AddChat(chat);
            }

            message.IdSender = userId;
            message.IdChat = chat.Id;
            message.Time = DateTime.Now;
            await _messageRepository.AddMessage(message);
        }
    }
}
