using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MtdKey.InboxMediator.Data
{
    [Table("loader_flags")]
    public class LoaderFlag
    {
        [Key]
        public string EmailAsKey {  get; set; } = string.Empty;
        [Required]
        public bool Complete { get; set; }

        [ForeignKey(nameof(EmailAsKey))]
        public virtual EmailMediator? EmailMediator { get; set; }
    }
}
