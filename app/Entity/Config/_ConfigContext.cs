using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Entity
{
    public partial class OrderMakerContext : DbContext
    {
        public virtual DbSet<MtdConfigFile> MtdConfigFiles { get; set; }
        public virtual DbSet<MtdConfigParam> MtdConfigParam { get; set; }

        public void ConfigModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.Entity<MtdConfigFile>(entity =>
            {
                entity.ToTable("mtd_config_file");

                entity.HasIndex(e => e.Id)
                    .HasDatabaseName("id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FileData)
                    .IsRequired()
                    .HasColumnName("file_data")
                    .HasColumnType("mediumblob");

                entity.Property(e => e.FileSize)
                    .IsRequired()
                    .HasColumnName("file_size")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.FileType)
                    .IsRequired()
                    .HasColumnName("file_type")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(45)");
            });

            modelBuilder.Entity<MtdConfigParam>(entity =>
            {
                entity.ToTable("mtd_config_param");

                entity.HasIndex(e => e.Id)
                    .HasDatabaseName("id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasColumnType("longtext");
            });

        }

    }
}
