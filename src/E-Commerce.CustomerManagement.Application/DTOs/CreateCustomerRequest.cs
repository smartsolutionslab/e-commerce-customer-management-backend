namespace E_Commerce.CustomerManagement.Application.DTOs;

public record CreateCustomerRequest(
    string Email,
    string FirstName,
    string LastName,
    string? MiddleName,
    DateTime DateOfBirth);