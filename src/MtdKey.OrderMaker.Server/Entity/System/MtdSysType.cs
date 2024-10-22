/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using System.Collections.Generic;

namespace MtdKey.OrderMaker.Entity
{
    public partial class MtdSysType
    {
        public MtdSysType()
        {
            MtdFormPartField = new HashSet<MtdFormPartField>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public sbyte Active { get; set; }

        public virtual ICollection<MtdFormPartField> MtdFormPartField { get; set; }
    }
}
