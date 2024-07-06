using System.ComponentModel.DataAnnotations;

namespace MtdKey.OrderMaker.Services.EmailService
{
    public class EmailTask
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [StringLength(320)]
        public string Recepient { get; set; }
        [Required]
        [MaxLength(128)]
        public string Template { get; set; }
        [Required]
        [MaxLength(36)]
        public string DocId { get; set; }
    }
}
