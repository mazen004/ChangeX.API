using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChangeX.DAL.Entities
{
    public class Client
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string ContactInfo { get; set; } = string.Empty;
    }
}
