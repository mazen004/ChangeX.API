using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChangeX.DAL.Entities
{
    public class User
    {
        [Key]
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsPrimaryContact { get; set; }
        //public bool IsActive { get; set; } = true; // softDelete
        //public DateTime? InActiveAt { get; set; }

        [ForeignKey(nameof(Client))]
        public Guid ClientID { get; set; }
        public Client Client { get; set; } = null!;
    }
}
