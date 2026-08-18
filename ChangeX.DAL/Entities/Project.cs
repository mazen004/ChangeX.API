using ChangeX.DAL.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChangeX.DAL.Entities
{
    public class Project
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Scope { get; set; } = string.Empty;
        public Guid ClientID { get; set; }
        public Client Client { get; set; } = null!;
        public ProjectState State { get; set; }
    }
}
