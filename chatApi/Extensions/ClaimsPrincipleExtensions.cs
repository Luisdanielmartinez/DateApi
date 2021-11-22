using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace chatApi.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetUserName(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
        public static int GetUserId(this ClaimsPrincipal user)                 // second extension method added here to GetUserId
        {

            return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);  // need to return an integer here so we wrap the return in int.Parse()
        }
    }
}
