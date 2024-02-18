using Microsoft.EntityFrameworkCore;
using Users.Infrastructure.Entities;

namespace Users.Infrastructure.DbContexts;

internal class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers { get; set; }
}
