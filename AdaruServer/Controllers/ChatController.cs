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
        private IMapper _mapper;

        public ChatController(
            IChatRepository chatRepository,
            IMessageRepository messageRepository,
            IClientRepository clientRepository,
            IMapper mapper)
        {
            _chatRepository = chatRepository;
            _messageRepository = messageRepository;
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        // api/chat/chats
        [Authorize]
        [HttpGet("chats")]
        public async Task<List<ClientViewModel>> GetChats()
        {
            var chats = await _chatRepository.GetChats(int.Parse(User.GetName()));
            var result = new List<ClientViewModel>();

            foreach (var c in chats)
            {
                var client = await _clientRepository.GetClient(c.IdTarget);
                result.Add(_mapper.Map<ClientViewModel>(client));
            }

            return result;
        }

        // api/chat/client
        [Authorize]
        [HttpGet("client")]
        public async Task<ChatViewModel> GetChat(int id)
        {
            var chat = await _chatRepository.GetChat(int.Parse(User.GetName()), id);
            if (chat == null) return null;
            var result = _mapper.Map<ChatViewModel>(chat);
            var client = await _clientRepository.GetClient(chat.IdTarget);
            result.Target = _mapper.Map<ClientViewModel>(client);
            result.Messages =
                (await _messageRepository.GetMessages(chat.Id)).Select(m => _mapper.Map<MessageViewModel>(m));
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
