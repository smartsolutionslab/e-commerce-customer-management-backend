using E_Commerce.Common.Application.Abstractions;
using E_Commerce.CustomerManagement.Domain.ValueObjects;

namespace E_Commerce.CustomerManagement.Application.Commands;

public record AddCustomerAddressCommand(
    CustomerId CustomerId,
    string Street,
    string City,
    string PostalCode,
    string Country,
    bool IsDefault = false) : ICommand<Guid>;