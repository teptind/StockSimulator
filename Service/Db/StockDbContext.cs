using Microsoft.EntityFrameworkCore;
using ProductsCounting.Service.Db.Entities;

namespace ProductsCounting.Service.Db
{
    public sealed class StockDbContext : DbContext
    {
        public DbSet<ProductEntity> Stock { get; set; }

        public StockDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=productdb;Trusted_Connection=True;");
        }
    }
}