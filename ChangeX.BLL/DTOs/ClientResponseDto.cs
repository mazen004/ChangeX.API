using ChangeX.BLL.DTOs.Users;

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
        public UserInClientDto? DefaultContact { get; set; }
    }
}
