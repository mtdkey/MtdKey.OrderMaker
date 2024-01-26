/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

namespace MtdKey.OrderMaker.Entity
{
    public partial class MtdFilterField
    {
        public long Id { get; set; }
        public int MtdFilter { get; set; }
        public string MtdFormPartFieldId { get; set; }
        public string Value { get; set; }
        public string ValueExtra { get; set; }
        public int MtdTerm { get; set; }

        public virtual MtdFilter MtdFilterNavigation { get; set; }
        public virtual MtdFormPartField MtdFormPartFieldNavigation { get; set; }
        public virtual MtdSysTerm MtdTermNavigation { get; set; }
    }
}
