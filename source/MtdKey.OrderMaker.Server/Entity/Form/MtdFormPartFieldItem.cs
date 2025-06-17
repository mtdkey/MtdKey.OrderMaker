using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MtdKey.OrderMaker.Entity
{

    [Table("mtd_form_part_field_item")]
    public class MtdFormPartFieldItem
    {
        public MtdFormPartFieldItem()
        {
            MtdStoreItems = new HashSet<MtdStoreItem>();
        }

        [Key]
        public Guid Id { get; set; }
        [MaxLength(250)]
        [Required]
        public string Name { get; set; } = string.Empty;
        [MaxLength(250)]
        [Required]
        public string FieldId { get; set; }
        [Required]
        public bool IsDeleted { get; set; } = false;

        [ForeignKey(nameof(FieldId))]
        public virtual MtdFormPartField MtdFormPartField { get; set; }

        public virtual ICollection<MtdStoreItem> MtdStoreItems { get; set; }
    }
}
