using E_Commerce.Common.Domain.Primitives;
using E_Commerce.Common.Domain.ValueObjects;
using E_Commerce.CustomerManagement.Domain.ValueObjects;

namespace E_Commerce.CustomerManagement.Domain.Events;

public sealed record CustomerEmailVerifiedEvent(
    CustomerId CustomerId,
    TenantId TenantId,
    Email Email) : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}