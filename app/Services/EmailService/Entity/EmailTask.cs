using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MtdKey.OrderMaker.Services.EmailService.Entity
{
    [Table("email_task")]
    [Index(nameof(Created), Name = "IX_CREATED")]
    public class EmailTask
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [StringLength(320)]
        public string Recepient { get; set; } = string.Empty;
        [Required]
        [StringLength(128)]
        public string Subject { get; set; } = string.Empty;
        [Required]
        public int Template { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string Attributes { get; set; }
        [Required]
        public DateTime Created { get; set; } = DateTime.Now;
        [Required]
        public bool Complete { get; set; }
        [Required]
        public bool DeliveryStatus { get; set; }

    }
}
