using Accounts.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Accounts.Infrastructure.DbContexts;

internal class AccountDbContext(DbContextOptions<AccountDbContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<ConversionRate> ConversionRates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>()
            .HasQueryFilter(x => !x.IsSoftDeleted);

        modelBuilder.Entity<Wallet>()
            .HasQueryFilter(x => !x.IsSoftDeleted);

        base.OnModelCreating(modelBuilder);
    }
}
