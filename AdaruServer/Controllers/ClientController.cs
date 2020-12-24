using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using AdaruServer.Extensions;
using AdaruServer.Helpers;
using AdaruServer.Services.Implementation;
using AdaruServer.ViewModels;
using AutoMapper;
using DBRepository;
using DBRepository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models;
using AuthorizationOptions = AdaruServer.Helpers.AuthorizationOptions;

namespace AdaruServer.Controllers
{
    [Route("api/[controller]")]
    public class ClientController : Controller
    {
        private IWebHostEnvironment _environment;
        private IClientRepository _clientRepository;
        private IPerformerRepository _performerRepository;
        private ICustomerRepository _customerRepository;
        private IRoleRepository _roleRepository;
        private IImageRepository _imageRepository;
        private IImageService _imageService;
        private IMapper _mapper;

        public ClientController(
            IWebHostEnvironment environment,
            IClientRepository clientRepository, 
            IPerformerRepository performerRepository,
            ICustomerRepository customerRepository,
            IRoleRepository roleRepository, 
            IImageRepository imageRepository,
            IImageService imageService,
            IMapper mapper)
        {
            _environment = environment;
            _clientRepository = clientRepository;
            _performerRepository = performerRepository;
            _customerRepository = customerRepository;
            _roleRepository = roleRepository;
            _imageRepository = imageRepository;
            _imageService = imageService;
            _mapper = mapper;
        }

        // api/client/token
        [HttpPost("token")]
        public async Task<IActionResult> Token([FromBody] AuthorizationViewModel model)
        {
            var identity = await GetIdentity(model.Login, model.Password);

            if (identity == null)
            {
                return Unauthorized();
            }

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: AuthorizationOptions.ISSUER,
                audience: AuthorizationOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthorizationOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthorizationOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                id = identity.Name
            };

            return Ok(response);
        }

        // api/client/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegistrationViewModel model)
        {
            try
            {
                var client = _mapper.Map<Client>(model);
                client.IdRole = (await _roleRepository.GetUserRole(model.Role)).Id;
                await _clientRepository.AddClient(client);
                return Ok();
            }
            catch (RepositoryException ex)
            {
                return BadRequest(new {message = ex.Message});
            }
            catch (NullReferenceException)
            {
                return BadRequest(new {message = "Выбрана не существующая роль."});
            }
        }

        // api/client/performers
        [HttpGet("performers")]
        public async Task<List<PerformerInfoViewModel>> GetPerformers()
        {
            var performers = await _performerRepository.GetPerformers();
            return await CreatePerformerViewModelsAsync(performers);
        }

        // api/client/performers/tags
        [HttpPost("performers/tags")]
        public async Task<List<PerformerInfoViewModel>> GetPerformers([FromBody]IEnumerable<string> tags)
        {
            var performers = await _performerRepository.GetPerformers(tags);
            return await CreatePerformerViewModelsAsync(performers);
        }

        // api/client/performer/concrete
        [HttpGet("performer/concrete")]
        public async Task<PerformerInfoViewModel> GetPerformer(int id)
        {
            var performer = await _performerRepository.GetPerformer(id);
            return await CreatePerformerInfoViewModelAsync(performer);
        }

        // api/client/customers
        [HttpGet("customers")]
        public async Task<List<ClientInfoViewModel>> GetCustomers()
        {
            var performers = await _customerRepository.GetCustomers();
            return performers.Select(p => _mapper.Map<ClientInfoViewModel>(p)).ToList();
        }

        // api/client/customers/tags
        [HttpPost("customers/tags")]
        public async Task<List<ClientInfoViewModel>> GetCustomers([FromBody]IEnumerable<string> tags)
        {
            var performers = await _customerRepository.GetCustomers(tags);
            return performers.Select(p => _mapper.Map<ClientInfoViewModel>(p)).ToList();
        }

        // api/client/customers/concrete
        [HttpGet("customers/concrete")]
        public async Task<ClientInfoViewModel> GetCustomer(int id)
        {
            var performer = await _customerRepository.GetCustomer(id);
            return _mapper.Map<ClientInfoViewModel>(performer);
        }

        [Authorize]
        [HttpGet("client/me")]
        public async Task<ClientInfoViewModel> GetCustomer()
        {
            var id = int.Parse(User.GetName());
            var user = await _clientRepository.GetClient(id);
            var role = await _roleRepository.GetUserRole(user.IdRole);

            switch (role.Role)
            {
                case "performer":
                {
                    var performer = await _performerRepository.GetPerformer(id);
                    return await CreatePerformerInfoViewModelAsync(performer);
                }
                case "customer":
                {
                    var customer = await _customerRepository.GetCustomer(id);
                    return _mapper.Map<ClientInfoViewModel>(customer);
                }
                default:
                    throw new ArgumentException();
            }
        }
        
        // api/client/add/tags
        [Authorize]
        [HttpPost("add/tags")]
        public async Task<IActionResult> AddTagsToPerformer([FromBody]IEnumerable<string> tags)
        {
            try
            {
                var performer = await _performerRepository.GetPerformer(int.Parse(User.GetName()));
                await _performerRepository.AddTagsToPerformer(performer, tags);
            }
            catch (RepositoryException ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            return Ok();
        }

        // api/client/delete/tags
        [Authorize]
        [HttpDelete("delete/tags")]
        public async System.Threading.Tasks.Task DeletePerformerTags([FromBody] IEnumerable<string> tags)
        {
            var performer = await _performerRepository.GetPerformer(int.Parse(User.GetName()));
            await _performerRepository.DeletePerformerTags(performer, tags);
        }

        // api/client/update/avatar
        [Authorize]
        [HttpPost("update/avatar")]
        public async System.Threading.Tasks.Task UpdateAvatar([FromBody]string image)
        {
            var client = await _clientRepository.GetClient(int.Parse(User.GetName()));
            var newImage = await _imageService.AddImageAsync(client.Login, image);

            if (client.IdImage.HasValue)
            {
                var oldImage = await _imageRepository.GetImage(client.IdImage.Value);
                client.IdImage = newImage.Id;
                await _clientRepository.UpdateClient(client);
                await _imageRepository.DeleteImage(oldImage.Id);
                System.IO.File.Delete(oldImage.Path);
            }
            else
            {
                client.IdImage = newImage.Id;
                await _clientRepository.UpdateClient(client);
            }
        }

        // api/client/get/avatar
        [HttpGet("get/avatar")]
        public async Task<string> GetAvatar(int id)
        {
            var image = await _imageRepository.GetImage(id);
            return await _imageService.GetImage(image);
        }

        private async Task<ClaimsIdentity> GetIdentity(string login, string password)
        {
            ClaimsIdentity identity = null;

            var client = await _clientRepository.GetClient(login);

            if (client != null)
            {
                // TODO: хеширование
                if (string.Equals(client.Password, password))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, client.Id.ToString())
                    };
                    identity = new ClaimsIdentity(claims, "Token",
                        ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                }
            }

            return identity;
        }

        private async Task<List<PerformerInfoViewModel>> CreatePerformerViewModelsAsync(List<PerformerInfo> performers)
        {
            var result = new List<PerformerInfoViewModel>();

            foreach (var p in performers)
            {
                 result.Add(await CreatePerformerInfoViewModelAsync(p));
            }

            return result;
        }

        private async Task<PerformerInfoViewModel> CreatePerformerInfoViewModelAsync(PerformerInfo performer)
        {
            var model = _mapper.Map<PerformerInfoViewModel>(performer);
            model.Tags = (await _performerRepository.GetPerformerTags(model.Id)).Select(t => t.Name);
            return model;
        }
    }
}
