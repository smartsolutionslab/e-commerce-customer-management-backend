using E_Commerce.Common.Application.Abstractions;
using E_Commerce.CustomerManagement.Application.DTOs;
using E_Commerce.CustomerManagement.Domain.ValueObjects;

namespace E_Commerce.CustomerManagement.Application.Queries;

public record GetCustomerAddressesQuery(
    CustomerId CustomerId) : IQuery<List<AddressResponse>>;
