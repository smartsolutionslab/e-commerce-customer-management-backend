using E_Commerce.Common.Application.Abstractions;
using E_Commerce.CustomerManagement.Application.Commands;
using E_Commerce.CustomerManagement.Application.Interfaces;
using E_Commerce.CustomerManagement.Domain.Entities;
using E_Commerce.CustomerManagement.Domain.ValueObjects;
using MediatR;

namespace E_Commerce.CustomerManagement.Application.Handlers;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Result<CustomerId>>
{
    private readonly ICustomerRepository _customerRepository;

    public CreateCustomerCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Result<CustomerId>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if email already exists
            var email = Email.Create(request.Email);
            var existingCustomer = await _customerRepository.GetByEmailAsync(email, cancellationToken);
            if (existingCustomer != null)
                return Result.Failure<CustomerId>(new Error("Customer.EmailExists", "Customer with this email already exists"));

            // Create customer
            var customer = Customer.Create(
                request.TenantId,
                request.Email,
                request.FirstName,
                request.LastName,
                request.DateOfBirth);

            await _customerRepository.AddAsync(customer, cancellationToken);
            await _customerRepository.SaveChangesAsync(cancellationToken);

            return Result.Success(customer.Id);
        }
        catch (Exception ex)
        {
            return Result.Failure<CustomerId>(new Error("CreateCustomer.Failed", ex.Message));
        }
    }
}
           