using System;
using System.Collections.Generic;

namespace MtdKey.OrderMaker.Core
{
    public class DocModel
    {
        public string Id { get; set; }
        public string FormId { get; set; }
        public string FormName { get; set; }
        public int Sequence { get; set; }
        public byte[] Image { get; set; }
        public DateTime Created { get; set; }
        public List<DocPartModel> Parts { get; set; } = [];
        public List<DocFieldModel> Fields { get; set; } = [];
    }
}
