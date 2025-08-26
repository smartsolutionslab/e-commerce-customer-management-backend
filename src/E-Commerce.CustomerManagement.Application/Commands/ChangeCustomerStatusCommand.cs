using E_Commerce.Common.Application.Abstractions;
using E_Commerce.CustomerManagement.Domain.Enums;
using E_Commerce.CustomerManagement.Domain.ValueObjects;


namespace E_Commerce.CustomerManagement.Events.Application.Commands;

public record ChangeCustomerStatusCommand(
    CustomerId CustomerId,
    CustomerStatus Status,
    string? Reason = null) : ICommand;