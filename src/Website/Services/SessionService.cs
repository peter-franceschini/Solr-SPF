using Microsoft.AspNetCore.Http;
using Website.Models.Database;

namespace Website.Services
{
    public class SessionService : ISessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IUserService UserService { get; set; }

        public SessionService(IHttpContextAccessor httpContextAccessor, IUserService userService)
        {
            _httpContextAccessor = httpContextAccessor;
            UserService = userService;
        }

        public AspNetUsers GetUser()
        {
            if (IsAuthenticated())
            {
                return UserService.GetUser(_httpContextAccessor.HttpContext.User.Identity.Name);
            }
            else
            {
                return null;
            }
        }

        public bool IsAuthenticated()
        {
            return _httpContextAccessor.HttpContext.User != null
                && _httpContextAccessor.HttpContext.User.Identity != null
                && _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public string GetUserId()
        {
            if (GetUser() != null)
            {
                return GetUser().Id;
            }

            return null;
        }
    }
}
