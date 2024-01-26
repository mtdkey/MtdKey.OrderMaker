/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/


using System.Collections.Generic;

namespace MtdKey.OrderMaker.Entity
{
    public partial class MtdFormPart
    {
        public MtdFormPart()
        {
            MtdFormPartField = new HashSet<MtdFormPartField>();
            MtdPolicyParts = new HashSet<MtdPolicyParts>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Sequence { get; set; }
        public sbyte Active { get; set; }
        public int MtdSysStyle { get; set; }
        public string MtdFormId { get; set; }
        public sbyte Title { get; set; }   

        public virtual MtdForm MtdFormNavigation { get; set; }
        public virtual MtdSysStyle MtdSysStyleNavigation { get; set; }
        public virtual MtdFormPartHeader MtdFormPartHeader { get; set; }
        public virtual ICollection<MtdFormPartField> MtdFormPartField { get; set; }
        public virtual ICollection<MtdPolicyParts> MtdPolicyParts { get; set; }

    }
}
