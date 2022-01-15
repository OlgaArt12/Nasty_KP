using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Uspevaemost.Models;

#nullable disable

namespace Uspevaemost.Data
{
    public partial class UspevaemostContext : DbContext
    {
        public UspevaemostContext()
        {
        }

        public UspevaemostContext(DbContextOptions<UspevaemostContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Academgroup> Academgroups { get; set; }
        public virtual DbSet<StudPlan> StudPlans { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<Vedomost> Vedomosts { get; set; }
        public virtual DbSet<ZaprosTwo> ZaprosTwos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Academgroup>(entity =>
            {
                entity.HasKey(e => e.GroupId)
                    .HasName("PK_GroupID");

                entity.Property(e => e.Shifr).IsUnicode(false);
            });

            modelBuilder.Entity<StudPlan>(entity =>
            {
                entity.HasKey(e => e.PlanId)
                    .HasName("PK_PlanID");

                entity.Property(e => e.Attestation).IsUnicode(false);

                entity.HasOne(d => d.IdGroupNavigation)
                    .WithMany(p => p.StudPlans)
                    .HasForeignKey(d => d.IdGroup)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Plan_Academgroup");

                entity.HasOne(d => d.NameSubjectNavigation)
                    .WithMany(p => p.StudPlans)
                    .HasForeignKey(d => d.NameSubject)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Plan_Subject");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.ZachetkaId)
                    .HasName("PK_ZachetkaID");

                entity.Property(e => e.DateStud).HasDefaultValueSql("('There is no birthday')");

                entity.Property(e => e.Fio).IsUnicode(false);

                entity.HasOne(d => d.IdgroupNavigation)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.Idgroup)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Stud_Academgroup");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.HasKey(e => e.IdnameSubject)
                    .HasName("PK_IDNameSubject");

                entity.Property(e => e.Fioprepod).IsUnicode(false);

                entity.Property(e => e.NameSubject).IsUnicode(false);
            });

            modelBuilder.Entity<Vedomost>(entity =>
            {
                entity.HasKey(e => e.Idvedomost)
                    .HasName("PK_IDVedomost");

                entity.Property(e => e.Mark)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('неуд')");

                entity.HasOne(d => d.IdZachNavigation)
                    .WithMany(p => p.Vedomosts)
                    .HasForeignKey(d => d.IdZach)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Vedomost_Student");

                entity.HasOne(d => d.NameSubject)
                    .WithMany(p => p.Vedomosts)
                    .HasForeignKey(d => d.NameSubjectId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Vedomost_Subject");
            });

            modelBuilder.Entity<ZaprosTwo>(entity =>
            {
                entity.ToView("ZaprosTwo");

                entity.Property(e => e.Fio).IsUnicode(false);

                entity.Property(e => e.Mark).IsUnicode(false);

                entity.Property(e => e.NameSubject).IsUnicode(false);

                entity.Property(e => e.Shifr).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
