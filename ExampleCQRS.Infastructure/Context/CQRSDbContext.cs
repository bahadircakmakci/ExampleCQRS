using ExampleCQRS.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleCQRS.Infastructure.Context
{
    public class CQRSDbContext : DbContext
    {
        public CQRSDbContext()
        {

        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Encyclopedia> Encyclopedias { get; set; }
        public CQRSDbContext(DbContextOptions<CQRSDbContext> options) : base(options)
        {            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {            
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
