using System;

namespace ChangeX.BLL.DTOs.Users
{
    public class UpdateUserDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        // public string Password { get; set; } = string.Empty;
        public string SystemRole { get; set; } = string.Empty;
        public bool IsPrimaryContact { get; set; } = false;
        public Guid ClientID { get; set; } = Guid.Empty;
    }
}