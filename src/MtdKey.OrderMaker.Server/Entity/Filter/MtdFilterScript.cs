/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using System.Collections.Generic;

namespace MtdKey.OrderMaker.Entity
{
    public partial class MtdFilterScript
    {
        public MtdFilterScript()
        {
            MtdPolicyScripts = new HashSet<MtdPolicyScripts>();
            MtdFilterScriptApply = new HashSet<MtdFilterScriptApply>();
        }

        public int Id { get; set; }
        public string MtdFormId { get; set; }
        public string Name { get; set; }
        public string Script { get; set; }

        public virtual MtdForm MtdForm { get; set; }
        public virtual ICollection<MtdPolicyScripts> MtdPolicyScripts { get; set; }
        public virtual ICollection<MtdFilterScriptApply> MtdFilterScriptApply { get; set; }
    }
}
