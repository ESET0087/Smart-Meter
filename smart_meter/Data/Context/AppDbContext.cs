using Microsoft.EntityFrameworkCore;
using smart_meter.Data.Entities;

namespace smart_meter.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Consumer> Consumer { get; set; }
    }
}
