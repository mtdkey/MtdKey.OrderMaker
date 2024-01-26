using System;
using System.Collections.Generic;

namespace MtdKey.OrderMaker.Core.Scripts
{
    public class FilterSQLparams
    {
        public DateTime DateStart {  get; set; } = DateTime.MinValue;
        public DateTime DateEnd{ get; set; } = DateTime.MaxValue;
        public string FormId { get; set; } = string.Empty;
        public string StoreId { get; set; } = string.Empty;
        public string SearchText { get; set; } = string.Empty;
        public string OwnerId { get; set; } = string.Empty;
        public int Page { get; set;} = 1;
        public int PageSize { get; set; } = int.MaxValue;
        public string SortByFieldId { get; set; } = string.Empty;
        public string SortOrder { get; set; } = "asc";
        public string SearchNumber { get; set;} = string.Empty;
        public List<FilterFieldModel> FilterFields { get; set; } = new();
        public List<string> FilterColumnIds { get; set; } = new();
        public List<string> UserInGroupIds { get; set; } = new();
        public List<DocFieldModel> DocFieldModels { get; set; } = new();
        public TypeRequest TypeRequest { get; set; }
    }
}
