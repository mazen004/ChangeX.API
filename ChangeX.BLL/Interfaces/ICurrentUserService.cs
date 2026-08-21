using System;
using System.Collections.Generic;
using System.Text;

namespace ChangeX.BLL.Interfaces
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
        Guid? ClientId { get; }
        string? Name { get; }
        string? Email { get; }
        string? Role { get; }

        bool IsAuthenticated { get; }
        bool IsAdmin { get; }
    }
}
