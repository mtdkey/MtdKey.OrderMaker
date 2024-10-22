/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

namespace MtdKey.OrderMaker.Models.Index
{
    public class FooterModelView
    {
        public string FormId { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int PageCount { get; set; }
    }
}
