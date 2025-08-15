using E_Commerce.Common.Domain.Primitives;
using E_Commerce.Common.Domain.ValueObjects;

namespace E_Commerce.CustomerManagement.Domain.ValueObjects;

public class Address : Entity<AddressId>
{
    public string Street { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string PostalCode { get; private set; }
    public string Country { get; private set; }
    public bool IsDefault { get; private set; }

    private Address(AddressId id, TenantId tenantId, string street, string city, string state, string postalCode, string country, bool isDefault = false)
        : base(id, tenantId)
    {
        Street = street;
        City = city;
        State = state;
        PostalCode = postalCode;
        Country = country;
        IsDefault = isDefault;
    }

    private Address() : base() { } // For EF

    public static Address Create(TenantId tenantId, string street, string city, string state, string postalCode, string country, bool isDefault = false)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException("Street cannot be empty", nameof(street));

        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City cannot be empty", nameof(city));

        return new Address(AddressId.NewId(), tenantId, street.Trim(), city.Trim(), state.Trim(), postalCode.Trim(), country.Trim(), isDefault);
    }

    public void SetAsDefault()
    {
        IsDefault = true;
        MarkAsUpdated();
    }

    public void SetAsNonDefault()
    {
        IsDefault = false;
        MarkAsUpdated();
    }
}

public record AddressId
{
    public Guid Value { get; }

    private AddressId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("AddressId cannot be empty", nameof(value));
        
        Value = value;
    }

    public static AddressId Create(Guid value) => new(value);
    public static AddressId NewId() => new(Guid.NewGuid());

    public static implicit operator Guid(AddressId addressId) => addressId.Value;
    public override string ToString() => Value.ToString();
}
