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
        }

        public Guid? ClientId
        {
            get
            {
                var value = User.FindFirstValue("ClientId");

                return Guid.TryParse(value, out var id)
                    ? id
                    : null;
            }
        }

        public string? Name =>
            User.FindFirstValue("Name");

        public string? Email =>
            User.FindFirstValue("Email");

        public string? PhoneNumber =>
            User.FindFirstValue("PhoneNumber");

        public string? Role =>
            User.FindFirstValue("Role");

        public bool IsAuthenticated =>
            User.Identity?.IsAuthenticated == true;

        public bool IsAdmin =>
            User.IsInRole("Admin");
    }
}