using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChangeX.DAL.Entities
{
    public class CRStatus
    {
        [Key]
        public Guid ID { get; set; }

        public string CurrentStatus { get; set; } = string.Empty;

        public string AvailableStatuses { get; set; } = string.Empty;

        public string AccessedBy { get; set; } = string.Empty;
    }
}
