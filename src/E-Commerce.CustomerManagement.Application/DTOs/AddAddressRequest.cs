namespace E_Commerce.CustomerManagement.Application.DTOs;

public record AddAddressRequest(
    string Street,
    string City,
    string State,
    string PostalCode,
    string Country,
    bool IsDefault = false);