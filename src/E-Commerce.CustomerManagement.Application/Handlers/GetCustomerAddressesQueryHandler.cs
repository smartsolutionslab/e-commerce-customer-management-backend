using E_Commerce.Common.Application.Abstractions;
using E_Commerce.CustomerManagement.Application.DTOs;
using E_Commerce.CustomerManagement.Application.Interfaces;
using E_Commerce.CustomerManagement.Application.Queries;
using MediatR;

namespace E_Commerce.CustomerManagement.Application.Handlers;

public class GetCustomerAddressesQueryHandler : IRequestHandler<GetCustomerAddressesQuery, Result<List<AddressResponse>>>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerAddressesQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Result<List<AddressResponse>>> Handle(GetCustomerAddressesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);
            if (customer == null)
                return Result.Failure<List<AddressResponse>>(new Error("Customer.NotFound", "Customer not found"));

            var addresses = customer.Addresses.Select(address => new AddressResponse(
                address.Id,
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
