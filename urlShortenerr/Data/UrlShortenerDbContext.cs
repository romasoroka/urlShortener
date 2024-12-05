using Microsoft.EntityFrameworkCore;

namespace urlShortener.Data
{
    public class UrlShortenerDbContext : DbContext
    {
        public DbSet<UrlRecord> UrlRecords { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; } 

        public UrlShortenerDbContext(DbContextOptions<UrlShortenerDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
