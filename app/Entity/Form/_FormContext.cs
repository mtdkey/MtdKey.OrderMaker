using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Entity
{
    public partial class OrderMakerContext : DbContext
    {
        public virtual DbSet<MtdForm> MtdForm { get; set; }
        public virtual DbSet<MtdFormRelated> MtdFormRelated { get; set; }
        public virtual DbSet<MtdFormDesk> MtdFormDesk { get; set; }
        public virtual DbSet<MtdFormHeader> MtdFormHeader { get; set; }
        public virtual DbSet<MtdFormPart> MtdFormPart { get; set; }
        public virtual DbSet<MtdFormPartField> MtdFormPartField { get; set; }
        public virtual DbSet<MtdFormPartHeader> MtdFormPartHeader { get; set; }

        public void FormModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MtdForm>(entity =>
            {
                entity.ToTable("mtd_form");

                entity.HasIndex(e => e.Id)
                    .HasDatabaseName("id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasColumnType("varchar(512)");

                entity.Property(e => e.MtdCategory)
                    .IsRequired()
                    .HasColumnName("mtd_category")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(120)");

                entity.Property(e => e.Sequence)
                    .HasColumnName("sequence")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.VisibleDate)
                    .HasColumnName("visible_date")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.VisibleNumber)
                    .HasColumnName("visible_number")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'1'");

                entity.HasOne(d => d.MtdCategoryNavigation)
                    .WithMany(p => p.MtdForm)
                    .HasForeignKey(d => d.MtdCategory)
                    .HasConstraintName("fk_form_category");

            });


            modelBuilder.Entity<MtdFormDesk>(entity =>
            {
                entity.ToTable("mtd_form_desk");

                entity.HasIndex(e => e.Id)
                    .HasDatabaseName("id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.ColorBack)
                    .IsRequired()
                    .HasColumnName("color_back")
                    .HasColumnType("varchar(45)")
                    .HasDefaultValueSql("'gray'");

                entity.Property(e => e.ColorFont)
                    .IsRequired()
                    .HasColumnName("color_font")
                    .HasColumnType("varchar(45)")
                    .HasDefaultValueSql("'white'");

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasColumnName("image")
                    .HasColumnType("mediumblob");

                entity.Property(e => e.ImageSize)
                    .HasColumnName("image_size")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ImageType)
                    .IsRequired()
                    .HasColumnName("image_type")
                    .HasColumnType("varchar(256)");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.MtdFormDesk)
                    .HasForeignKey<MtdFormDesk>(d => d.Id)
                    .HasConstraintName("fk_mtd_form_des_mtd_from");
            });

            modelBuilder.Entity<MtdFormHeader>(entity =>
            {
                entity.ToTable("mtd_form_header");

                entity.HasIndex(e => e.Id)
                    .HasDatabaseName("id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasColumnName("image")
                    .HasColumnType("mediumblob");

                entity.Property(e => e.ImageSize)
                    .HasColumnName("image_size")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ImageType)
                    .IsRequired()
                    .HasColumnName("image_type")
                    .HasColumnType("varchar(256)");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.MtdFormHeader)
                    .HasForeignKey<MtdFormHeader>(d => d.Id)
                    .HasConstraintName("fk_image_form");
            });

            modelBuilder.Entity<MtdFormRelated>(entity =>
            {
                entity.ToTable("mtd_form_related");

                entity.HasIndex(e => e.Id)
                    .HasDatabaseName("id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.ParentFormId)
                    .HasDatabaseName("fk_parent_form_idx");

                entity.HasIndex(e => e.ChildFormId)
                    .HasDatabaseName("fk_child_form_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.ParentFormId)
                    .IsRequired()
                    .HasColumnName("parent_form_id")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.ChildFormId)
                    .IsRequired()
                    .HasColumnName("child_form_id")
                    .HasColumnType("varchar(36)");

                entity.HasOne(d => d.MtdParentForm)
                    .WithMany(p => p.MtdParentForms)
                    .HasForeignKey(d => d.ParentFormId)
                    .HasConstraintName("fk_parent_form");

                entity.HasOne(d => d.MtdChildForm)
                    .WithMany(p => p.MtdChildForms)
                    .HasForeignKey(d => d.ChildFormId)
                    .HasConstraintName("fk_child_form");
            });

            modelBuilder.Entity<MtdFormPart>(entity =>
            {
                entity.ToTable("mtd_form_part");

                entity.HasIndex(e => e.Id)
                    .HasDatabaseName("id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.MtdFormId)
                    .HasDatabaseName("fk_mtd_form_part_mtd_form1_idx");

                entity.HasIndex(e => e.MtdSysStyle)
                    .HasDatabaseName("fk_mtd_form_part_mtd_sys_style1_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasColumnType("varchar(512)");

                entity.Property(e => e.MtdFormId)
                    .IsRequired()
                    .HasColumnName("mtd_form")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.MtdSysStyle)
                    .HasColumnName("mtd_sys_style")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(120)");

                entity.Property(e => e.Sequence)
                    .HasColumnName("sequence")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'1'");

                entity.HasOne(d => d.MtdFormNavigation)
                    .WithMany(p => p.MtdFormParts)
                    .HasForeignKey(d => d.MtdFormId)
                    .HasConstraintName("fk_mtd_form_part_mtd_form1");

                entity.HasOne(d => d.MtdSysStyleNavigation)
                    .WithMany(p => p.MtdFormPart)
                    .HasForeignKey(d => d.MtdSysStyle)
                    .HasConstraintName("fk_mtd_form_part_mtd_sys_style1");
            });

            modelBuilder.Entity<MtdFormPartField>(entity =>
            {
                entity.ToTable("mtd_form_part_field");

                entity.HasIndex(e => e.Id)
                    .HasDatabaseName("id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.MtdFormPartId)
                    .HasDatabaseName("fk_mtd_form_part_field_mtd_form_part1_idx");

                entity.HasIndex(e => e.MtdSysType)
                    .HasDatabaseName("fk_mtd_form_part_field_mtd_sys_type1_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasColumnType("varchar(512)");

                entity.Property(e => e.MtdFormPartId)
                    .IsRequired()
                    .HasColumnName("mtd_form_part")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.MtdSysType)
                    .HasColumnName("mtd_sys_type")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(120)");

                entity.Property(e => e.Required)
                    .HasColumnName("required")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ReadOnly)
                    .HasColumnName("read_only")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Sequence)
                    .HasColumnName("sequence")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.DefaultData)
                    .IsRequired()
                    .HasColumnName("default_data")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValue("");


                entity.HasOne(d => d.MtdFormPartNavigation)
                    .WithMany(p => p.MtdFormPartField)
                    .HasForeignKey(d => d.MtdFormPartId)
                    .HasConstraintName("fk_mtd_form_part_field_mtd_form_part1");

                entity.HasOne(d => d.MtdSysTypeNavigation)
                    .WithMany(p => p.MtdFormPartField)
                    .HasForeignKey(d => d.MtdSysType)
                    .HasConstraintName("fk_mtd_form_part_field_mtd_sys_type1");
            });

            modelBuilder.Entity<MtdFormPartHeader>(entity =>
            {
                entity.ToTable("mtd_form_part_header");

                entity.HasIndex(e => e.Id)
                    .HasDatabaseName("id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasColumnName("image")
                    .HasColumnType("mediumblob");

                entity.Property(e => e.ImageSize)
                    .HasColumnName("image_size")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ImageType)
                    .IsRequired()
                    .HasColumnName("image_type")
                    .HasColumnType("varchar(256)");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.MtdFormPartHeader)
                    .HasForeignKey<MtdFormPartHeader>(d => d.Id)
                    .HasConstraintName("fk_image_form_part");
            });
        }

    }
}
