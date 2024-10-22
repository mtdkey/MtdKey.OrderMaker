/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using System.Collections.Generic;

namespace MtdKey.OrderMaker.Entity
{
    public partial class MtdFilter
    {
        public MtdFilter()
        {
            MtdFilterColumns = new HashSet<MtdFilterColumn>();
            MtdFilterFields = new HashSet<MtdFilterField>();
            MtdFilterScriptApply = new HashSet<MtdFilterScriptApply>();
        }

        public int Id { get; set; }
        public string IdUser { get; set; }
        public string MtdFormId { get; set; }
        public int PageSize { get; set; }
        public string SearchText { get; set; }
        public string SearchNumber { get; set; }
        public int Page { get; set; }
        public sbyte ShowNumber { get; set; }
        public sbyte ShowDate { get; set; }
        public string Sort { get; set; }
        public string SortOrder { get; set; }

        public virtual MtdForm MtdFormNavigation { get; set; }
        public virtual MtdFilterDate MtdFilterDate { get; set; }
        public virtual MtdFilterOwner MtdFilterOwner { get; set; }
        public virtual ICollection<MtdFilterColumn> MtdFilterColumns { get; set; }
        public virtual ICollection<MtdFilterField> MtdFilterFields { get; set; }
        public virtual ICollection<MtdFilterScriptApply> MtdFilterScriptApply { get; set; }
    }
}
