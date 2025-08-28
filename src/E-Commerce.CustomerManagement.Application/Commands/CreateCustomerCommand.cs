using E_Commerce.Common.Application.Abstractions;
using E_Commerce.Common.Domain.ValueObjects;
using E_Commerce.CustomerManagement.Domain.ValueObjects;

namespace E_Commerce.CustomerManagement.Application.Commands;

public record CreateCustomerCommand(
    TenantId TenantId,
    string Email,
    string FirstName,
    string LastName,
    DateTime? DateOfBirth = null) : ICommand<CustomerId>;