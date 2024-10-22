using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MtdKey.InboxMediator.Data
{
    [Table("email_mediators")]
    public class EmailMediator
    {
        public EmailMediator()
        {
            InboxHeaders = [];
            LoaderFlags = [];
            ReaderFlags = [];
        }

        [Key]        
        public string EmailAsKey { get; set; } = string.Empty;
        [Required]
        [MaxLength(256)]
        public string Password { get; set; } = string.Empty;
        [Required]
        [MaxLength(256)]
        public string Server { get; set; } = string.Empty;
        [Required]
        public int Port { get; set; } = 0;
        [Required]
        public bool UseSSL { get; set; } = true;
        [Required]
        public bool Active { get; set; } = false;

        public virtual ICollection<InboxHeader> InboxHeaders { get; set; }
        public virtual ICollection<LoaderFlag> LoaderFlags { get; set; }
        public virtual ICollection<ReaderFlag> ReaderFlags { get; set; }

    }
}
