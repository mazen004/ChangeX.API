using System;
using System.Collections.Generic;
using System.Text;

namespace ChangeX.BLL.DTOs
{
    public class RequestCRDto
    {
        public string Name { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public string Scope { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid ProjectID { get; set; }
    }
}
