using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChangeX.DAL.Entities
{
    public class Detail
    {
        [Key]
        public Guid ID { get; set; }

        [ForeignKey(nameof(CR))]
        public Guid CRID { get; set; }

        public CR CR { get; set; } = null!;

        public string Attachment { get; set; } = string.Empty;

        public string Comment { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;

        public DateTime UploadedTime { get; set; }
    }
}
