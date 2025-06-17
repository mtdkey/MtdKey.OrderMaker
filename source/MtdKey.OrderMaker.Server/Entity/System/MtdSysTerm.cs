/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using System.Collections.Generic;

namespace MtdKey.OrderMaker.Entity
{
    public partial class MtdSysTerm
    {
        public MtdSysTerm()
        {
            MtdFilterField = new HashSet<MtdFilterField>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Sign { get; set; }

        public virtual ICollection<MtdFilterField> MtdFilterField { get; set; }
    }
}
