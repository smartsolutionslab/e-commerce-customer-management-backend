using E_Commerce.Common.Application.Abstractions;
using E_Commerce.CustomerManagement.Application.DTOs;

namespace E_Commerce.CustomerManagement.Application.Queries;

public record GetCustomersQuery(
    int Page = 1,
    int Limit = 20,
    string? Search = null) : IQuery<List<CustomerResponse>>;