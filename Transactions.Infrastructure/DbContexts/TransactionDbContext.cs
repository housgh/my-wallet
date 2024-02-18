using Microsoft.EntityFrameworkCore;
using Transactions.Infrastructure.Entities;

namespace Transactions.Infrastructure.DbContexts;

internal class TransactionDbContext(DbContextOptions<TransactionDbContext> options) : DbContext(options)
{
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>()
            .HasQueryFilter(x => !x.IsSoftDeleted);
        base.OnModelCreating(modelBuilder);
    }
}
