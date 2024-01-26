using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Areas.Identity.Data
{
    [Index(nameof(DatabaseId), Name = "DataBaseId", IsUnique = false )]
    public class WebAppUser : IdentityUser
    {
        [PersonalData]
        public string Title { get; set; }
        public string TitleGroup { get; set; }
        public Guid DatabaseId { get; set; } = Guid.Empty;
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
