/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using MtdKey.OrderMaker.Core;
using MtdKey.OrderMaker.Entity;
using System.Collections.Generic;

namespace MtdKey.OrderMaker.Models.Index
{
    public class RowsModelView
    {
        public string FormId { get; set; }
        public string SearchNumber { get; set; }
        public int PageCount { get; set; }
        public bool ShowNumber { get; set; }
        public bool ShowDate { get; set; }
        public List<ApprovalStore> ApprovalStores { get; set; }
        public MtdApproval MtdApproval { get; set; }
        public string SearchText { get; set; }
        public bool IsCreator { get; set; }
        public int PageSize { get; set; }
        public int PageCurrent { get; set; }
        public int FieldsCount { get; set; }

        public List<DocModel> DocList { get; set; }

    }
}
