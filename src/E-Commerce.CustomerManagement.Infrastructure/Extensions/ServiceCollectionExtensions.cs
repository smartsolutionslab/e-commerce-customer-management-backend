using E_Commerce.Common.Infrastructure.Services;
using E_Commerce.Common.Infrastructure.Messaging;
using E_Commerce.CustomerManagement.Application.Interfaces;
using E_Commerce.CustomerManagement.Infrastructure.Persistence;
using E_Commerce.CustomerManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace E_Commerce.CustomerManagement.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomerManagementInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<CustomerDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Infrastructure Services
        services.AddScoped<ITenantService, TenantService>();
        
        // Message Broker with IOptions
        services.Configure<MessageBrokerConfig>(configuration.GetSection("MessageBroker"));
        services.AddSingleton<IMessageBroker, RabbitMqMessageBroker>();

        // Repositories
        services.AddScoped<ICustomerRepository, CustomerRepository>();

        // MediatR for Application layer
        services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(typeof(E_Commerce.CustomerManagement.Application.Commands.CreateCustomerCommand).Assembly));

        // Health Checks
        services.AddHealthChecks()
            .AddSqlServer(
                configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found."),
                name: "customer-database");

        return services;
    }
}
