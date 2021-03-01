using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.Data.Models;

namespace P01_HospitalDatabase.Data
{
    public class HospitalContext : DbContext
    {
        public HospitalContext()
        {

        }

        public HospitalContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Visitation> Visitations { get; set; }
        public DbSet<Diagnose> Diagnoses { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<PatientMedicament> PatientMedicaments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=HospitalDatabase;Integrated Security=True;");
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.Property(x => x.FirstName)
                .IsUnicode(true);

                entity.Property(x => x.LastName)
                .IsUnicode(true);

                entity.Property(x => x.Email)
                .IsUnicode(true);

                entity.Property(x => x.Address)
                .IsUnicode(true);

            });

            modelBuilder.Entity<Visitation>(entity =>
            {
                entity.Property(x => x.Comments)
                .IsUnicode(true);
            });

            modelBuilder.Entity<Diagnose>(entity =>
            {
                entity.Property(x => x.Name)
                .IsUnicode(true);

                entity.Property(x => x.Comments)
                .IsUnicode(true);
            });

            modelBuilder.Entity<Medicament>(entity =>
            {
                entity.Property(x => x.Name)
                .IsUnicode(true);
            });

            modelBuilder.Entity<PatientMedicament>(entity =>
            {
                entity.HasKey(x => new { x.PatientId, x.MedicamentId });
            });
        }

    }
}
