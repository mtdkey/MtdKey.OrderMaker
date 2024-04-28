using Microsoft.EntityFrameworkCore;
using MtdKey.OrderMaker.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MtdKey.OrderMaker.Entity
{
    [Table("mtd_store_item")]
    [Index(nameof(Result), Name = "IX_TEXT_RESULT")]
    public class MtdStoreItem : IStoreField
    {
        [Key]
        public long Id { get; set; }
        [MaxLength(36)]
        [Required]
        public string StoreId { get; set; }
        [MaxLength(36)]
        [Required]
        public string FieldId { get; set; }
        [Required]
        public Guid ItemId { get; set; }
        [MaxLength(250)]
        [Required]
        public string Result { get; set; }
        [Required]
        public bool IsDeleted { get; set; } = false;

        [ForeignKey(nameof(StoreId))]
        public virtual MtdStore MtdStore { get; set; }
        
        [ForeignKey(nameof(FieldId))]
        public virtual MtdFormPartField MtdFormPartField { get; set; }
        
        [ForeignKey(nameof(ItemId))]
        public virtual MtdFormPartFieldItem MtdFormPartFieldItem { get; set; }
    }
}
