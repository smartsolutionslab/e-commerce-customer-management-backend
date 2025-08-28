using E_Commerce.Common.Application.Abstractions;
using E_Commerce.CustomerManagement.Application.DTOs;
using E_Commerce.CustomerManagement.Application.Interfaces;
using E_Commerce.CustomerManagement.Application.Queries;
using E_Commerce.CustomerManagement.Domain.Enums;

namespace E_Commerce.CustomerManagement.Application.Handlers;

public class GetCustomersQueryHandler(ICustomerRepository customerRepository)
    : IQueryHandler<GetCustomersQuery, List<CustomerResponse>>
{
    public async Task<Result<List<CustomerResponse>>> HandleAsync(GetCustomersQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            var customers = await customerRepository.GetPagedAsync(
                query.Page, 
                query.Limit, 
                query.Search, 
                cancellationToken);

            var customerResponses = customers.Select(customer => new CustomerResponse(
                customer.Id.Value,
                customer.Email.Value,
                customer.FullName.FirstName,
                customer.FullName.LastName,
                customer.FullName.MiddleName,
                customer.PhoneNumber?.Value,
                customer.DateOfBirth,
                customer.Status.GetDisplayName(),
                customer.CreatedAt,
                customer.UpdatedAt
            )).ToList();

            return Result.Success(customerResponses);
        }
        catch (Exception ex)
        {
            return Result.Failure<List<CustomerResponse>>(new Error("GetCustomers.Failed", ex.Message));
        }
    }
}
