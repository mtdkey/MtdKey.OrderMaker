using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MtdKey.InboxMediator.Data
{
    [Table("reader_flags")]
    [Index(nameof(EmailAsKey), nameof(ReaderId), IsUnique = true)]
    public class ReaderFlag
    {
        [Key]
        public long Id { get; set; }
        public string EmailAsKey { get; set; } = string.Empty;
        [Required]
        public Guid ReaderId { get; set; }
        [Required]
        public uint MaxUID { get; set; } = 0;

        [ForeignKey(nameof(EmailAsKey))]
        public virtual EmailMediator? EmailMediator { get; set; }
    }
}
