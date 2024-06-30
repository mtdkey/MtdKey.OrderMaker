using Microsoft.EntityFrameworkCore;

namespace MtdKey.OrderMaker.Entity
{
    public partial class OrderMakerContext : DbContext
    {
        public virtual DbSet<TargetForm> TargetForms { get; set; }
        public virtual DbSet<TargetField> TargetFields { get; set; }
        public virtual DbSet<TargetFilter> TargetFilters { get; set; }  
    }
}
