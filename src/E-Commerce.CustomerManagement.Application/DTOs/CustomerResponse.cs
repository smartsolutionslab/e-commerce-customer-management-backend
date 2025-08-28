namespace E_Commerce.CustomerManagement.Application.DTOs;

public record CustomerResponse(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string? MiddleName,
    string? PhoneNumber,
    DateTime? DateOfBirth,
    string Status,
    DateTime CreatedAt,
    DateTime? UpdatedAt);