using E_Commerce.Common.Application.Services;
using E_Commerce.CustomerManagement.Domain.Events;
using Microsoft.Extensions.Logging;

namespace E_Commerce.CustomerManagement.Application.EventHandlers;

public class CustomerCreatedEventHandler(ILogger<CustomerCreatedEventHandler> logger)
    : IDomainEventHandler<CustomerCreatedEvent>
{
    public async Task HandleAsync(CustomerCreatedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Customer created: {CustomerId} - {Email}", 
            domainEvent.CustomerId, domainEvent.Email);

        // TODO: Send welcome email, create customer profile in external systems
        await Task.CompletedTask;
    }
}