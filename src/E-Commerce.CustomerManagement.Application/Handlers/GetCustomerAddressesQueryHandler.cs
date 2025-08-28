using E_Commerce.Common.Application.Abstractions;
using E_Commerce.CustomerManagement.Application.DTOs;
using E_Commerce.CustomerManagement.Application.Interfaces;
using E_Commerce.CustomerManagement.Application.Queries;

namespace E_Commerce.CustomerManagement.Application.Handlers;

public class GetCustomerAddressesQueryHandler(ICustomerRepository customerRepository)
    : IQueryHandler<GetCustomerAddressesQuery, List<AddressResponse>>
{
    public async Task<Result<List<AddressResponse>>> HandleAsync(GetCustomerAddressesQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            var customer = await customerRepository.GetByIdAsync(query.CustomerId, cancellationToken);
            if (customer == null)
                return Result.Failure<List<AddressResponse>>(new Error("Customer.NotFound", "Customer not found"));

            var addresses = customer.Addresses.Select(address => new AddressResponse(
                address.Id.Value,
                address.Street,
                address.City,
                address.PostalCode,
                address.Country,
                address.IsDefault,
                address.CreatedAt
            )).ToList();

            return Result.Success(addresses);
        }
        catch (Exception ex)
        {
            return Result.Failure<List<AddressResponse>>(new Error("GetAddresses.Failed", ex.Message));
        }
    }
}


