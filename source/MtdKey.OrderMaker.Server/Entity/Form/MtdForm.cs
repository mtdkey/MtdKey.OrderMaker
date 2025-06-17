/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/


using System.Collections.Generic;

namespace MtdKey.OrderMaker.Entity
{
    public partial class MtdForm
    {
        public MtdForm()
        {
            MtdApproval = new HashSet<MtdApproval>();
            MtdFilter = new HashSet<MtdFilter>();
            MtdFormParts = new HashSet<MtdFormPart>();
            MtdPolicyForms = new HashSet<MtdPolicyForms>();
            MtdStore = new HashSet<MtdStore>();
            MtdFilterScript = new HashSet<MtdFilterScript>();
            MtdEventSubscribes = new HashSet<MtdEventSubscribe>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public sbyte Active { get; set; }
        public string MtdCategory { get; set; }
        public int Sequence { get; set; }
        public sbyte VisibleNumber { get; set; }
        public sbyte VisibleDate { get; set; }

        public virtual MtdCategoryForm MtdCategoryNavigation { get; set; }
        public virtual MtdFormDesk MtdFormDesk { get; set; }
        public virtual MtdFormHeader MtdFormHeader { get; set; }
        public virtual ICollection<MtdApproval> MtdApproval { get; set; }
        public virtual ICollection<MtdFilter> MtdFilter { get; set; }
        public virtual ICollection<MtdFormPart> MtdFormParts { get; set; }
        public virtual ICollection<MtdPolicyForms> MtdPolicyForms { get; set; }
        public virtual ICollection<MtdStore> MtdStore { get; set; }
        public virtual ICollection<MtdFilterScript> MtdFilterScript { get; set; }
        public virtual ICollection<MtdEventSubscribe> MtdEventSubscribes { get; set; }
        public virtual ICollection<TargetForm> TargetForms { get; set; } = [];
    }
}

