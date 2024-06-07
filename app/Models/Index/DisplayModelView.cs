/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using System.Collections.Generic;


namespace MtdKey.OrderMaker.Models.Index
{
    public class DisplayData
    {
        public long Id { get; set; }
        public string Header { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
    }

    public class DisplayModelView
    {
        public string FormId { get; set; }
        public int IdFilter { get; set; }
        public List<DisplayData> DisplayDatas { get; set; }

    }
}
