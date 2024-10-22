using Microsoft.EntityFrameworkCore;

namespace MtdKey.OrderMaker.Entity
{
    public partial class OrderMakerContext : DbContext
    {
        public virtual DbSet<MtdApproval> MtdApproval { get; set; }
        public virtual DbSet<MtdApprovalResolution> MtdApprovalResolution { get; set; }
        public virtual DbSet<MtdApprovalRejection> MtdApprovalRejection { get; set; }
        public virtual DbSet<MtdApprovalStage> MtdApprovalStage { get; set; }


        public void ApprovalModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<MtdApproval>(entity =>
            {
                entity.ToTable("mtd_approval");

                entity.HasIndex(e => e.Id)
                    .HasDatabaseName("id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.MtdForm)
                    .HasDatabaseName("fk_approvel_form_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasColumnType("varchar(512)");

                entity.Property(e => e.MtdForm)
                    .IsRequired()
                    .HasColumnName("mtd_form")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(120)");

                entity.Property(e => e.ImgStart)
                    .HasColumnName("img_start")
                    .HasColumnType("mediumblob");

                entity.Property(e => e.ImgStartType)
                    .HasColumnName("img_start_type")
                    .HasColumnType("varchar(48)");

                entity.Property(e => e.ImgStartText)
                    .HasColumnName("img_start_text")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.ImgIteraction)
                    .HasColumnName("img_iteraction")
                    .HasColumnType("mediumblob");

                entity.Property(e => e.ImgIteractionType)
                    .HasColumnName("img_iteraction_type")
                    .HasColumnType("varchar(48)");

                entity.Property(e => e.ImgIteractionText)
                    .HasColumnName("img_iteraction_text")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.ImgWaiting)
                    .HasColumnName("img_waiting")
                    .HasColumnType("mediumblob");

                entity.Property(e => e.ImgWaitingType)
                    .HasColumnName("img_waiting_type")
                    .HasColumnType("varchar(48)");

                entity.Property(e => e.ImgWaitingText)
                    .HasColumnName("img_waiting_text")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.ImgApproved)
                    .HasColumnName("img_approved")
                    .HasColumnType("mediumblob");

                entity.Property(e => e.ImgApprovedType)
                    .HasColumnName("img_approved_type")
                    .HasColumnType("varchar(48)");

                entity.Property(e => e.ImgApprovedText)
                    .HasColumnName("img_approved_text")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.ImgRejected)
                    .HasColumnName("img_rejected")
                    .HasColumnType("mediumblob");

                entity.Property(e => e.ImgRejectedType)
                    .HasColumnName("img_rejected_type")
                    .HasColumnType("varchar(48)");

                entity.Property(e => e.ImgRejectedText)
                    .HasColumnName("img_rejected_text")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.ImgRequired)
                    .HasColumnName("img_required")
                    .HasColumnType("mediumblob");

                entity.Property(e => e.ImgRequiredType)
                    .HasColumnName("img_required_type")
                    .HasColumnType("varchar(48)");

                entity.Property(e => e.ImgRequiredText)
                    .HasColumnName("img_required_text")
                    .HasColumnType("varchar(255)");

                entity.HasOne(d => d.MtdFormNavigation)
                    .WithMany(p => p.MtdApproval)
                    .HasForeignKey(d => d.MtdForm)
                    .HasConstraintName("fk_approvel_form");
            });

            modelBuilder.Entity<MtdApprovalResolution>(entity =>
            {
                entity.ToTable("mtd_approval_resolution");

                entity.HasIndex(e => e.Id)
                    .HasDatabaseName("id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.MtdApprovalStageId)
                    .HasDatabaseName("fk_resolution_stage_idx");

                entity.HasIndex(e => e.Sequence)
                    .HasDatabaseName("ix_sequence");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.Color)
                    .IsRequired()
                    .HasColumnName("color")
                    .HasColumnType("varchar(45)")
                    .HasDefaultValueSql("'green'");

                entity.Property(e => e.MtdApprovalStageId)
                    .HasColumnName("mtd_approval_stage_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Note)
                    .IsRequired()
                    .HasColumnName("note")
                    .HasColumnType("varchar(512)");

                entity.Property(e => e.ImgData)
                    .HasColumnName("img_data")
                    .HasColumnType("mediumblob");

                entity.Property(e => e.ImgType)
                    .HasColumnName("img_type")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Sequence)
                    .HasColumnName("sequence")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.MtdApprovalStage)
                    .WithMany(p => p.MtdApprovalResolution)
                    .HasForeignKey(d => d.MtdApprovalStageId)
                    .HasConstraintName("fk_resolution_stage");
            });

            modelBuilder.Entity<MtdApprovalRejection>(entity =>
            {
                entity.ToTable("mtd_approval_rejection");

                entity.HasIndex(e => e.Id)
                    .HasDatabaseName("id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.MtdApprovalStageId)
                    .HasDatabaseName("fk_rejection_stage_idx");

                entity.HasIndex(e => e.Sequence)
                    .HasDatabaseName("ix_sequence");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.Color)
                    .IsRequired()
                    .HasColumnName("color")
                    .HasColumnType("varchar(45)")
                    .HasDefaultValueSql("'green'");

                entity.Property(e => e.MtdApprovalStageId)
                    .HasColumnName("mtd_approval_stage_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Note)
                    .IsRequired()
                    .HasColumnName("note")
                    .HasColumnType("varchar(512)");

                entity.Property(e => e.ImgData)
                    .HasColumnName("img_data")
                    .HasColumnType("mediumblob");

                entity.Property(e => e.ImgType)
                    .HasColumnName("img_type")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Sequence)
                    .HasColumnName("sequence")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.MtdApprovalStage)
                    .WithMany(p => p.MtdApprovalRejection)
                    .HasForeignKey(d => d.MtdApprovalStageId)
                    .HasConstraintName("fk_rejection_stage");
            });

            modelBuilder.Entity<MtdApprovalStage>(entity =>
            {
                entity.ToTable("mtd_approval_stage");

                entity.HasIndex(e => e.Id)
                    .HasDatabaseName("id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.MtdApproval)
                    .HasDatabaseName("fk_stage_approval_idx");

                entity.HasIndex(e => e.UserId)
                    .HasDatabaseName("IX_USER");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.BlockParts)
                    .IsRequired()
                    .HasColumnName("block_parts")
                    .HasColumnType("longtext");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasColumnType("varchar(512)");

                entity.Property(e => e.MtdApproval)
                    .IsRequired()
                    .HasColumnName("mtd_approval")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(120)");

                entity.Property(e => e.Stage)
                    .HasColumnName("stage")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id")
                    .HasColumnType("varchar(36)");

                entity.HasOne(d => d.MtdApprovalNavigation)
                    .WithMany(p => p.MtdApprovalStages)
                    .HasForeignKey(d => d.MtdApproval)
                    .HasConstraintName("fk_stage_approval");
            });
        }

    }




}
