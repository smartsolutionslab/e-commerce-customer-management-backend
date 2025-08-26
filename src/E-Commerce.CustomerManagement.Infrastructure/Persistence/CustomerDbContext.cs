using E_Commerce.Common.Infrastructure.Persistence;
using E_Commerce.Common.Infrastructure.Services;
using E_Commerce.Common.Infrastructure.Messaging;
using E_Commerce.CustomerManagement.Domain.Entities;
using E_Commerce.CustomerManagement.Infrastructure.Persistence.Configurations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.CustomerManagement.Infrastructure.Persistence;

public class CustomerDbContext : BaseDbContext
{
    public DbSet<Customer> Customers => Set<Customer>();

    public CustomerDbContext(
        DbContextOptions<CustomerDbContext> options, 
        ITenantService tenantService, 
        IPublisher publisher,
        IMessageBroker messageBroker)
        : base(options, tenantService, publisher, messageBroker)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
    }
}