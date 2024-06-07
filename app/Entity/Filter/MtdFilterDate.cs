/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.

*/

using System;

namespace MtdKey.OrderMaker.Entity
{
    public partial class MtdFilterDate
    {
        public int Id { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }

        public virtual MtdFilter IdNavigation { get; set; }
    }
}
