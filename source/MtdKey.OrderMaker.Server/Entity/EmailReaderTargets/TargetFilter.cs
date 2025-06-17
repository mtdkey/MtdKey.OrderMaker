using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MtdKey.OrderMaker.Entity
{
    [Table("target_filters")]
    public class TargetFilter
    {
        [Key]
        public long Id { get; set; }        
        [Required]
        [MaxLength(36)]
        public string FormId { get; set; }
        [Required]
        [MaxLength(128)]
        public string EmailAsKey { get; set; } = string.Empty;
        [MaxLength(128)]
        public string Filter { get; set; }

        [ForeignKey(nameof(FormId))]
        public virtual TargetForm TargetForm { get; set; }
    }
}
