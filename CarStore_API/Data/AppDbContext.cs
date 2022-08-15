using CarStore_API.Model;
using Microsoft.EntityFrameworkCore;

namespace CarStore_API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<Car> Cars { get; set; }
    }
}
