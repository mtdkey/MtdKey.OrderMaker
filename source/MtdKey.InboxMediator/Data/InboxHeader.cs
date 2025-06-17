using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MtdKey.InboxMediator.Data
{
    [Table("inbox_headers")]
    [Index(nameof(EmailAsKey), nameof(UID), IsUnique = true)]
    public class InboxHeader
    {

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        [MaxLength(128)]
        public string EmailAsKey { get; set; } = string.Empty;
        [Required]
        public uint UID { get; set; } = 0;

        [ForeignKey(nameof(EmailAsKey))]
        public virtual EmailMediator? EmailMediator { get; set; }
    }
}
