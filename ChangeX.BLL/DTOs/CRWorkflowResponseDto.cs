namespace ChangeX.BLL.DTOs
{
    public class CRWorkflowResponseDto
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public string Scope { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal EstimatedManHour { get; set; }
        public decimal ManHourRate { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly FinishDate { get; set; }
        public Guid ProjectID { get; set; }
        public string CurrentStatus { get; set; } = string.Empty;
        public List<string> AvailableStatuses { get; set; } = [];
        public InvoiceWorkflowDto? Invoice { get; set; }
        public List<DetailWorkflowDto> Details { get; set; } = [];
    }

    public class InvoiceWorkflowDto
    {
        public Guid ID { get; set; }
        public decimal Cost { get; set; }
        public DateTime CreatedTime { get; set; }
        public string State { get; set; } = string.Empty;
    }

    public class DetailWorkflowDto
    {
        public Guid ID { get; set; }
        public string Attachment { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public DateTime UploadedTime { get; set; }
    }
}
