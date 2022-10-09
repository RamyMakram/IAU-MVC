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
        public virtual DbSet<DomainEmail> DomainEmails { get; set; }
        public virtual DbSet<DomainInfo> DomainInfos { get; set; }
        public virtual DbSet<DomainStyle> DomainStyles { get; set; }
        public virtual DbSet<SubDomain> SubDomains { get; set; }
        public virtual DbSet<TasahelHomeSetting> TasahelHomeSettings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=LAPTOP-CN4OLI1Q\\SQL19;Database=Tasahel;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<About>(entity =>
            {
                entity.ToTable("About");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DescAr).IsRequired();

                entity.Property(e => e.DescEn).IsRequired();

                entity.Property(e => e.DomainId).HasColumnName("DomainID");

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

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ConnectionString).IsRequired();

                entity.Property(e => e.Domain1)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasColumnName("Domain");

                entity.Property(e => e.DomainMachineId).HasColumnName("DomainMachineID");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<DomainEmail>(entity =>
            {
                entity.HasKey(e => e.DomainId);

                entity.ToTable("DomainEmail");

                entity.Property(e => e.DomainId)
                    .ValueGeneratedNever()
                    .HasColumnName("DomainID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.MessageAppSid).HasMaxLength(300);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Sender).HasMaxLength(250);

                entity.Property(e => e.Smtp)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("SMTP");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.Domain)
                    .WithOne(p => p.DomainEmail)
                    .HasForeignKey<DomainEmail>(d => d.DomainId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DomainEmail_Domain");
            });

            modelBuilder.Entity<DomainInfo>(entity =>
            {
                entity.HasKey(e => e.DomainId)
                    .HasName("PK_DomainInfo_1");

                entity.ToTable("DomainInfo");

                entity.Property(e => e.DomainId)
                    .ValueGeneratedNever()
                    .HasColumnName("DomainID");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.HasOne(d => d.Domain)
                    .WithOne(p => p.DomainInfo)
                    .HasForeignKey<DomainInfo>(d => d.DomainId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DomainInfo_Domain");
            });

            modelBuilder.Entity<DomainStyle>(entity =>
            {
                entity.HasKey(e => e.DomainId)
                    .HasName("PK_DomainStyle_1");

                entity.ToTable("DomainStyle");

                entity.Property(e => e.DomainId)
                    .ValueGeneratedNever()
                    .HasColumnName("DomainID");

                entity.Property(e => e.Favicon).IsRequired();

                entity.Property(e => e.Icon).IsRequired();

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Maincolor)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.MetaDesc).IsRequired();

                entity.Property(e => e.MetaKeyword).IsRequired();

                entity.Property(e => e.Secondcolor)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Thirdcolor)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Domain)
                    .WithOne(p => p.DomainStyle)
                    .HasForeignKey<DomainStyle>(d => d.DomainId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DomainStyle_Domain");
            });

            modelBuilder.Entity<SubDomain>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Domain)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.DomainId).HasColumnName("DomainID");

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UseHttps)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.DomainNavigation)
                    .WithMany(p => p.SubDomains)
                    .HasForeignKey(d => d.DomainId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubDomains_Domain");
            });

            modelBuilder.Entity<TasahelHomeSetting>(entity =>
            {
                entity.HasKey(e => e.DomainId)
                    .HasName("PK_TasahelHomeSetting_1");

                entity.ToTable("TasahelHomeSetting");

                entity.Property(e => e.DomainId)
                    .ValueGeneratedNever()
                    .HasColumnName("DomainID");

                entity.Property(e => e.FollowIco).IsRequired();

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.NewReqIco)
                    .IsRequired()
                    .HasColumnName("NewReqICo");

                entity.HasOne(d => d.Domain)
                    .WithOne(p => p.TasahelHomeSetting)
                    .HasForeignKey<TasahelHomeSetting>(d => d.DomainId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TasahelHomeSetting_Domain");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
