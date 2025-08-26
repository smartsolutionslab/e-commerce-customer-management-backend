namespace E_Commerce.CustomerManagement.Api.Endpoints;

public record CreateCustomerRequest(
    string Email,
    string FirstName,
    string LastName,
    DateTime? DateOfBirth = null);