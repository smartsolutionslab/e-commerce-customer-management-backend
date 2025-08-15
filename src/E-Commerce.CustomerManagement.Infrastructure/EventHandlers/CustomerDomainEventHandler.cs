using E_Commerce.Common.Infrastructure.Messaging;
using E_Commerce.CustomerManagement.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace E_Commerce.CustomerManagement.Infrastructure.EventHandlers;

public class CustomerCreatedEventHandler : INotificationHandler<CustomerCreatedEvent>
{
    private readonly IMessageBroker _messageBroker;
    private readonly ILogger<CustomerCreatedEventHandler> _logger;

    public CustomerCreatedEventHandler(IMessageBroker messageBroker, ILogger<CustomerCreatedEventHandler> logger)
    {
        _messageBroker = messageBroker;
        _logger = logger;
    }

    public async Task Handle(CustomerCreatedEvent notification, CancellationToken cancellationToken)
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

public record CustomerCreatedIntegrationEvent(
    Guid CustomerId,
    Guid TenantId,
    string Email,
    string FirstName,
    string LastName);
