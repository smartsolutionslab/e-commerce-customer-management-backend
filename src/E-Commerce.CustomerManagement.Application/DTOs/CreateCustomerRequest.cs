namespace E_Commerce.CustomerManagement.Application.DTOs;

public record CreateCustomerRequest(
    string Email,
    string FirstName,
    string LastName,
    string? MiddleName,
    DateTime DateOfBirth);

public record UpdateCustomerRequest(
    string Email,
    string FirstName,
    string LastName,
    string? MiddleName,
    string? PhoneNumber);

public record AddAddressRequest(
    string Street,
    string City,
    string State,
    string PostalCode,
    string Country,
    bool IsDefault = false);

public record CustomerResponse(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string? MiddleName,
    string? PhoneNumber,
    DateTime DateOfBirth,
    string Status,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
