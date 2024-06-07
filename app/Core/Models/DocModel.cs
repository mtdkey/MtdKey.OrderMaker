
using System;
using System.Collections.Generic;

namespace MtdKey.OrderMaker.Core
{
    public class DocModel
    {
        public string Id { get; set; }
        public string FormName { get; set; }
        public byte[] Image { get; set; }
        public string FormId { get; set; }
        public int Sequence { get; set; }
        public DateTime Created { get; set; }
        public bool EditDate { get; set; }
        public List<DocPartModel> Parts { get; set; } = new();
        public List<DocFieldModel> Fields { get; set; } = new();
    }
}
