using System;
using System.Collections.Generic;
using System.Text;

namespace ChangeX.BLL.DTOs
{
    public class ChangeStatusDto
    {
        public Guid CRID { get; set; }
        public string TargetStatus { get; set; }
        public string ActorRole { get; set; } // until auth is added
    }
}
