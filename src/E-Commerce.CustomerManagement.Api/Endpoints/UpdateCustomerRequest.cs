namespace E_Commerce.CustomerManagement.Api.Endpoints;

public record UpdateCustomerRequest(
    string FirstName,
    string LastName,
    DateTime? DateOfBirth = null);