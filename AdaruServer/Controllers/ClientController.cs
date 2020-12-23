using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AdaruServer.Helpers;
using AdaruServer.ViewModels;
using AutoMapper;
using DBRepository;
using DBRepository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models;

namespace AdaruServer.Controllers
{
    [Route("api/[controller]")]
    public class ClientController : Controller
    {
        private IClientRepository _clientRepository;
        private IRoleRepository _roleRepository;
        private IMapper _mapper;

        public ClientController(
            IClientRepository clientRepository, 
            IRoleRepository roleRepository, 
            IMapper mapper)
        {
            _clientRepository = clientRepository;
            _roleRepository = roleRepository;
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
        public async Task<List<ClientInfoViewModel>> GetPerformers()
        {
            var performers = await _clientRepository.GetPerformers();
            return performers.Select(p => _mapper.Map<ClientInfoViewModel>(p)).ToList();
        }

        // api/client/performers/tags
        [HttpPost("performers/tags")]
        public async Task<List<ClientInfoViewModel>> GetCustomers([FromBody]IEnumerable<string> tags)
        {
            var performers = await _clientRepository.GetPerformers(tags);
            return performers.Select(p => _mapper.Map<ClientInfoViewModel>(p)).ToList();
        }

        // api/client/customers
        [HttpGet("customers")]
        public async Task<List<ClientInfoViewModel>> GetCustomers()
        {
            var performers = await _clientRepository.GetCustomers();
            return performers.Select(p => _mapper.Map<ClientInfoViewModel>(p)).ToList();
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
    }
}
