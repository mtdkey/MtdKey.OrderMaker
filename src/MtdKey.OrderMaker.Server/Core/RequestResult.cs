using System.Collections.Generic;

namespace MtdKey.OrderMaker.Core
{
    public class RequestResult
    {
        public int PageCount { get; set; } = 1;
        public List<DocModel> Docs { get; set; } = new();
    }
}
