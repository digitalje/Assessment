using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PSUCourse.API.Models
{
    public partial class PendulumStateContext : DbContext
    {
        public PendulumStateContext()
        {
        }

        public PendulumStateContext(DbContextOptions<PendulumStateContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Course> Course { get; set; }
        public virtual DbSet<CourseProfessor> CourseProfessor { get; set; }
        public virtual DbSet<Professor> Professor { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("server=duffphelps.database.windows.net;database=PendulumState;user Id=adminDP; password=Mtrx02@221997;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasIndex(e => e.CourseName)
                    .HasName("UC_Course")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CourseName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ProfessorId).HasColumnName("ProfessorID");

                entity.HasOne(d => d.Professor)
                    .WithMany(p => p.Course)
                    .HasForeignKey(d => d.ProfessorId)
                    .HasConstraintName("FK_ProfessorCourse");
            });

            modelBuilder.Entity<CourseProfessor>(entity =>
            {
                entity.HasIndex(e => e.CourseName)
                    .HasName("UC_CourseProfessor")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CourseName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ProfessorEmail).HasMaxLength(250);

                entity.Property(e => e.ProfessorName).HasMaxLength(200);
            });

            modelBuilder.Entity<Professor>(entity =>
            {
                entity.HasIndex(e => e.ProfessorName)
                    .HasName("UC_Professor")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Email).HasMaxLength(200);

                entity.Property(e => e.ProfessorName)
                    .IsRequired()
                    .HasMaxLength(200);
            });
        }
    }
}
