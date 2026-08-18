using System;
using System.Collections.Generic;
using System.Text;

namespace ChangeX.BLL.DTOs
{
    public class ChangeStatusDto
    {
        public Guid CRID { get; set; }
        public string TargetStatus { get; set; } = string.Empty;
        public string ActorRole { get; set; } = string.Empty; // until auth is added
    }
}
