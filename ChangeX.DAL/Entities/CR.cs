using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChangeX.DAL.Entities
{
    public class CR
    {
        [Key]
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public string Scope { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [ForeignKey(nameof(CurrentStatus))]
        public Guid CurrentStatusID { get; set; }

        public CRStatus CurrentStatus { get; set; } = null!;

        public decimal EstimatedManHour { get; set; }

        public decimal ManHourRate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime FinishDate { get; set; }

        [ForeignKey(nameof(Project))]
        public Guid ProjectID { get; set; }

        public Project Project { get; set; } = null!;

        [ForeignKey(nameof(User))]
        public Guid UserID { get; set; }

        public User User { get; set; } = null!;
    }
}
