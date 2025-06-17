/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using System.Collections.Generic;

namespace MtdKey.OrderMaker.Entity
{
    public partial class MtdApproval
    {
        public MtdApproval()
        {
            MtdApprovalStages = new HashSet<MtdApprovalStage>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MtdForm { get; set; }
        public byte[] ImgStart { get; set; }
        public string ImgStartType { get; set; }
        public string ImgStartText { get; set; }
        public byte[] ImgIteraction { get; set; }
        public string ImgIteractionType { get; set; }
        public string ImgIteractionText { get; set; }
        public byte[] ImgWaiting { get; set; }
        public string ImgWaitingType { get; set; }
        public string ImgWaitingText { get; set; }
        public byte[] ImgApproved { get; set; }
        public string ImgApprovedType { get; set; }
        public string ImgApprovedText { get; set; }
        public byte[] ImgRejected { get; set; }
        public string ImgRejectedType { get; set; }
        public string ImgRejectedText { get; set; }
        public byte[] ImgRequired { get; set; }
        public string ImgRequiredType { get; set; }
        public string ImgRequiredText { get; set; }

        public virtual MtdForm MtdFormNavigation { get; set; }
        public virtual ICollection<MtdApprovalStage> MtdApprovalStages { get; set; }
    }
}
