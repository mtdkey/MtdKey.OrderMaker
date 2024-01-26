/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using System.Collections.Generic;

namespace MtdKey.OrderMaker.Entity
{
    public partial class MtdCategoryForm
    {
        public MtdCategoryForm()
        {
            MtdForm = new HashSet<MtdForm>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Parent { get; set; }

        public virtual ICollection<MtdForm> MtdForm { get; set; }
    }
}
