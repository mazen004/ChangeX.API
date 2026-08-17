using System;
using System.Collections.Generic;
using System.Text;

namespace ChangeX.BLL.DTOs
{
    public class ClientDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string ContactInfo { get; set; }
    }
}
