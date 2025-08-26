using E_Commerce.Common.Domain.Primitives;
using E_Commerce.Common.Domain.ValueObjects;
using E_Commerce.CustomerManagement.Domain.ValueObjects;
using E_Commerce.CustomerManagement.Domain.Enums;
using E_Commerce.CustomerManagement.Domain.Events;

namespace E_Commerce.CustomerManagement.Domain.Entities;

public sealed class Customer : Entity<CustomerId>
{
    public Email Email { get; private set; } = null!;
    public FullName FullName { get; private set; } = null!;
    public PhoneNumber? PhoneNumber { get; private set; }
    public DateTime? DateOfBirth { get; private set; }
    public CustomerStatus Status { get; private set; }
    public bool IsEmailVerified { get; private set; }
    public DateTime? LastLoginAt { get; private set; }

    private readonly List<CustomerAddress> _addresses = [];
    public IReadOnlyList<CustomerAddress> Addresses => _addresses.AsReadOnly();

    // Private constructor for Entity Framework
    private Customer() : base() { }

    // Private constructor for domain creation
    private Customer(CustomerId id, TenantId tenantId, Email email, FullName fullName)
        : base(id, tenantId)
    {
        Email = email;
        FullName = fullName;
        Status = CustomerStatus.Active;
        IsEmailVerified = false;
    }

    public static Customer Create(TenantId tenantId, string email, string firstName, string lastName, DateTime? dateOfBirth = null)
    {
        var emailVo = Email.Create(email);
        var fullNameVo = FullName.Create(firstName, lastName);
        
        var customer = new Customer(CustomerId.NewId(), tenantId, emailVo, fullNameVo)
        {
            DateOfBirth = dateOfBirth
        };

        customer.RaiseDomainEvent(new CustomerCreatedEvent(customer.Id, customer.TenantId, emailVo, fullNameVo));

        return customer;
    }

    public void UpdatePersonalInfo(string firstName, string lastName, DateTime? dateOfBirth = null)
    {
        FullName = FullName.Create(firstName, lastName);
        DateOfBirth = dateOfBirth;
        MarkAsUpdated();
    }

    public void UpdateEmail(string email)
    {
        var newEmail = Email.Create(email);
        if (!Email.Equals(newEmail))
        {
            Email = newEmail;
            IsEmailVerified = false;
            MarkAsUpdated();
            
            RaiseDomainEvent(new CustomerEmailChangedEvent(Id, TenantId, newEmail));
        }
    }

    public void UpdatePhoneNumber(string phoneNumber)
    {
        PhoneNumber = PhoneNumber.Create(phoneNumber);
        MarkAsUpdated();
    }

    public void VerifyEmail()
    {
        if (!IsEmailVerified)
        {
            IsEmailVerified = true;
            MarkAsUpdated();
            
            RaiseDomainEvent(new CustomerEmailVerifiedEvent(Id, TenantId, Email));
        }
    }

    public void ChangeStatus(CustomerStatus newStatus)
    {
        if (Status != newStatus)
        {
            var oldStatus = Status;
            Status = newStatus;
            MarkAsUpdated();
            
            RaiseDomainEvent(new CustomerStatusChangedEvent(Id, TenantId, oldStatus, newStatus));
        }
    }

    public CustomerAddress AddAddress(string street, string city, string postalCode, string country, bool isDefault = false)
    {
        var address = CustomerAddress.Create(street, city, postalCode, country, isDefault);
        
        if (isDefault)
        {
            foreach (var existingAddress in _addresses)
            {
                existingAddress.SetAsNonDefault();
            }
        }
        
        _addresses.Add(address);
        MarkAsUpdated();
        
        return address;
    }

    public void RemoveAddress(AddressId addressId)
    {
        var address = _addresses.FirstOrDefault(a => a.Id == addressId);
        if (address != null)
        {
            _addresses.Remove(address);
            MarkAsUpdated();
        }
    }

    public void RecordLogin()
    {
        LastLoginAt = DateTime.UtcNow;
        MarkAsUpdated();
    }

    public void Deactivate()
    {
        ChangeStatus(CustomerStatus.Inactive);
    }

    public void Block(string reason)
    {
        ChangeStatus(CustomerStatus.Blocked);
        RaiseDomainEvent(new CustomerBlockedEvent(Id, TenantId, reason));
    }
}
