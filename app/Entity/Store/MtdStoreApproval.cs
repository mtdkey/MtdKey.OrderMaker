/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using System;

namespace MtdKey.OrderMaker.Entity
{
    public partial class MtdStoreApproval
    {
        public string Id { get; set; }
        public int MtdApproveStage { get; set; }
        public string PartsApproved { get; set; }
        public sbyte Complete { get; set; }
        public int Result { get; set; }
        public string SignChain { get; set; }
        public DateTime LastEventTime { get; set; }

        public virtual MtdStore IdNavigation { get; set; }
        public virtual MtdApprovalStage MtdApproveStageNavigation { get; set; }
    }
}
