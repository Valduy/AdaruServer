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

namespace AdaruServer.Controllers
{
    [Route("api/[controller]")]
    public class ChatController : Controller
    {
        private IChatRepository _chatRepository;
        private IClientRepository _clientRepository;
        private IMapper _mapper;

        public ChatController(
            IChatRepository chatRepository,
            IClientRepository clientRepository,
            IMapper mapper)
        {
            _chatRepository = chatRepository;
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
    }
}
