﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MtdKey.OrderMaker.Core;
using Microsoft.EntityFrameworkCore;
using System;

namespace MtdKey.OrderMaker.Entity
{
    [Table("mtd_store_file_links")]
    [Index(nameof(Result), Name = "IX_FILE_LINK_RESULT")]
    public class MtdStoreFileLink : IStoreField
    {
        [Key]
        public long Id { get; set; }
        [MaxLength(36)]
        [Required]
        public string StoreId { get; set; }
        [MaxLength(36)]
        [Required]
        public string FieldId { get; set; }
        [MaxLength(256)]
        [Required]
        public string FileName { get; set; }
        [Required]
        public long FileSize { get; set; }
        [MaxLength(256)]
        [Required]
        public string FileType { get; set; }
        [Required]
        public Guid Result { get; set; }
        [Required]
        public bool IsDeleted { get; set; } = false;

        [ForeignKey(nameof(StoreId))]
        public virtual MtdStore MtdStore { get; set; }
        [ForeignKey(nameof(FieldId))]
        public virtual MtdFormPartField MtdFormPartField { get; set; }
    }
}
