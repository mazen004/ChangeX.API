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
        public CRStatus CurrentStatus { get; set; } = null!;
        public decimal EstimatedManHour { get; set; }
        public decimal ManHourRate { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly FinishDate { get; set; }
<<<<<<< HEAD

        [ForeignKey(nameof(Detail))]
        public Guid DetailsID { get; set; }

=======
>>>>>>> 2c2d5eb8291f6a8d0dea5dd44762baeb5b121e48
        [ForeignKey(nameof(CurrentStatus))]
        public Guid CurrentStatusID { get; set; }

        [ForeignKey(nameof(Project))]
        public Guid ProjectID { get; set; }
        public Project Project { get; set; } = null!;
    }
}
