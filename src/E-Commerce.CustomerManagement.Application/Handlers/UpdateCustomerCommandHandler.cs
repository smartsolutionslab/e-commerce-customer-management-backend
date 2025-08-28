using E_Commerce.Common.Application.Abstractions;
using E_Commerce.CustomerManagement.Application.Commands;
using E_Commerce.CustomerManagement.Application.Interfaces;

namespace E_Commerce.CustomerManagement.Application.Handlers;

public class UpdateCustomerCommandHandler(ICustomerRepository customerRepository)
    : ICommandHandler<UpdateCustomerCommand>
{
    public async Task<Result> HandleAsync(UpdateCustomerCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var customer = await customerRepository.GetByIdAsync(command.CustomerId, cancellationToken);
            if (customer == null)
                return Result.Failure(new Error("Customer.NotFound", "Customer not found"));

            customer.UpdatePersonalInfo(command.FirstName, command.LastName, command.DateOfBirth);

            customerRepository.Update(customer);
            await customerRepository.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(new Error("UpdateCustomer.Failed", ex.Message));
        }
    }
}
