/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MtdKey.OrderMaker.Entity
{
    public partial class MtdStore
    {
        public MtdStore()
        {
            MtdLogDocument = new HashSet<MtdLogDocument>();
            MtdLogApproval = new HashSet<MtdLogApproval>();
            MtdStoreDates = new HashSet<MtdStoreDate>();
            MtdStoreTexts = new HashSet<MtdStoreText>();
            MtdStoreInts = new HashSet<MtdStoreInt>();
            MtdStoreDecimals = new HashSet<MtdStoreDecimal>();
            MtdStoreMemos = new HashSet<MtdStoreMemo>();
            MtdStoreFiles = new HashSet<MtdStoreFile>();
            MtdStoreItems = new HashSet<MtdStoreItem>();
        }

        public string Id { get; set; }
        public int Sequence { get; set; }
        public sbyte Active { get; set; }
        public string MtdFormId { get; set; }
        public DateTime Timecr { get; set; }
        [Required]
        public bool IsDeleted { get; set; } = false;

        public virtual MtdForm MtdFormNavigation { get; set; }
        public virtual MtdStoreApproval MtdStoreApproval { get; set; }      
        public virtual MtdStoreOwner MtdStoreOwner { get; set; }
        public virtual ICollection<MtdLogDocument> MtdLogDocument { get; set; }
        public virtual ICollection<MtdLogApproval> MtdLogApproval { get; set; }
        public virtual ICollection<MtdStoreDate> MtdStoreDates { get; set; }
        public virtual ICollection<MtdStoreText> MtdStoreTexts { get; set; }
        public virtual ICollection<MtdStoreInt> MtdStoreInts { get; set; }
        public virtual ICollection<MtdStoreDecimal> MtdStoreDecimals { get; set; }
        public virtual ICollection<MtdStoreMemo> MtdStoreMemos { get; set;}
        public virtual ICollection<MtdStoreFile> MtdStoreFiles { get; set;}
        public virtual ICollection<MtdStoreItem> MtdStoreItems { get; set;}
    }
}
