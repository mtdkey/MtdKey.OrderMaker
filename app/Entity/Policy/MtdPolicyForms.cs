/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using MtdKey.OrderMaker.Entity;
using System;
using System.Collections.Generic;

namespace MtdKey.OrderMaker.Entity
{
    public partial class MtdPolicyForms
    {
        public int Id { get; set; }
        public string MtdPolicy { get; set; }
        public string MtdForm { get; set; }
        public sbyte Create { get; set; }
        public sbyte EditAll { get; set; }
        public sbyte EditGroup { get; set; }
        public sbyte EditOwn { get; set; }
        public sbyte ViewAll { get; set; }
        public sbyte ViewGroup { get; set; }
        public sbyte ViewOwn { get; set; }
        public sbyte DeleteAll { get; set; }
        public sbyte DeleteGroup { get; set; }
        public sbyte DeleteOwn { get; set; }
        public sbyte ChangeOwner { get; set; }
        public sbyte Reviewer { get; set; }
        public sbyte ChangeDate { get; set; }
        public sbyte OwnDenyGroup { get; set; }
        public sbyte ExportToExcel { get; set; }
        public sbyte RelatedCreate { get; set; }
        public sbyte RelatedEdit { get; set; }

        public virtual MtdForm MtdFormNavigation { get; set; }
        public virtual MtdPolicy MtdPolicyNavigation { get; set; }
    }
}
