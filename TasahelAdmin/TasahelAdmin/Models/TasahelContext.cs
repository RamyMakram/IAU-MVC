using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace TasahelAdmin.Models
{
    public partial class TasahelContext : DbContext
    {
        public TasahelContext()
        {
        }

        public TasahelContext(DbContextOptions<TasahelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<About> Abouts { get; set; }
        public virtual DbSet<Domain> Domains { get; set; }
        public virtual DbSet<DomainInfo> DomainInfos { get; set; }
        public virtual DbSet<TasahelHomeSetting> TasahelHomeSettings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=LAPTOP-CN4OLI1Q\\SQL19;Database=Tasahel;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<About>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.DomainId });

                entity.ToTable("About");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.DomainId).HasColumnName("DomainID");

                entity.Property(e => e.DescAr).IsRequired();

                entity.Property(e => e.DescEn).IsRequired();

                entity.Property(e => e.TitleAr).IsRequired();

                entity.Property(e => e.TitleEn).IsRequired();

                entity.HasOne(d => d.Domain)
                    .WithMany(p => p.Abouts)
                    .HasForeignKey(d => d.DomainId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_About_Domain");
            });

            modelBuilder.Entity<Domain>(entity =>
            {
                entity.ToTable("Domain");

                entity.HasIndex(e => e.Domain1, "IX_Domain")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.ConnectionString).IsRequired();

                entity.Property(e => e.Domain1)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasColumnName("Domain");

                entity.Property(e => e.DomainKey).IsRequired();

                entity.Property(e => e.DomainMachineId)
                    .IsRequired()
                    .HasColumnName("DomainMachineID");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.Favicon).IsRequired();

                entity.Property(e => e.Icon).IsRequired();

                entity.Property(e => e.Maincolor)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.MetaDesc).IsRequired();

                entity.Property(e => e.MetaKeyword).IsRequired();

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Secondcolor)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Thirdcolor)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<DomainInfo>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.DomainId });

                entity.ToTable("DomainInfo");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.DomainId).HasColumnName("DomainID");

                entity.HasOne(d => d.Domain)
                    .WithMany(p => p.DomainInfos)
                    .HasForeignKey(d => d.DomainId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DomainInfo_Domain");
            });

            modelBuilder.Entity<TasahelHomeSetting>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.DomainId });

                entity.ToTable("TasahelHomeSetting");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.DomainId).HasColumnName("DomainID");

                entity.Property(e => e.FollowIco).IsRequired();

                entity.Property(e => e.NewReqIco)
                    .IsRequired()
                    .HasColumnName("NewReqICo");

                entity.HasOne(d => d.Domain)
                    .WithMany(p => p.TasahelHomeSettings)
                    .HasForeignKey(d => d.DomainId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TasahelHomeSetting_Domain");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
