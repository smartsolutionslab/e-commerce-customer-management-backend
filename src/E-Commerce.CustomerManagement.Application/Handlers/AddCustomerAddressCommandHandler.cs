using E_Commerce.Common.Application.Abstractions;
using E_Commerce.CustomerManagement.Application.Commands;
using E_Commerce.CustomerManagement.Application.Interfaces;

namespace E_Commerce.CustomerManagement.Application.Handlers;

public class AddCustomerAddressCommandHandler(ICustomerRepository customerRepository)
    : ICommandHandler<AddCustomerAddressCommand, Guid>
{
    public async Task<Result<Guid>> HandleAsync(AddCustomerAddressCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var customer = await customerRepository.GetByIdAsync(command.CustomerId, cancellationToken);
            if (customer == null)
                return Result.Failure<Guid>(new Error("Customer.NotFound", "Customer not found"));

            var address = customer.AddAddress(
                command.Street,
                command.City,
                command.PostalCode,
                command.Country,
                command.IsDefault);

            customerRepository.Update(customer);
            await customerRepository.SaveChangesAsync(cancellationToken);

            return Result.Success(address.Id.Value);
        }
        catch (Exception ex)
        {
            return Result.Failure<Guid>(new Error("AddAddress.Failed", ex.Message));
        }
    }
}
       