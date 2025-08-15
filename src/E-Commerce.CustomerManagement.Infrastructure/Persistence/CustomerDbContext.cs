using E_Commerce.Common.Infrastructure.Persistence;
using E_Commerce.Common.Infrastructure.Services;
using E_Commerce.CustomerManagement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.CustomerManagement.Infrastructure.Persistence;

public class CustomerDbContext : BaseDbContext
{
    public CustomerDbContext(DbContextOptions<CustomerDbContext> options, ITenantService tenantService, IPublisher publisher)
        : base(options, tenantService, publisher)
    {
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Address> Addresses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
