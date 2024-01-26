using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Areas.Identity.Data
{
    public class WebAppRole : IdentityRole
    {
        public string Title { get; set; }
        public int Seq { get; set; }

    }
}
