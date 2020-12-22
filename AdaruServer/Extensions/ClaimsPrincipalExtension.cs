using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AdaruServer.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        public static string GetName(this ClaimsPrincipal principal) 
            => ((ClaimsIdentity)principal.Identity).FindFirst(ClaimTypes.Name)?.Value;
    }
}
