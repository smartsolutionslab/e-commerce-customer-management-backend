using E_Commerce.Common.Domain.Primitives;
using E_Commerce.Common.Domain.ValueObjects;
using E_Commerce.CustomerManagement.Domain.Enums;
using E_Commerce.CustomerManagement.Domain.ValueObjects;

namespace E_Commerce.CustomerManagement.Domain.Events;

public sealed record CustomerStatusChangedEvent(
    CustomerId CustomerId,
    TenantId TenantId,
    CustomerStatus OldStatus,
    CustomerStatus NewStatus) : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}