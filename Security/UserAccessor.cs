using System.Security.Claims;
using Application.Core;
using Microsoft.AspNetCore.Http;

namespace Security
{
    public class UserAccessor : IUserAccessor
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UserAccessor(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        
        public string GetUserEmail()
        {
            return _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
        }
    }
}