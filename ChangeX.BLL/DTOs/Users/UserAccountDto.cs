using ChangeX.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChangeX.BLL.DTOs.Users
{
    public class UserAccountDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string SystemRole { get; set; }
        public Guid ClientID { get; set; }
        public string ClientName { get; set; }
    }
}
