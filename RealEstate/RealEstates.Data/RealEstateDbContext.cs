using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RealEstates.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstates.Data
{
    public class RealEstateDbContext : DbContext
    {
        public RealEstateDbContext()
        {

        }
        public RealEstateDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Estate> Properties { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<TypeBuilding> TypesBuilding { get; set; }
        public DbSet<TypeProperty> TypesProperty { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=RealEstate;Integrated Security=True;");
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PropertiesTags>(entity =>
            {
                entity.HasKey(x => new { x.TagId, x.PropertyId });
            });
        }
    }
}
