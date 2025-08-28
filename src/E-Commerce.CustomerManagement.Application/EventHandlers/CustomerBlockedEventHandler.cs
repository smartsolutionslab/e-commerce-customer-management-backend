using E_Commerce.Common.Application.Services;
using E_Commerce.CustomerManagement.Domain.Events;
using Microsoft.Extensions.Logging;

namespace E_Commerce.CustomerManagement.Application.EventHandlers;

public class CustomerBlockedEventHandler(ILogger<CustomerBlockedEventHandler> logger)
    : IDomainEventHandler<CustomerBlockedEvent>
{
    public async Task HandleAsync(CustomerBlockedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        logger.LogWarning("Customer blocked: {CustomerId} - Reason: {Reason}", 
            domainEvent.CustomerId, domainEvent.Reason);

        // TODO: Revoke authentication tokens, notify security team, log incident
        await Task.CompletedTask;
    }
}
