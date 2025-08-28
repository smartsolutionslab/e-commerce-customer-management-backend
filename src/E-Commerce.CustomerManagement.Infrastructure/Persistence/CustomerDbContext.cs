using E_Commerce.Common.Persistence.DbContext;
using E_Commerce.Common.Persistence.Services;
using E_Commerce.Common.Messaging.Abstractions;
using E_Commerce.Common.Application.Services;
using E_Commerce.CustomerManagement.Domain.Entities;
using E_Commerce.CustomerManagement.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.CustomerManagement.Infrastructure.Persistence;

public class CustomerDbContext : BaseDbContext
{
    public DbSet<Customer> Customers => Set<Customer>();

    public CustomerDbContext(
        DbContextOptions<CustomerDbContext> options, 
        ITenantService tenantService, 
        IDomainEventPublisher domainEventPublisher,
        IMessageBroker messageBroker)
        : base(options, tenantService, domainEventPublisher, messageBroker)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
    }
}