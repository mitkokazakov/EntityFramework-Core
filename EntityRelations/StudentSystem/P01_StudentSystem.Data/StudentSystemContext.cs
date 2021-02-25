using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext()
        {

        }

        public StudentSystemContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Homework> HomeworkSubmissions { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=StudentSystem;Integrated Security=True;");
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(x => x.Name)
                .IsUnicode(true);

                entity.Property(x => x.PhoneNumber)
                .IsUnicode(false)
                .IsRequired(false);

                entity.Property(x => x.Birthday)
                .IsRequired(false);


            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(x => x.Name).IsUnicode(true);

                entity.Property(x => x.Description)
                .IsRequired(false)
                .IsUnicode(true);
            });

            modelBuilder.Entity<Resource>(entity =>
            {
                entity.Property(x => x.Name).IsUnicode(true);

                entity.Property(x => x.Url).IsUnicode(false);
            });

            modelBuilder.Entity<Homework>(entity =>
            {
                entity.Property(x => x.Content).IsUnicode(false);

            });

            modelBuilder.Entity<StudentCourse>(entity =>
            {
                entity.HasKey(x => new { x.StudentId, x.CourseId });
            });
        }
    }
}
