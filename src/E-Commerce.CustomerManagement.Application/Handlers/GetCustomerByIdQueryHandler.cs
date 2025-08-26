using E_Commerce.Common.Application.Abstractions;
using E_Commerce.CustomerManagement.Application.DTOs;
using E_Commerce.CustomerManagement.Application.Interfaces;
using E_Commerce.CustomerManagement.Application.Queries;
using E_Commerce.CustomerManagement.Domain.Enums;
using E_Commerce.CustomerManagement.Domain.ValueObjects;
using MediatR;

namespace E_Commerce.CustomerManagement.Application.Handlers;

public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, Result<CustomerResponse>>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Result<CustomerResponse>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var customerId = CustomerId.Create(request.Id);
            var customer = await _customerRepository.GetByIdAsync(customerId, cancellationToken);

            if (customer == null)
                return Result.Failure<CustomerResponse>(new Error("Customer.NotFound", "Customer not found"));

            var response = new CustomerResponse(
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
            );

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Failure<CustomerResponse>(new Error("GetCustomer.Failed", ex.Message));
        }
    }
}
