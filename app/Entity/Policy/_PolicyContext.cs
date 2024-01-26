using Microsoft.EntityFrameworkCore;


namespace MtdKey.OrderMaker.Entity
{
    public partial class OrderMakerContext : DbContext
    {
        public virtual DbSet<MtdPolicy> MtdPolicy { get; set; }
        public virtual DbSet<MtdPolicyForms> MtdPolicyForms { get; set; }
        public virtual DbSet<MtdPolicyParts> MtdPolicyParts { get; set; }
        public virtual DbSet<MtdPolicyScripts> MtdPolicyScripts { get; set; }

        public void PolicyModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MtdPolicy>(entity =>
            {
                entity.ToTable("mtd_policy");

                entity.HasIndex(e => e.Id)
                    .HasDatabaseName("id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasColumnType("varchar(512)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(255)");
            });

            modelBuilder.Entity<MtdPolicyForms>(entity =>
            {
                entity.ToTable("mtd_policy_forms");

                entity.HasIndex(e => e.Id)
                    .HasDatabaseName("id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.MtdForm)
                    .HasDatabaseName("fk_policy_forms_form_idx");

                entity.HasIndex(e => e.MtdPolicy)
                    .HasDatabaseName("fk_policy_forms_policy_idx");

                entity.HasIndex(e => new { e.MtdPolicy, e.MtdForm })
                    .HasDatabaseName("UNIQUE_FORM")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ChangeDate)
                    .HasColumnName("change_date")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ChangeOwner)
                    .HasColumnName("change_owner")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Create)
                    .HasColumnName("create")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.DeleteAll)
                    .HasColumnName("delete_all")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.DeleteGroup)
                    .HasColumnName("delete_group")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.DeleteOwn)
                    .HasColumnName("delete_own")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.EditAll)
                    .HasColumnName("edit_all")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.EditGroup)
                    .HasColumnName("edit_group")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.EditOwn)
                    .HasColumnName("edit_own")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.MtdForm)
                    .IsRequired()
                    .HasColumnName("mtd_form")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.MtdPolicy)
                    .IsRequired()
                    .HasColumnName("mtd_policy")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.Reviewer)
                    .HasColumnName("reviewer")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ViewAll)
                    .HasColumnName("view_all")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ViewGroup)
                    .HasColumnName("view_group")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ViewOwn)
                    .HasColumnName("view_own")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.OwnDenyGroup)
                    .HasColumnName("own_deny_group")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ExportToExcel)
                    .HasColumnName("export_to_excel")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.RelatedCreate)
                    .HasColumnName("related_create")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.RelatedEdit)
                    .HasColumnName("related_edit")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.MtdFormNavigation)
                    .WithMany(p => p.MtdPolicyForms)
                    .HasForeignKey(d => d.MtdForm)
                    .HasConstraintName("fk_policy_forms_form");

                entity.HasOne(d => d.MtdPolicyNavigation)
                    .WithMany(p => p.MtdPolicyForms)
                    .HasForeignKey(d => d.MtdPolicy)
                    .HasConstraintName("fk_policy_forms_policy");
            });

            modelBuilder.Entity<MtdPolicyParts>(entity =>
            {
                entity.ToTable("mtd_policy_parts");

                entity.HasIndex(e => e.Id)
                    .HasDatabaseName("id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => new { e.MtdPolicy, e.MtdFormPart })
                    .HasDatabaseName("UNIQUE_PART")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Create)
                    .HasColumnName("create")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Edit)
                    .HasColumnName("edit")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.MtdFormPart)
                    .IsRequired()
                    .HasColumnName("mtd_form_part")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.MtdPolicy)
                    .IsRequired()
                    .HasColumnName("mtd_policy")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.View)
                    .HasColumnName("view")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.MtdFormPartNavigation)
                    .WithMany(p => p.MtdPolicyParts)
                    .HasForeignKey(d => d.MtdFormPart)
                    .HasConstraintName("fk_policy_part_part");

                entity.HasOne(d => d.MtdPolicyNavigation)
                    .WithMany(p => p.MtdPolicyParts)
                    .HasForeignKey(d => d.MtdPolicy)
                    .HasConstraintName("fk_policy_part_policy");
            });

            modelBuilder.Entity<MtdPolicyScripts>(entity =>
            {
                entity.ToTable("mtd_policy_scripts");

                entity.HasIndex(e => new { e.MtdFilterScriptId, e.MtdPolicyId })
                    .HasDatabaseName("Unique_Policy_Script")
                    .IsUnique();

                entity.HasIndex(e => e.Id)
                    .HasDatabaseName("Unique_id")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.MtdFilterScriptId)
                    .IsRequired()
                    .HasColumnName("mtd_filter_script_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MtdPolicyId)
                    .IsRequired()
                    .HasColumnName("mtd_policy_id")
                    .HasColumnType("varchar(36)");

                entity.HasOne(d => d.MtdPolicy)
                    .WithMany(p => p.MtdPolicyScripts)
                    .HasForeignKey(d => d.MtdPolicyId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_policy_script");

                entity.HasOne(d => d.MtdFilterScript)
                    .WithMany(p => p.MtdPolicyScripts)
                    .HasForeignKey(d => d.MtdFilterScriptId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_policy_filter");

            });
        }


    }
}
