using E_Commerce.Common.Application.Abstractions;
using E_Commerce.CustomerManagement.Application.DTOs;
using E_Commerce.CustomerManagement.Application.Interfaces;
using E_Commerce.CustomerManagement.Application.Queries;
using E_Commerce.CustomerManagement.Domain.Enums;
using MediatR;

namespace E_Commerce.CustomerManagement.Application.Handlers;

public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, Result<List<CustomerResponse>>>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomersQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Result<List<CustomerResponse>>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var customers = await _customerRepository.GetPagedAsync(
                request.Page, 
                request.Limit, 
                request.Search, 
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