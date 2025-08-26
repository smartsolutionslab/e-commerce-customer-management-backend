using E_Commerce.Common.Application.Abstractions;
using E_Commerce.CustomerManagement.Domain.ValueObjects;

namespace E_Commerce.CustomerManagement.Application.Commands;

public record DeleteCustomerCommand(
    CustomerId CustomerId) : ICommand;