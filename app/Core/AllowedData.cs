using System.Collections.Generic;

namespace MtdKey.OrderMaker.Core
{
    public class AllowedData
    {
        public  List<DocPartModel> DocParts { get; set; }
        public List<DocFieldModel> DocFields { get; set; }
        public List<string> UsersInGroupIds { get; set; } = [];
    }
}
