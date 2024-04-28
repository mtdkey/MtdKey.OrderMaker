using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;

namespace MtdKey.OrderMaker.Entity
{
    public partial class OrderMakerContext : DbContext
    {
        public virtual DbSet<MtdStore> MtdStore { get; set; }
        public virtual DbSet<MtdStoreApproval> MtdStoreApproval { get; set; }
        public virtual DbSet<MtdStoreOwner> MtdStoreOwner { get; set; }

        public virtual DbSet<MtdStoreDate> MtdStoreDates{ get; set; }
        public virtual DbSet<MtdStoreText> MtdStoreTexts { get; set; }
        public virtual DbSet<MtdStoreInt> MtdStoreInts { get; set; }
        public virtual DbSet<MtdStoreDecimal> MtdStoreDecimals { get; set; }
        public virtual DbSet<MtdStoreMemo> MtdStoreMemos { get; set; }
        public virtual DbSet<MtdStoreFile> MtdStoreFiles { get; set; }

        public virtual DbSet<MtdStoreItem> MtdStoreItems { get; set; }

        public void StoreModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<MtdStore>(entity =>
            {
                entity.ToTable("mtd_store");

                entity.HasIndex(e => e.Id)
                    .HasDatabaseName("id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.MtdFormId)
                    .HasDatabaseName("fk_mtd_store_mtd_form1_idx");

                entity.HasIndex(e => e.Timecr)
                    .HasDatabaseName("IX_TIMECR");

                entity.HasIndex(e => new { e.MtdFormId, e.Sequence })
                    .HasDatabaseName("Seq_Unique")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.MtdFormId)
                    .IsRequired()
                    .HasColumnName("mtd_form")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.Sequence)
                    .HasColumnName("sequence")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Timecr)
                    .HasColumnName("timecr")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.MtdFormNavigation)
                    .WithMany(p => p.MtdStore)
                    .HasForeignKey(d => d.MtdFormId)
                    .HasConstraintName("fk_mtd_store_mtd_form1");
            });

            modelBuilder.Entity<MtdStoreApproval>(entity =>
            {
                entity.ToTable("mtd_store_approval");

                entity.HasIndex(e => e.Complete)
                    .HasDatabaseName("IX_APPROVED");

                entity.HasIndex(e => e.Id)
                    .HasDatabaseName("id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.MtdApproveStage)
                    .HasDatabaseName("fk_store_approve_stage_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.Complete)
                    .HasColumnName("complete")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.MtdApproveStage)
                    .HasColumnName("md_approve_stage")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PartsApproved)
                    .IsRequired()
                    .HasColumnName("parts_approved")
                    .HasColumnType("longtext");

                entity.Property(e => e.Result)
                    .HasColumnName("result")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.SignChain)
                    .HasColumnName("sign_chain")
                    .HasColumnType("longtext");

                entity.Property(e => e.LastEventTime)
                    .IsRequired()
                    .HasColumnName("last_event_time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.MtdStoreApproval)
                    .HasForeignKey<MtdStoreApproval>(d => d.Id)
                    .HasConstraintName("fk_store_approve");

                entity.HasOne(d => d.MtdApproveStageNavigation)
                    .WithMany(p => p.MtdStoreApproval)
                    .HasForeignKey(d => d.MtdApproveStage)
                    .HasConstraintName("fk_store_approve_stage");
            });
            modelBuilder.Entity<MtdStoreOwner>(entity =>
            {
                entity.ToTable("mtd_store_owner");

                entity.HasIndex(e => e.Id)
                    .HasDatabaseName("id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.UserId)
                    .HasDatabaseName("IX_USER");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("user_name")
                    .HasColumnType("varchar(256)");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.MtdStoreOwner)
                    .HasForeignKey<MtdStoreOwner>(d => d.Id)
                    .HasConstraintName("fk_owner_store");
            });

            modelBuilder.Entity<MtdStore>().HasQueryFilter(b => b.IsDeleted == false);
            modelBuilder.Entity<MtdStoreText>().HasQueryFilter(b => b.MtdStore.IsDeleted == false && b.IsDeleted == false);
            modelBuilder.Entity<MtdStoreMemo>().HasQueryFilter(b => b.MtdStore.IsDeleted == false && b.IsDeleted == false);
            modelBuilder.Entity<MtdStoreInt>().HasQueryFilter(b => b.MtdStore.IsDeleted == false && b.IsDeleted == false);
            modelBuilder.Entity<MtdStoreFile>().HasQueryFilter(b => b.MtdStore.IsDeleted == false && b.IsDeleted == false);
            modelBuilder.Entity<MtdStoreDecimal>().HasQueryFilter(b => b.MtdStore.IsDeleted == false && b.IsDeleted == false);
            modelBuilder.Entity<MtdStoreDate>().HasQueryFilter(b => b.MtdStore.IsDeleted == false && b.IsDeleted == false);
            modelBuilder.Entity<MtdStoreItem>().HasQueryFilter(b => b.MtdStore.IsDeleted == false && b.IsDeleted == false);

            modelBuilder.Entity<MtdLogApproval>().HasQueryFilter(b => b.MtdStoreNavigation.IsDeleted == false);
            modelBuilder.Entity<MtdLogDocument>().HasQueryFilter(b => b.MtdStoreNavigation.IsDeleted == false);
            modelBuilder.Entity<MtdStoreApproval>().HasQueryFilter(b => b.IdNavigation.IsDeleted == false);
            modelBuilder.Entity<MtdStoreOwner>().HasQueryFilter(b => b.IdNavigation.IsDeleted == false);
        }


    }
}
