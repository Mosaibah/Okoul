using Microsoft.EntityFrameworkCore;

namespace Okoul.Models
{
    public class OkoulContext : DbContext
    {
        public OkoulContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Author> Author { get; set; }
        public DbSet<Quote> Quote { get; set; }
    }
}
