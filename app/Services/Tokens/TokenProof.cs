using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MtdKey.OrderMaker.Services.Tokens
{
    [Table("token_proofs")]
    [Index(nameof(Created))]
    public class TokenProof
    {
        [Key]
        [MaxLength(128)]
        public string Id { get; set; }        
        [MaxLength(128)]
        [Required]
        public string SecretKey { get; set; }
        [Required]
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
