/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

using System;
using System.Collections.Generic;

namespace MtdKey.OrderMaker.Entity
{
    public partial class MtdPolicy
    {
        public MtdPolicy()
        {
            MtdPolicyForms = new HashSet<MtdPolicyForms>();            
            MtdPolicyParts = new HashSet<MtdPolicyParts>();
            MtdPolicyScripts = new HashSet<MtdPolicyScripts>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<MtdPolicyForms> MtdPolicyForms { get; set; }        
        public virtual ICollection<MtdPolicyParts> MtdPolicyParts { get; set; }
        public virtual ICollection<MtdPolicyScripts> MtdPolicyScripts { get; set; }
    }
}
