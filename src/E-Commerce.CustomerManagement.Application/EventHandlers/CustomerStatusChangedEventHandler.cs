using E_Commerce.Common.Application.Services;
using E_Commerce.CustomerManagement.Domain.Events;
using Microsoft.Extensions.Logging;

namespace E_Commerce.CustomerManagement.Application.EventHandlers;

public class CustomerStatusChangedEventHandler(ILogger<CustomerStatusChangedEventHandler> logger)
    : IDomainEventHandler<CustomerStatusChangedEvent>
{
    public async Task HandleAsync(CustomerStatusChangedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Customer status changed: {CustomerId} from {OldStatus} to {NewStatus}", 
            domainEvent.CustomerId, domainEvent.OldStatus, domainEvent.NewStatus);

        // TODO: Notify other services, update external systems, send notifications
        // For example: if status changed to Blocked, disable access in authentication service
        
        await Task.CompletedTask;
    }
}
