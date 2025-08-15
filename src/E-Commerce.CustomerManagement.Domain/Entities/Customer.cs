using E_Commerce.Common.Domain.Primitives;
using E_Commerce.Common.Domain.ValueObjects;
using E_Commerce.CustomerManagement.Domain.ValueObjects;
using E_Commerce.CustomerManagement.Domain.Events;

namespace E_Commerce.CustomerManagement.Domain.Entities;

public sealed class Customer : Entity<CustomerId>
{
    public Email Email { get; private set; }
    public FullName FullName { get; private set; }
    public PhoneNumber? PhoneNumber { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public CustomerStatus Status { get; private set; }
    
    private readonly List<Address> _addresses = [];
    public IReadOnlyList<Address> Addresses => _addresses.AsReadOnly();

    private Customer(CustomerId id, TenantId tenantId, Email email, FullName fullName, DateTime dateOfBirth)
        : base(id, tenantId)
    {
        Email = email;
        FullName = fullName;
        DateOfBirth = dateOfBirth;
        Status = CustomerStatus.Active;
    }

    private Customer() : base() { } // For EF

    public static Customer Create(TenantId tenantId, Email email, FullName fullName, DateTime dateOfBirth)
    {
        var customer = new Customer(CustomerId.NewId(), tenantId, email, fullName, dateOfBirth);
        
        customer.RaiseDomainEvent(new CustomerCreatedEvent(
            customer.Id,
            customer.TenantId,
            customer.Email,
            customer.FullName));

        return customer;
    }

    public void UpdateEmail(Email email)
    {
        if (Email == email) return;

        var oldEmail = Email;
        Email = email;
        MarkAsUpdated();

        RaiseDomainEvent(new CustomerEmailChangedEvent(Id, TenantId, oldEmail, email));
    }

    public void UpdateFullName(FullName fullName)
    {
        if (FullName == fullName) return;

        FullName = fullName;
        MarkAsUpdated();

        RaiseDomainEvent(new CustomerUpdatedEvent(Id, TenantId));
    }

    public void UpdatePhoneNumber(PhoneNumber? phoneNumber)
    {
        PhoneNumber = phoneNumber;
        MarkAsUpdated();
    }

    public void AddAddress(Address address)
    {
        if (_addresses.Any(a => a.IsDefault) && address.IsDefault)
        {
            // Remove default flag from existing addresses
            foreach (var existingAddress in _addresses.Where(a => a.IsDefault))
            {
                existingAddress.SetAsNonDefault();
            }
        }

        _addresses.Add(address);
        MarkAsUpdated();

        RaiseDomainEvent(new CustomerAddressAddedEvent(Id, TenantId, address.Id));
    }

    public void Deactivate()
    {
        if (Status == CustomerStatus.Inactive) return;

        Status = CustomerStatus.Inactive;
        MarkAsUpdated();

        RaiseDomainEvent(new CustomerDeactivatedEvent(Id, TenantId));
    }
}
