using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Entity
{
    public partial class OrderMakerContext : DbContext
    {
        public virtual DbSet<MtdFilter> MtdFilter { get; set; }
        public virtual DbSet<MtdFilterColumn> MtdFilterColumn { get; set; }
        public virtual DbSet<MtdFilterDate> MtdFilterDate { get; set; }
        public virtual DbSet<MtdFilterOwner> MtdFilterOwner { get; set; }
        public virtual DbSet<MtdFilterField> MtdFilterField { get; set; }
        public virtual DbSet<MtdFilterScript> MtdFilterScript { get; set; }
        public virtual DbSet<MtdFilterScriptApply> MtdFilterScriptApply { get; set; }

        public void FilterModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MtdFilter>(entity =>
            {
                entity.ToTable("mtd_filter");

                entity.HasIndex(e => e.Id)
                    .HasDatabaseName("id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.IdUser)
                    .HasDatabaseName("IX_INDEX_USER");

                entity.HasIndex(e => e.MtdFormId)
                    .HasDatabaseName("mtd_filter_mtd_form_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IdUser)
                    .IsRequired()
                    .HasColumnName("idUser")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.MtdFormId)
                    .IsRequired()
                    .HasColumnName("mtd_form")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.Page)
                    .HasColumnName("page")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.PageSize)
                    .HasColumnName("page_size")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'10'");

                entity.Property(e => e.SearchNumber)
                    .IsRequired()
                    .HasColumnName("searchNumber")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.SearchText)
                    .IsRequired()
                    .HasColumnName("searchText")
                    .HasColumnType("varchar(256)");

                entity.Property(e => e.ShowDate)
                    .HasColumnName("show_date")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Sort)
                    .HasColumnName("sort")
                    .HasColumnType("text");

                entity.Property(e => e.SortOrder)
                    .HasColumnName("sort_order")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.ShowNumber)
                    .HasColumnName("show_number")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'1'");

                entity.HasOne(d => d.MtdFormNavigation)
                    .WithMany(p => p.MtdFilter)
                    .HasForeignKey(d => d.MtdFormId)
                    .HasConstraintName("mtd_filter_mtd_form");
            });

            modelBuilder.Entity<MtdFilterColumn>(entity =>
            {
                entity.ToTable("mtd_filter_column");

                entity.HasIndex(e => e.Id)
                    .HasDatabaseName("id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.MtdFilter)
                    .HasDatabaseName("mtd_filter_column_idx");

                entity.HasIndex(e => e.MtdFormPartFieldId)
                    .HasDatabaseName("mtd_roster_field_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MtdFilter)
                    .HasColumnName("mtd_filter")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MtdFormPartFieldId)
                    .IsRequired()
                    .HasColumnName("mtd_form_part_field")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.Sequence)
                    .HasColumnName("sequence")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.HasOne(d => d.MtdFilterNavigation)
                    .WithMany(p => p.MtdFilterColumns)
                    .HasForeignKey(d => d.MtdFilter)
                    .HasConstraintName("mtd_filter_column_mtd_field");

                entity.HasOne(d => d.MtdFormPartFieldNavigation)
                    .WithMany(p => p.MtdFilterColumn)
                    .HasForeignKey(d => d.MtdFormPartFieldId)
                    .HasConstraintName("mtd_roster_field");
            });

            modelBuilder.Entity<MtdFilterDate>(entity =>
            {
                entity.ToTable("mtd_filter_date");

                entity.HasIndex(e => e.Id)
                    .HasDatabaseName("id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DateEnd)
                    .HasColumnName("date_end")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateStart)
                    .HasColumnName("date_start")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.MtdFilterDate)
                    .HasForeignKey<MtdFilterDate>(d => d.Id)
                    .HasConstraintName("fk_date_filter");
            });

            modelBuilder.Entity<MtdFilterOwner>(entity =>
            {
                entity.ToTable("mtd_filter_owner");

                entity.HasIndex(e => e.Id)
                    .HasDatabaseName("id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OwnerId)
                    .IsRequired()
                    .HasColumnName("owner_id")
                    .HasColumnType("varchar(36)");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.MtdFilterOwner)
                    .HasForeignKey<MtdFilterOwner>(d => d.Id)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_owner_filter");
            });


            modelBuilder.Entity<MtdFilterField>(entity =>
            {
                entity.ToTable("mtd_filter_field");

                entity.HasIndex(e => e.Id)
                    .HasDatabaseName("id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.MtdFilter)
                    .HasDatabaseName("mtd_filter_idx");

                entity.HasIndex(e => e.MtdFormPartFieldId)
                    .HasDatabaseName("mtd_filter_field_mtd_form_field_idx");

                entity.HasIndex(e => e.MtdTerm)
                    .HasDatabaseName("mtd_filter_field_term_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.MtdFilter)
                    .HasColumnName("mtd_filter")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MtdFormPartFieldId)
                    .IsRequired()
                    .HasColumnName("mtd_form_part_field")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.MtdTerm)
                    .HasColumnName("mtd_term")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnName("value")
                    .HasColumnType("varchar(256)");

                entity.Property(e => e.ValueExtra)
                    .HasColumnName("value_extra")
                    .HasColumnType("varchar(256)");

                entity.HasOne(d => d.MtdFilterNavigation)
                    .WithMany(p => p.MtdFilterFields)
                    .HasForeignKey(d => d.MtdFilter)
                    .HasConstraintName("mtd_filter_field_mtd_field");

                entity.HasOne(d => d.MtdFormPartFieldNavigation)
                    .WithMany(p => p.MtdFilterField)
                    .HasForeignKey(d => d.MtdFormPartFieldId)
                    .HasConstraintName("mtd_filter_field_mtd_form_field");

                entity.HasOne(d => d.MtdTermNavigation)
                    .WithMany(p => p.MtdFilterField)
                    .HasForeignKey(d => d.MtdTerm)
                    .HasConstraintName("mtd_filter_field_mtd_term");
            });

            modelBuilder.Entity<MtdFilterScript>(entity =>
            {
                entity.ToTable("mtd_filter_script");

                entity.HasIndex(e => e.Id)
                    .HasDatabaseName("id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.MtdFormId)
                    .HasDatabaseName("fk_script_filter_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MtdFormId)
                    .IsRequired()
                    .HasColumnName("mtd_form_id")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(256)");


                entity.Property(e => e.Script)
                    .IsRequired()
                    .HasColumnName("script")
                    .HasColumnType("longtext");

                entity.HasOne(d => d.MtdForm)
                    .WithMany(p => p.MtdFilterScript)
                    .HasForeignKey(d => d.MtdFormId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_script_filter");
            });

            modelBuilder.Entity<MtdFilterScriptApply>(entity =>
            {
                entity.ToTable("mtd_filter_script_apply");

                entity.HasIndex(e => e.Id)
                    .HasDatabaseName("id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.MtdFilterId)
                    .HasDatabaseName("fk_script_filter_apply1_idx");

                entity.HasIndex(e => e.MtdFilterScriptId)
                    .HasDatabaseName("fk_script_filter_apply2_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(36)");

                entity.Property(e => e.MtdFilterId)
                    .IsRequired()
                    .HasColumnName("mtd_filter_id")
                    .HasColumnType("int(11)");


                entity.Property(e => e.MtdFilterScriptId)
                    .IsRequired()
                    .HasColumnName("mtd_filter_script_id")
                    .HasColumnType("int(11)");


                entity.HasOne(d => d.MtdFilter)
                    .WithMany(p => p.MtdFilterScriptApply)
                    .HasForeignKey(d => d.MtdFilterId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_script_filter_apply1");

                entity.HasOne(d => d.MtdFilterScript)
                    .WithMany(p => p.MtdFilterScriptApply)
                    .HasForeignKey(d => d.MtdFilterScriptId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_script_filter_apply2");

            });
        }

    }
}
