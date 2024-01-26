using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Entity
{
    public class MtdFormRelated
    {
        public string Id { get; set; }
        public string ParentFormId { get; set; }
        public string ChildFormId { get; set; }

        public virtual MtdForm MtdParentForm { get; set; }
        public virtual MtdForm MtdChildForm { get; set; }
    }
}
