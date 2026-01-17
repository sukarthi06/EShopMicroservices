using Ordering.Application.Data;
using System.Reflection;

namespace Ordering.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
    : DbContext(options),IApplicationDbContext
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    public IReadOnlyList<IAggregate> GetAggregatesWithDomainEvents()
    {
        return ChangeTracker
           .Entries<IAggregate>()
           .Where(e => e.Entity.DomainEvents.Any())
           .Select(e => e.Entity)
           .ToList();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}
