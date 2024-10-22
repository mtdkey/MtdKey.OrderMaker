using Microsoft.EntityFrameworkCore;


namespace MtdKey.InboxMediator.Data
{
    public class InboxMediatorContext(DbContextOptions<InboxMediatorContext> options) : DbContext(options)
    {
        public DbSet<EmailMediator> EmailMediators { get; set; }
        public DbSet<InboxHeader> InboxHeaders { get; set; }
        public DbSet<LoaderFlag> LoaderFlags { get; set; }
        public DbSet<ReaderFlag> ReaderFlags { get; set; }
    }
}
