using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FlowModelDesktop.Models
{
    public partial class FlowModelDbContext : DbContext
    {
        public FlowModelDbContext()
        {
        }

        public FlowModelDbContext(DbContextOptions<FlowModelDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Material> Materials { get; set; } = null!;
        public virtual DbSet<Measure> Measures { get; set; } = null!;
        public virtual DbSet<Parameter> Parameters { get; set; } = null!;
        public virtual DbSet<ParameterValue> ParameterValues { get; set; } = null!;
        public virtual DbSet<TypeParameter> TypeParameters { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=FlowModelDb.db;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Material>(entity =>
            {
                entity.ToTable("Material");

                entity.HasIndex(e => e.Id, "IX_Material_Id")
                    .IsUnique();
            });

            modelBuilder.Entity<Measure>(entity =>
            {
                entity.ToTable("Measure");

                entity.HasIndex(e => e.Id, "IX_Measure_Id")
                    .IsUnique();
            });

            modelBuilder.Entity<Parameter>(entity =>
            {
                entity.ToTable("Parameter");

                entity.HasIndex(e => e.Id, "IX_Parameter_Id")
                    .IsUnique();

                entity.HasOne(d => d.IdMeasureNavigation)
                    .WithMany(p => p.Parameters)
                    .HasForeignKey(d => d.IdMeasure);

                entity.HasOne(d => d.IdTypeParamNavigation)
                    .WithMany(p => p.Parameters)
                    .HasForeignKey(d => d.IdTypeParam)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ParameterValue>(entity =>
            {
                entity.HasKey(e => new { e.IdMat, e.IdParam });

                entity.ToTable("ParameterValue");

                entity.HasOne(d => d.IdMatNavigation)
                    .WithMany(p => p.ParameterValues)
                    .HasForeignKey(d => d.IdMat)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.IdParamNavigation)
                    .WithMany(p => p.ParameterValues)
                    .HasForeignKey(d => d.IdParam)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<TypeParameter>(entity =>
            {
                entity.ToTable("TypeParameter");

                entity.HasIndex(e => e.Id, "IX_TypeParameter_Id")
                    .IsUnique();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
