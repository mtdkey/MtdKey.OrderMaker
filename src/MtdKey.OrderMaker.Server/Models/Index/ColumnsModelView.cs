/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using System.Collections.Generic;


namespace MtdKey.OrderMaker.Models.Index
{
    public class ColumnsModelView
    {
        public string FormId { get; set; }
        public List<ColumnItem> ColumnItems { get; set; }
        public bool ShowNumber { get; set; }
        public bool ShowDate { get; set; }
    }
}
