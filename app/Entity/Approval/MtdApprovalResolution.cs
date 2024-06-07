/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

namespace MtdKey.OrderMaker.Entity
{
    public partial class MtdApprovalResolution
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public int Sequence { get; set; }
        public string Color { get; set; }
        public int MtdApprovalStageId { get; set; }
        public byte[] ImgData { get; set; }
        public string ImgType { get; set; }

        public virtual MtdApprovalStage MtdApprovalStage { get; set; }
    }
}
