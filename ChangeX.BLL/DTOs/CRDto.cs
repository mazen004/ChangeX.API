namespace ChangeX.BLL.DTOs
{
    public class CRDto
    {
        public string Name { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public string Scope { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal EstimatedManHour { get; set; }
        public decimal ManHourRate { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly FinishDate { get; set; }
        public Guid ProjectID { get; set; }
    }
}
