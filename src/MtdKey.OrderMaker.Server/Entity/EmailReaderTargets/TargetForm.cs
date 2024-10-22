using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MtdKey.OrderMaker.Entity
{
    [Table("target_forms")]
    public class TargetForm
    {
        [Key]
        [MaxLength(36)]
        public string FormId { get; set; }

        [ForeignKey(nameof(FormId))]
        public virtual MtdForm MtdForm { get; set; }
        public virtual ICollection<TargetFilter> TargetFilters { get; set; } = [];
        public virtual ICollection<TargetField> TargetFields { get; set; } = [];

    }
}
