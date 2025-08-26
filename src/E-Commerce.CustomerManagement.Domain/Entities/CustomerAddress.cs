using E_Commerce.CustomerManagement.Domain.ValueObjects;

namespace E_Commerce.CustomerManagement.Domain.Entities;

public class CustomerAddress
{
    public AddressId Id { get; private set; } = null!;
    public bool IsDefault { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Address properties directly on the entity for EF Core
    public string Street { get; private set; } = string.Empty;
    public string City { get; private set; } = string.Empty;
    public string PostalCode { get; private set; } = string.Empty;
    public string Country { get; private set; } = string.Empty;

    // Address value object for domain logic (not mapped to DB)
    public Address Address => Address.Create(Street, City, PostalCode, Country);

    private CustomerAddress() { } // For EF Core

    private CustomerAddress(AddressId id, string street, string city, string postalCode, string country, bool isDefault = false)
    {
        Id = id;
        Street = street;
        City = city;
        PostalCode = postalCode;
        Country = country;
        IsDefault = isDefault;
        CreatedAt = DateTime.UtcNow;
    }

    public static CustomerAddress Create(string street, string city, string postalCode, string country, bool isDefault = false)
    {
        // Validate using the Address value object
        var address = Address.Create(street, city, postalCode, country);
        
        return new CustomerAddress(AddressId.NewId(), address.Street, address.City, address.PostalCode, address.Country, isDefault);
    }

    public void SetAsDefault()
    {
        IsDefault = true;
    }

    public void SetAsNonDefault()
    {
        IsDefault = false;
    }

    public void UpdateAddress(string street, string city, string postalCode, string country)
    {
        // Validate using the Address value object
        var address = Address.Create(street, city, postalCode, country);
        
        Street = address.Street;
        City = address.City;
        PostalCode = address.PostalCode;
        Country = address.Country;
    }

    public override string ToString() => Address.ToString();
}
