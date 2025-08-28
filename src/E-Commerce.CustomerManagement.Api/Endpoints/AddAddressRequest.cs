namespace E_Commerce.CustomerManagement.Api.Endpoints;

public record AddAddressRequest(
    string Street,
    string City,
    string PostalCode,
    string Country,
    bool IsDefault = false);