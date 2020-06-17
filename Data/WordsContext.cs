using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Diagnostics;
using System.Text;


namespace App.Data
{
    [Table("wordsList_tbl")]
    public class WordsContext : DbContext
    {
        public DbSet<Word> Words { get; set; }

        IConfiguration _configuration;

        public WordsContext(IConfiguration configuration)
        {
            _configuration = configuration;
            Database.EnsureCreated(); // if ensured - comment
        }

        public WordsContext()
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Word>()
                        .HasKey(prop => prop.Id)
                        .HasName("Id");

            modelBuilder.Entity<Word>()
                        .Property(prop => prop.Value)
                        .IsRequired(true);

            modelBuilder.Entity<Word>()
                        .Property(prop => prop.Frequency)
                        .IsRequired(true);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration["connectionString"]);
        }
    }
}
