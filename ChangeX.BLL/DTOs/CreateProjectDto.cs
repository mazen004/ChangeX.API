using ChangeX.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChangeX.BLL.DTOs
{
    public class CreateProjectDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Scope { get; set; }
        public Guid ClientID { get; set; }
        public ProjectState State { get; set; }
    }
}
