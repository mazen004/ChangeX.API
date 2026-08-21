using System;
using System.Collections.Generic;
using System.Text;

namespace ChangeX.BLL.DTOs.Users
{
    public class UserInClientDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool SystemRole { get; set; } = false;
    }
}
