namespace E_Commerce.CustomerManagement.Application.DTOs;

public record AddressResponse(
    Guid Id,
    string Street,
    string City,
    string PostalCode,
    string Country,
    bool IsDefault,
    DateTime CreatedAt);
