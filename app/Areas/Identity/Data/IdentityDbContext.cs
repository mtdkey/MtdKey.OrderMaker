using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MtdKey.OrderMaker.Areas.Identity.Data;

namespace MtdKey.OrderMaker.Entity
{
    public class IdentityDbContext : IdentityDbContext<WebAppUser, WebAppRole, string>, IDataProtectionKeyContext
    {

        //private readonly Guid DatabaseId;
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
            : base(options)
        {
            //DatabaseId = Guid.Empty;

            //var item = _contextAccessor.HttpContext?.Items["databaseId"] ?? string.Empty;
            //if(Guid.TryParse((string) item, out Guid guid)) 
            //    DatabaseId = guid;
        }
        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<WebAppUser>().HasQueryFilter(p => p.DatabaseId == DatabaseId);
        }


    }
}
