using AutoFixture.Xunit2;
using E_Commerce.Common.Domain.ValueObjects;
using E_Commerce.CustomerManagement.Domain.Entities;
using E_Commerce.CustomerManagement.Domain.Events;
using E_Commerce.CustomerManagement.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace E_Commerce.CustomerManagement.UnitTests.Domain;

public class CustomerTests
{
    [Theory, AutoData]
    public void Create_ShouldCreateCustomerWithValidData(
        string emailValue,
        string firstName,
        string lastName,
        DateTime dateOfBirth)
    {
        // Arrange
        var tenantId = TenantId.NewId();
        var email = Email.Create($"{emailValue}@test.com");
        var fullName = FullName.Create(firstName, lastName);

        // Act
        var customer = Customer.Create(tenantId, email, fullName, dateOfBirth);

        // Assert
        customer.Should().NotBeNull();
        customer.Id.Should().NotBe(CustomerId.Create(Guid.Empty));
        customer.TenantId.Should().Be(tenantId);
        customer.Email.Should().Be(email);
        customer.FullName.Should().Be(fullName);
        customer.DateOfBirth.Should().Be(dateOfBirth);
        customer.Status.Should().Be(CustomerStatus.Active);
        
        var domainEvents = customer.GetDomainEvents();
        domainEvents.Should().HaveCount(1);
        domainEvents.First().Should().BeOfType<CustomerCreatedEvent>();
    }

    [Theory, AutoData]
    public void UpdateEmail_ShouldUpdateEmailAndRaiseDomainEvent(
        string initialEmailValue,
        string newEmailValue,
        string firstName,
        string lastName,
        DateTime dateOfBirth)
    {
        // Arrange
        var tenantId = TenantId.NewId();
        var initialEmail = Email.Create($"{initialEmailValue}@test.com");
        var newEmail = Email.Create($"{newEmailValue}@test.com");
        var fullName = FullName.Create(firstName, lastName);

        var customer = Customer.Create(tenantId, initialEmail, fullName, dateOfBirth);
        customer.ClearDomainEvents(); // Clear creation event

        // Act
        customer.UpdateEmail(newEmail);

        // Assert
        customer.Email.Should().Be(newEmail);
        customer.UpdatedAt.Should().NotBeNull();
        
        var domainEvents = customer.GetDomainEvents();
        domainEvents.Should().HaveCount(1);
        
        var emailChangedEvent = domainEvents.First().Should().BeOfType<CustomerEmailChangedEvent>().Subject;
        emailChangedEvent.CustomerId.Should().Be(customer.Id);
        emailChangedEvent.OldEmail.Should().Be(initialEmail);
        emailChangedEvent.NewEmail.Should().Be(newEmail);
    }

    [Theory, AutoData]
    public void Deactivate_ShouldSetStatusToInactiveAndRaiseDomainEvent(
        string emailValue,
        string firstName,
        string lastName,
        DateTime dateOfBirth)
    {
        // Arrange
        var tenantId = TenantId.NewId();
        var email = Email.Create($"{emailValue}@test.com");
        var fullName = FullName.Create(firstName, lastName);

        var customer = Customer.Create(tenantId, email, fullName, dateOfBirth);
        customer.ClearDomainEvents();

        // Act
        customer.Deactivate();

        // Assert
        customer.Status.Should().Be(CustomerStatus.Inactive);
        customer.UpdatedAt.Should().NotBeNull();
        
        var domainEvents = customer.GetDomainEvents();
        domainEvents.Should().HaveCount(1);
        domainEvents.First().Should().BeOfType<CustomerDeactivatedEvent>();
    }
}
