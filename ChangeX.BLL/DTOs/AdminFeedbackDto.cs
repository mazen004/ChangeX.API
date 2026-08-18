namespace ChangeX.BLL.DTOs
{
    public class AdminFeedbackDto
    {
        public string Decision { get; set; } = string.Empty;
        public EstimateCRDto? Estimate { get; set; }
        public decimal? InvoiceCost { get; set; }
        public string? Message { get; set; }
    }
}
