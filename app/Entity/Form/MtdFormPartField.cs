/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using System.Collections.Generic;

namespace MtdKey.OrderMaker.Entity
{
    public partial class MtdFormPartField
    {
        public MtdFormPartField()
        {
            MtdFilterColumn = new HashSet<MtdFilterColumn>();
            MtdFilterField = new HashSet<MtdFilterField>();
            MtdStoreDates = new HashSet<MtdStoreDate>();
            MtdStoreTexts = new HashSet<MtdStoreText>();
            MtdStoreInts = new HashSet<MtdStoreInt>();
            MtdStoreDecimals = new HashSet<MtdStoreDecimal>();
            MtdStoreMemos = new HashSet<MtdStoreMemo>();
            MtdStoreFiles = new HashSet<MtdStoreFile>();
            MtdFormPartFieldItems = new HashSet<MtdFormPartFieldItem>();
            MtdStoreItems = new HashSet<MtdStoreItem>();
            MtdStoreFileLinks = new HashSet<MtdStoreFileLink>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public sbyte Required { get; set; }
        public int Sequence { get; set; }
        public sbyte Active { get; set; }
        public sbyte ReadOnly { get; set; }
        public int MtdSysType { get; set; }
        public string MtdFormPartId { get; set; }
        public string DefaultData { get; set; }

        public virtual MtdFormPart MtdFormPartNavigation { get; set; }
        public virtual MtdSysType MtdSysTypeNavigation { get; set; }
        public virtual ICollection<MtdFilterColumn> MtdFilterColumn { get; set; }
        public virtual ICollection<MtdFilterField> MtdFilterField { get; set; }

        public virtual ICollection<MtdStoreDate> MtdStoreDates { get; set; }
        public virtual ICollection<MtdStoreText> MtdStoreTexts { get; set; }
        public virtual ICollection<MtdStoreInt> MtdStoreInts { get; set; }
        public virtual ICollection<MtdStoreDecimal> MtdStoreDecimals { get; set; }
        public virtual ICollection<MtdStoreMemo> MtdStoreMemos { get; set; }
        public virtual ICollection<MtdStoreFile> MtdStoreFiles { get; set; }
        public virtual ICollection<MtdFormPartFieldItem> MtdFormPartFieldItems { get; set; }
        public virtual ICollection<MtdStoreItem> MtdStoreItems { get; set; }
        public virtual ICollection<MtdStoreFileLink> MtdStoreFileLinks { get; set; }
        public virtual ICollection<TargetField> TargetFields { get; set; } 
    }
}
