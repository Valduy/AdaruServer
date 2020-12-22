using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdaruServer.Extensions;
using AdaruServer.ViewModels;
using AutoMapper;
using DBRepository;
using DBRepository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Task = System.Threading.Tasks.Task;

namespace AdaruServer.Controllers
{
    [Route("api/[controller]")]
    public class ProfileController : Controller
    {
        private IProfileRepository _profileRepository;
        private IMapper _mapper;

        public ProfileController(
            IProfileRepository profileRepository,
            IMapper mapper)
        {
            _profileRepository = profileRepository;
            _mapper = mapper;
        }

        // api/profile/add
        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddProfile([FromBody] string resume)
        {
            try
            {
                var userId = User.GetName();
                var profile = _mapper.Map<Models.Profile>(resume);
                profile.IdClient = int.Parse(userId);
                await _profileRepository.AddProfile(profile);
            }
            catch (RepositoryException ex)
            {
                return BadRequest(new {message = ex.Message});
            }

            return Ok();
        }

        // api/profile/get
        [Authorize]
        [HttpGet("get")]
        public async Task<ProfileViewModel> GetProfile()
        {
            var userId = User.GetName();
            var profile = await _profileRepository.GetProfile(int.Parse(userId));
            return _mapper.Map<ProfileViewModel>(profile);
        }
    }
}
