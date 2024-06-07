using System;

namespace MtdKey.OrderMaker.Entity
{
    public partial class MtdLogApproval
    {
        public int Id { get; set; }
        public string MtdStore { get; set; }
        public int Stage { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserRecipientId { get; set; }
        public string UserRecipientName { get; set; }
        public int Result { get; set; }
        public DateTime Timecr { get; set; }
        public byte[] ImgData { get; set; }
        public string ImgType { get; set; }
        public string Color { get; set; }
        public string Note { get; set; }
        public string Comment { get; set; }
        public sbyte IsSign { get; set; }


        public virtual MtdStore MtdStoreNavigation { get; set; }
        public virtual MtdApprovalStage StageNavigation { get; set; }
    }
}
