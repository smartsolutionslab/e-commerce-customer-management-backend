using E_Commerce.Common.Application.Services;
using E_Commerce.CustomerManagement.Domain.Events;
using Microsoft.Extensions.Logging;

namespace E_Commerce.CustomerManagement.Application.EventHandlers;

public class CustomerEmailVerifiedEventHandler(ILogger<CustomerEmailVerifiedEventHandler> logger)
    : IDomainEventHandler<CustomerEmailVerifiedEvent>
{
    public async Task HandleAsync(CustomerEmailVerifiedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Customer email verified: {CustomerId} - {Email}", 
            domainEvent.CustomerId, domainEvent.Email);

        // TODO: Enable full account features, send confirmation email, update external systems
        await Task.CompletedTask;
    }
}

 