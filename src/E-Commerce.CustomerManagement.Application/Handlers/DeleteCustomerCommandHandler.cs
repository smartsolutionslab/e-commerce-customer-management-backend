using E_Commerce.Common.Application.Abstractions;
using E_Commerce.CustomerManagement.Application.Commands;
using E_Commerce.CustomerManagement.Application.Interfaces;

namespace E_Commerce.CustomerManagement.Application.Handlers;

public class DeleteCustomerCommandHandler(ICustomerRepository customerRepository)
    : ICommandHandler<DeleteCustomerCommand>
{
    public async Task<Result> HandleAsync(DeleteCustomerCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var customer = await customerRepository.GetByIdAsync(command.CustomerId, cancellationToken);
            if (customer == null)
                return Result.Failure(new Error("Customer.NotFound", "Customer not found"));

            customerRepository.Remove(customer);
            await customerRepository.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(new Error("DeleteCustomer.Failed", ex.Message));
        }
    }
}


