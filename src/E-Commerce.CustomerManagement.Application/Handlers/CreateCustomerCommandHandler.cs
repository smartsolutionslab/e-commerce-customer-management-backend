using E_Commerce.Common.Application.Abstractions;
using E_Commerce.CustomerManagement.Application.Commands;
using E_Commerce.CustomerManagement.Application.Interfaces;
using E_Commerce.CustomerManagement.Domain.Entities;
using E_Commerce.CustomerManagement.Domain.ValueObjects;

namespace E_Commerce.CustomerManagement.Application.Handlers;

public class CreateCustomerCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<CreateCustomerCommand, CustomerId>
{
    public async Task<Result<CustomerId>> HandleAsync(CreateCustomerCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            // Check if email already exists
            var email = Email.Create(command.Email);
            var existingCustomer = await customerRepository.GetByEmailAsync(email, cancellationToken);
            if (existingCustomer != null)
                return Result.Failure<CustomerId>(new Error("Customer.EmailExists", "Customer with this email already exists"));

            // Create customer
            var customer = Customer.Create(
                command.TenantId,
                command.Email,
                command.FirstName,
                command.LastName,
                command.DateOfBirth);

            await customerRepository.AddAsync(customer, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(customer.Id);
        }
        catch (Exception ex)
        {
            return Result.Failure<CustomerId>(new Error("CreateCustomer.Failed", ex.Message));
        }
    }
}


