using E_Commerce.Common.Application.Abstractions;
using E_Commerce.Common.Application.Services;
using E_Commerce.CustomerManagement.Application.Interfaces;
using E_Commerce.CustomerManagement.Application.Handlers;
using E_Commerce.CustomerManagement.Infrastructure.Persistence;
using E_Commerce.CustomerManagement.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using E_Commerce.Common.Persistence.Extensions;
using E_Commerce.Common.Messaging.Extensions;
using E_Commerce.CustomerManagement.Application.Commands;
using E_Commerce.CustomerManagement.Application.DTOs;
using E_Commerce.CustomerManagement.Application.EventHandlers;
using E_Commerce.CustomerManagement.Application.Queries;
using E_Commerce.CustomerManagement.Domain.Events;
using E_Commerce.CustomerManagement.Domain.ValueObjects;

namespace E_Commerce.CustomerManagement.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomerManagementInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Common Persistence
        services.AddCommonPersistence();
        
        // Database
        services.AddSqlServerDatabase<CustomerDbContext>(configuration);
        
        // Unit of Work
        services.AddUnitOfWork<CustomerDbContext>();

        // Application Services
        services.AddScoped<ICommandDispatcher, CommandDispatcher>();
        services.AddScoped<IQueryDispatcher, QueryDispatcher>();
        services.AddScoped<IDomainEventPublisher, DomainEventPublisher>();
        
        // Message Broker
        services.AddRabbitMqMessaging(configuration);

        // Repositories
        services.AddScoped<ICustomerRepository, CustomerRepository>();

        // Command Handlers
        services.AddScoped<ICommandHandler<CreateCustomerCommand, CustomerId>, CreateCustomerCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateCustomerCommand>, UpdateCustomerCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteCustomerCommand>, DeleteCustomerCommandHandler>();
        services.AddScoped<ICommandHandler<AddCustomerAddressCommand, Guid>, AddCustomerAddressCommandHandler>();

        // Query Handlers  
        services.AddScoped<IQueryHandler<GetCustomersQuery, List<CustomerResponse>>, GetCustomersQueryHandler>();
        services.AddScoped<IQueryHandler<GetCustomerByIdQuery, CustomerResponse>, GetCustomerByIdQueryHandler>();
        services.AddScoped<IQueryHandler<GetCustomerAddressesQuery, List<AddressResponse>>, GetCustomerAddressesQueryHandler>();

        // Domain Event Handlers
        services.AddScoped<IDomainEventHandler<CustomerCreatedEvent>, CustomerCreatedEventHandler>();
        services.AddScoped<IDomainEventHandler<CustomerEmailChangedEvent>, CustomerEmailChangedEventHandler>();
        services.AddScoped<IDomainEventHandler<CustomerStatusChangedEvent>, CustomerStatusChangedEventHandler>();
        services.AddScoped<IDomainEventHandler<CustomerEmailVerifiedEvent>, CustomerEmailVerifiedEventHandler>();
        services.AddScoped<IDomainEventHandler<CustomerBlockedEvent>, CustomerBlockedEventHandler>();

        // Health Checks
        services.AddDatabaseHealthCheck(configuration, "DefaultConnection", "customer-database");

        return services;
    }
}
