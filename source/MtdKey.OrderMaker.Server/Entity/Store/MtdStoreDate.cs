/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/


using Microsoft.EntityFrameworkCore;
using MtdKey.OrderMaker.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MtdKey.OrderMaker.Entity
{
    [Table("mtd_store_date")]
    [Index(nameof(Result), Name = "IX_DATE_RESULT")]
    public class MtdStoreDate : IStoreField
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
        public DateTime Result { get; set; }
        [Required]
        public bool IsDeleted { get; set; } = false;

        [ForeignKey(nameof(StoreId))]
        public virtual MtdStore MtdStore { get; set; }
        [ForeignKey(nameof(FieldId))]
        public virtual MtdFormPartField MtdFormPartField { get; set; }
    }
}
