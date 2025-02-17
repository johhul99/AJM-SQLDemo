using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace SQL_Demo.Models
{
    public class SnusDBContext : DbContext
    {
        public SnusDBContext(DbContextOptions<SnusDBContext> options) : base(options) { }
        public DbSet<Snus> Snus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
