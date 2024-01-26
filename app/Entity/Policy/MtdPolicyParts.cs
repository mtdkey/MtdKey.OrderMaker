/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using System;
using System.Collections.Generic;

namespace MtdKey.OrderMaker.Entity
{
    public partial class MtdPolicyParts
    {
        public int Id { get; set; }
        public string MtdPolicy { get; set; }
        public string MtdFormPart { get; set; }
        public sbyte Create { get; set; }
        public sbyte Edit { get; set; }
        public sbyte View { get; set; }

        public virtual MtdFormPart MtdFormPartNavigation { get; set; }
        public virtual MtdPolicy MtdPolicyNavigation { get; set; }
    }
}
