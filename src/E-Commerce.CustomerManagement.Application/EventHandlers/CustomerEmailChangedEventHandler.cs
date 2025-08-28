using E_Commerce.Common.Application.Services;
using E_Commerce.CustomerManagement.Domain.Events;
using Microsoft.Extensions.Logging;

namespace E_Commerce.CustomerManagement.Application.EventHandlers;

public class CustomerEmailChangedEventHandler(ILogger<CustomerEmailChangedEventHandler> logger)
    : IDomainEventHandler<CustomerEmailChangedEvent>
{
    public async Task HandleAsync(CustomerEmailChangedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Customer email changed: {CustomerId} - {NewEmail}", 
            domainEvent.CustomerId, domainEvent.NewEmail);

        // TODO: Update email in external systems, send verification email, etc.
        await Task.CompletedTask;
    }
}