/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using System.Collections.Generic;

namespace MtdKey.OrderMaker.Entity
{
    public partial class MtdApprovalStage
    {
        public MtdApprovalStage()
        {
            MtdApprovalResolution = new HashSet<MtdApprovalResolution>();
            MtdApprovalRejection = new HashSet<MtdApprovalRejection>();
            MtdLogApproval = new HashSet<MtdLogApproval>();
            MtdStoreApproval = new HashSet<MtdStoreApproval>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MtdApproval { get; set; }
        public int Stage { get; set; }
        public string UserId { get; set; }
        public string BlockParts { get; set; }

        public virtual MtdApproval MtdApprovalNavigation { get; set; }
        public virtual ICollection<MtdApprovalResolution> MtdApprovalResolution { get; set; }
        public virtual ICollection<MtdApprovalRejection> MtdApprovalRejection { get; set; }
        public virtual ICollection<MtdLogApproval> MtdLogApproval { get; set; }
        public virtual ICollection<MtdStoreApproval> MtdStoreApproval { get; set; }
    }
}
