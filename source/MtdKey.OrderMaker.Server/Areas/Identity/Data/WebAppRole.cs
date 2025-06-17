using Microsoft.AspNetCore.Identity;

namespace MtdKey.OrderMaker.Areas.Identity.Data
{
    public class WebAppRole : IdentityRole
    {
        public string Title { get; set; }
        public int Seq { get; set; }

    }
}
