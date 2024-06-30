using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MtdKey.OrderMaker.Entity
{
    public enum RenderTarget
    {
        From = 0,
        Subject = 1,
        Body = 2,
        Attachments = 3,
    }

    [Table("target_fields")]    
    public class TargetField
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [MaxLength(36)]
        public string FormId { get; set; }
        [Required]
        public int Target { get; set; } = (int)RenderTarget.Subject;
        [Required]
        [MaxLength(36)]
        public string FieldId { get; set; }

        [ForeignKey(nameof(FormId))]
        public virtual TargetForm TargetForm { get; set; }

        [ForeignKey(nameof(FieldId))]
        public virtual MtdFormPartField MtdFormPartField { get; set; }
    }
}
