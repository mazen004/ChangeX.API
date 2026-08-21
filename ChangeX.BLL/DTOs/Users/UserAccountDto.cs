using ChangeX.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChangeX.BLL.DTOs.Users
{
    public class UserAccountDto
    {
        public Guid ID { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool SystemRole { get; set; } = false;
        public Guid ClientID { get; set; } = Guid.Empty;
        public string ClientName { get; set; } = string.Empty;
    }
}
