using Microsoft.AspNetCore.Http;

namespace ChangeX.BLL.DTOs
{
    public class DetailDto
    {
        public Guid CRID { get; set; }
        public IFormFile? Attachment { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
