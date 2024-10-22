using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MtdKey.OrderMaker.Services.FileStorage
{
    [Table("file_storage_items")]
    [Index(nameof(Created))]
    public class FileStorageItem
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        [MaxLength(256)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public long Size { get; set; } = long.MaxValue;
        [Required]
        [MaxLength(256)]
        public string Mime { get; set; } = string.Empty;
        [Required]
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
