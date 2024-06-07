/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using MtdKey.OrderMaker.Models.Controls.MTDSelectList;
using System.Collections.Generic;


namespace MtdKey.OrderMaker.Models.Index
{
    public class SelectorModelView
    {
        public string FormId { get; set; }
        public List<MTDSelectListItem> ScriptItems { get; set; }
        public List<MTDSelectListItem> ServiceItems { get; set; }
        public List<MTDSelectListItem> CustomItems { get; set; }
        public List<MTDSelectListItem> TermItems { get; set; }
        public List<MTDSelectListItem> UsersItems { get; set; }
    }
}
