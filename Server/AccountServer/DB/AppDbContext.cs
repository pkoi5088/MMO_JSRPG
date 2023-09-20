using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace AccountServer.DB
{
    public class AppDbContext : DbContext
    {
        public DbSet<AccountDb> Accounts { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AccountDb>()
                .HasIndex(a => a.AccountName)
                .IsUnique();
        }
    }
}
