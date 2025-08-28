namespace E_Commerce.CustomerManagement.Infrastructure.EventHandlers;

public record CustomerCreatedIntegrationEvent(
    Guid CustomerId,
    Guid TenantId,
    string Email,
    string FirstName,
    string LastName);