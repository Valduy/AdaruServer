using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AdaruServer.Helpers;
using AdaruServer.ViewModels;
using DBRepository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AdaruServer.Controllers
{
    [Route("api/[controller]")]
    public class AuthorizationController : Controller
    {
        private IClientRepository _clientRepository;

        public AuthorizationController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        // api/authorization/test
        [HttpPost("token")]
        public async Task<IActionResult> Token([FromBody] AuthorizationViewModel model)
        {
            var identity = await GetIdentity(model.Username, model.Password);

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
                username = identity.Name
            };

            return Ok(response);
        }

        private async Task<ClaimsIdentity> GetIdentity(string userName, string password)
        {
            ClaimsIdentity identity = null;

            var client = await _clientRepository.GetClient(userName);

            if (client != null)
            {
                // TODO: хеширование
                if (string.Equals(client.Password, password))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, client.Login)
                    };
                    identity = new ClaimsIdentity(claims, "Token", 
                        ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                }
            }

            return identity;
        }
    }
}
