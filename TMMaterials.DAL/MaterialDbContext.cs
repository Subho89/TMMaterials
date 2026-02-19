using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TMMaterials.DAL.Model;

namespace TMMaterials.DAL
{
    public class MaterialDbContext : DbContext
    {
        // 1. Core Metadata Tables
        public DbSet<tblMain> tblMain { get; set; }
        public DbSet<tblCollections> tblCollections { get; set; }
        public DbSet<tblStandards> tblStandards { get; set; }
        public DbSet<tblMaterialsLibrary> tblMaterialsLibrary { get; set; }

        // 2. The Relationship & Dynamic Data Tables
        public DbSet<tblCollectionStandards> tblCollectionStandards { get; set; }
        public DbSet<tblCollectionProperties> tblCollectionProperties { get; set; }
        public DbSet<tblCollectionPropertiesValues> tblCollectionPropertiesValues { get; set; }
        public DbSet<tblMaterialTypes> tblMaterialTypes { get; set; }

        public MaterialDbContext()
        {
            // This will create the database and all tables if they don't exist
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Connects to a local SQLite database file
            optionsBuilder.UseSqlite("Data Source=dbMaterialLibrary.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ensure unique constraints to prevent duplicate imports
            modelBuilder.Entity<tblMain>()
                .HasIndex(m => m.RegionName)
                .IsUnique();

            modelBuilder.Entity<tblCollectionProperties>()
                .HasIndex(p => p.PropertyName)
                .IsUnique();

            // Configure shadow foreign keys if needed, 
            // though EF will handle these automatically based on your list properties.

            base.OnModelCreating(modelBuilder);
        }
    }
}
