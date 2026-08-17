namespace ChangeX.BLL.DTOs
{
    public class CRStatusDto
    {
        public string CurrentStatus { get; set; } = string.Empty;
        public string AvailableStatuses { get; set; } = string.Empty;
        public string AccessedBy { get; set; } = string.Empty;
    }
}
