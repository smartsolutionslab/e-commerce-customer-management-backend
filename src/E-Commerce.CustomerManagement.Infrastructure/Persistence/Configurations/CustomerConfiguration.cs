using E_Commerce.Common.Domain.ValueObjects;
using E_Commerce.CustomerManagement.Domain.Entities;
using E_Commerce.CustomerManagement.Domain.ValueObjects;
using E_Commerce.CustomerManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.CustomerManagement.Infrastructure.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasConversion(
                id => id.Value,
                value => CustomerId.Create(value))
            .ValueGeneratedNever();

        builder.Property(c => c.TenantId)
            .HasConversion(
                id => id.Value,
                value => TenantId.Create(value))
            .IsRequired();

        builder.Property(c => c.Email)
            .HasConversion(
                email => email.Value,
                value => Email.Create(value))
            .HasMaxLength(255)
            .IsRequired();

        builder.OwnsOne(c => c.FullName, fn =>
        {
            fn.Property(p => p.FirstName)
                .HasColumnName("FirstName")
                .HasMaxLength(100)
                .IsRequired();

            fn.Property(p => p.LastName)
                .HasColumnName("LastName")
                .HasMaxLength(100)
                .IsRequired();

            fn.Property(p => p.MiddleName)
                .HasColumnName("MiddleName")
                .HasMaxLength(100);
        });

        builder.Property(c => c.PhoneNumber)
            .HasConversion(
                phone => phone != null ? phone.Value : null,
                value => value != null ? PhoneNumber.Create(value) : null)
            .HasMaxLength(20);

        builder.Property(c => c.DateOfBirth);

        builder.Property(c => c.Status)
            .HasConversion<int>();

        builder.Property(c => c.IsEmailVerified)
            .IsRequired();

        builder.Property(c => c.LastLoginAt);

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt);

        builder.OwnsMany(c => c.Addresses, a =>
        {
            a.ToTable("CustomerAddresses");
            a.WithOwner().HasForeignKey("CustomerId");
            
            a.Property(addr => addr.Id)
                .HasConversion(
                    id => id.Value,
                    value => AddressId.Create(value))
                .ValueGeneratedNever();
            
            a.HasKey(addr => addr.Id);
            
            // Direct address properties instead of nested OwnsOne
            a.Property(addr => addr.Street)
                .HasMaxLength(200)
                .IsRequired();
            
            a.Property(addr => addr.City)
                .HasMaxLength(100)
                .IsRequired();
            
            a.Property(addr => addr.PostalCode)
                .HasMaxLength(20)
                .IsRequired();
            
            a.Property(addr => addr.Country)
                .HasMaxLength(100)
                .IsRequired();
            
            a.Property(addr => addr.IsDefault);
            a.Property(addr => addr.CreatedAt);
        });

        builder.HasIndex(c => c.Email)
            .IsUnique()
            .HasDatabaseName("IX_Customers_Email");

        builder.HasIndex(c => c.TenantId)
            .HasDatabaseName("IX_Customers_TenantId");
    }
}
