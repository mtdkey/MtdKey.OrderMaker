using Microsoft.EntityFrameworkCore;

namespace MtdKey.OrderMaker.Entity
{
    public partial class OrderMakerContext : DbContext
    {
        public virtual DbSet<MtdLogApproval> MtdLogApproval { get; set; }
        public virtual DbSet<MtdLogDocument> MtdLogDocument { get; set; }

        public void LogModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MtdLogApproval>(entity =>
            {
                entity.ToTable("mtd_log_approval");

                entity.HasIndex(e => e.Id)
                    .HasDatabaseName("id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.MtdStore)
                    .HasDatabaseName("fk_log_approval_store_idx");

                entity.HasIndex(e => e.Stage)
                    .HasDatabaseName("fk_log_approval_stage_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MtdStore)
                    .IsRequired()
                    .HasColumnName("mtd_store")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.Result)
                    .HasColumnName("result")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Stage)
                    .HasColumnName("stage")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Timecr)
                    .HasColumnName("timecr")
                    .HasColumnType("datetime");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("user_name")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("'No Name'");

                entity.Property(e => e.UserRecipientId)
                    .HasColumnName("user_recipient_id")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.UserRecipientName)
                    .HasColumnName("user_recipient_name")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.IsSign)
                    .HasColumnName("is_sign")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ImgData)
                    .HasColumnName("img_data")
                    .HasColumnType("mediumblob");

                entity.Property(e => e.ImgType)
                    .HasColumnName("img_type")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Color)
                    .HasColumnName("color")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Note)
                    .HasColumnName("note")
                    .HasColumnType("varchar(512)");

                entity.Property(e => e.Comment)
                    .HasColumnName("app_comment")
                    .HasColumnType("varchar(512)");

                entity.HasOne(d => d.MtdStoreNavigation)
                    .WithMany(p => p.MtdLogApproval)
                    .HasForeignKey(d => d.MtdStore)
                    .HasConstraintName("fk_log_approval_store");

                entity.HasOne(d => d.StageNavigation)
                    .WithMany(p => p.MtdLogApproval)
                    .HasForeignKey(d => d.Stage)
                    .HasConstraintName("fk_log_approval_stage");
            });

            modelBuilder.Entity<MtdLogDocument>(entity =>
            {
                entity.ToTable("mtd_log_document");

                entity.HasIndex(e => e.Id)
                    .HasDatabaseName("id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.MtdStore)
                    .HasDatabaseName("fk_log_document_store_idx");

                entity.HasIndex(e => e.TimeCh)
                    .HasDatabaseName("ix_date");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MtdStore)
                    .IsRequired()
                    .HasColumnName("mtd_store")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.TimeCh)
                    .HasColumnName("timech")
                    .HasColumnType("datetime");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("user_name")
                    .HasColumnType("varchar(256)");

                entity.HasOne(d => d.MtdStoreNavigation)
                    .WithMany(p => p.MtdLogDocument)
                    .HasForeignKey(d => d.MtdStore)
                    .HasConstraintName("fk_log_document_store");
            });
        }

    }
}
