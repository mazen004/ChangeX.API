using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChangeX.DAL.Entities
{
    public class Invoice
    {
        [Key]
        public Guid ID { get; set; }

        [ForeignKey(nameof(CR))]
        public Guid CRID { get; set; }

        public CR CR { get; set; } = null!;

        public decimal Cost { get; set; }

        public DateTime CreatedTime { get; set; }

        public string State { get; set; } = string.Empty;
    }
}
