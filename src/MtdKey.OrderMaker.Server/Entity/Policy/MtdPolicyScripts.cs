﻿namespace MtdKey.OrderMaker.Entity
{
    public class MtdPolicyScripts
    {
        public string Id { get; set; }
        public string MtdPolicyId { get; set; }
        public int MtdFilterScriptId { get; set; }

        public virtual MtdFilterScript MtdFilterScript { get; set; }
        public virtual MtdPolicy MtdPolicy { get; set; }
    }
}
