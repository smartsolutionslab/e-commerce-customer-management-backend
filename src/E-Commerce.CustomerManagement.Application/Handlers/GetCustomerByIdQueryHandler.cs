using E_Commerce.Common.Application.Abstractions;
using E_Commerce.CustomerManagement.Application.DTOs;
using E_Commerce.CustomerManagement.Application.Interfaces;
using E_Commerce.CustomerManagement.Application.Queries;
using E_Commerce.CustomerManagement.Domain.Enums;
using E_Commerce.CustomerManagement.Domain.ValueObjects;

namespace E_Commerce.CustomerManagement.Application.Handlers;

public class GetCustomerByIdQueryHandler(ICustomerRepository customerRepository)
    : IQueryHandler<GetCustomerByIdQuery, CustomerResponse>
{
    public async Task<Result<CustomerResponse>> HandleAsync(GetCustomerByIdQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            var customerId = CustomerId.Create(query.Id);
            var customer = await customerRepository.GetByIdAsync(customerId, cancellationToken);

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