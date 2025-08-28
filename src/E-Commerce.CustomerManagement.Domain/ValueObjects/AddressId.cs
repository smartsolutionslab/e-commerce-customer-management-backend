namespace E_Commerce.CustomerManagement.Domain.ValueObjects;

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
    public static AddressId Create(string value) => new(Guid.Parse(value));
    public static AddressId NewId() => new(Guid.NewGuid());

    public static implicit operator Guid(AddressId addressId) => addressId.Value;
    public static implicit operator string(AddressId addressId) => addressId.Value.ToString();

    public override string ToString() => Value.ToString();
}
