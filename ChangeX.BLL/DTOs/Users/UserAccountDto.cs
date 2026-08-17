using ChangeX.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChangeX.BLL.DTOs.Users
{
    public class UserAccountDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SystemRole { get; set; } = string.Empty;
        public Guid ClientID { get; set; }
        public string ClientName { get; set; } = string.Empty;
    }
}
