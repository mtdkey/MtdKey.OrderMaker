/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.

*/


namespace MtdKey.OrderMaker.Models.Index
{
    public class HeaderModelView
    {
        public string FormId { get; set; }
        public string SearchText { get; set; }
        public bool WaitList { get; set; }
        public int Pending { get; set; }
        public bool IsApprovalForm {get;set;}
    }
}
