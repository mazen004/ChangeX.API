using ChangeX.DAL.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChangeX.DAL.Entities
{
    public class Project
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Scope { get; set; }
        public Guid ClientID { get; set; }
        public Client Client { get; set; }
        public ProjectState State { get; set; }
    }
}
