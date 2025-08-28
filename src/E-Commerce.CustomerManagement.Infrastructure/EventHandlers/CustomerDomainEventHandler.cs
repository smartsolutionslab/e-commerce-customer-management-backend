using E_Commerce.Common.Application.Services;
using E_Commerce.Common.Messaging.Abstractions;
using E_Commerce.CustomerManagement.Domain.Events;
using Microsoft.Extensions.Logging;

namespace E_Commerce.CustomerManagement.Infrastructure.EventHandlers;

public class CustomerCreatedEventHandler : IDomainEventHandler<CustomerCreatedEvent>
{
    private readonly IMessageBroker _messageBroker;
    private readonly ILogger<CustomerCreatedEventHandler> _logger;

    public CustomerCreatedEventHandler(IMessageBroker messageBroker, ILogger<CustomerCreatedEventHandler> logger)
    {
        _messageBroker = messageBroker;
        _logger = logger;
    }

    public async Task HandleAsync(CustomerCreatedEvent notification, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Publishing customer created event for customer {CustomerId}", notification.CustomerId);

        var integrationEvent = new CustomerCreatedIntegrationEvent(
            notification.CustomerId.Value,
            notification.TenantId.Value,
            notification.Email.Value,
            notification.FullName.FirstName,
            notification.FullName.LastName);

        await _messageBroker.PublishAsync(integrationEvent, "integration.events", "customer.created", cancellationToken);
    }
}