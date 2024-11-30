using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Putni_Troskovi_App.Model
{
    internal class AppDbContext : DbContext
    {
        public DbSet<Dan> Dani { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite("Data Source=PutniTroskoviDB.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dan>().HasData(
                new Dan { Id = 1, Datum = DateTime.Parse("2024-11-01") },
                new Dan { Id = 2, Datum = DateTime.Parse("2024-11-04") },
                new Dan { Id = 3, Datum = DateTime.Parse("2024-11-05") },
                new Dan { Id = 4, Datum = DateTime.Parse("2024-11-06") },
                new Dan { Id = 5, Datum = DateTime.Parse("2024-11-07") },
                new Dan { Id = 6, Datum = DateTime.Parse("2024-11-08") },
                new Dan { Id = 7, Datum = DateTime.Parse("2024-11-11") },
                new Dan { Id = 8, Datum = DateTime.Parse("2024-11-12") },
                new Dan { Id = 9, Datum = DateTime.Parse("2024-11-14") },
                new Dan { Id = 10, Datum = DateTime.Parse("2024-11-15") },
                new Dan { Id = 11, Datum = DateTime.Parse("2024-11-18") },
                new Dan { Id = 12, Datum = DateTime.Parse("2024-11-19") }
                );
        }
    }
}
