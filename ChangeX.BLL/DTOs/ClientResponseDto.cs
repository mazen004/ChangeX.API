using ChangeX.BLL.DTOs.Users;
using ChangeX.DAL.Entities;

namespace ChangeX.BLL.DTOs
{
    public class ClientResponseDto
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string ContactInfo { get; set; } = string.Empty;
        public Guid? DefaultContactID { get; set; }
        public User? DefaultContact { get; set; }
    }
}
