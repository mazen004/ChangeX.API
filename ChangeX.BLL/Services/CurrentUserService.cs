using ChangeX.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace ChangeX.BLL.Services
{
    public class CurrentUserService(IHttpContextAccessor httpContextAccessor)
        : ICurrentUserService
    {
        private ClaimsPrincipal User =>
            httpContextAccessor.HttpContext?.User
            ?? new ClaimsPrincipal();

        public Guid? UserId
        {
            get
            {
                var value = User.FindFirstValue("UserID");

                return Guid.TryParse(value, out var id)
                    ? id
                    : null;
            }
        }~

        public Guid? ClientId
        {
            get
            {
                var value = User.FindFirstValue("ClientID");

                return Guid.TryParse(value, out var id)
                    ? id
                    : null;
            }
        }

        public string? Name =>
            User.FindFirstValue(ClaimTypes.Name);

        public string? Email =>
            User.FindFirstValue(ClaimTypes.Email);

        public string? PhoneNumber =>
            User.FindFirstValue("PhoneNumber");

        public string? Role =>
            User.FindFirstValue(ClaimTypes.Role);

        public bool IsAuthenticated =>
            User.Identity?.IsAuthenticated == true;

        public bool IsAdmin =>
            User.IsInRole("Admin");
    }
}