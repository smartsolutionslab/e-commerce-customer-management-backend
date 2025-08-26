using E_Commerce.Common.Application.Abstractions;
using E_Commerce.CustomerManagement.Application.DTOs;

namespace E_Commerce.CustomerManagement.Application.Queries;

public record GetCustomerByIdQuery(
    Guid Id) : IQuery<CustomerResponse>;