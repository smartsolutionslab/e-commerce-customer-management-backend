namespace E_Commerce.CustomerManagement.Application.DTOs;

public record UpdateCustomerRequest(
    string Email,
    string FirstName,
    string LastName,
    string? MiddleName,
    string? PhoneNumber);