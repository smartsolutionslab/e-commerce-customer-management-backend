using E_Commerce.Common.Domain.ValueObjects;
using E_Commerce.CustomerManagement.Domain.Entities;
using E_Commerce.CustomerManagement.Domain.ValueObjects;
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

        builder.OwnsOne(c => c.Email, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("Email")
                .HasMaxLength(255)
                .IsRequired();
        });

        builder.OwnsOne(c => c.FullName, fullName =>
        {
            fullName.Property(fn => fn.FirstName)
                .HasColumnName("FirstName")
                .HasMaxLength(100)
                .IsRequired();

            fullName.Property(fn => fn.LastName)
                .HasColumnName("LastName")
                .HasMaxLength(100)
                .IsRequired();

            fullName.Property(fn => fn.MiddleName)
                .HasColumnName("MiddleName")
                .HasMaxLength(100);
        });

        builder.OwnsOne(c => c.PhoneNumber, phoneNumber =>
        {
            phoneNumber.Property(pn => pn!.Value)
                .HasColumnName("PhoneNumber")
                .HasMaxLength(20);
        });

        builder.Property(c => c.DateOfBirth)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(c => c.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt);

        builder.HasMany(c => c.Addresses)
            .WithOne()
            .HasForeignKey("CustomerId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(c => new { c.TenantId, c.Email.Value })
            .IsUnique()
            .HasDatabaseName("IX_Customers_TenantId_Email");

        builder.HasIndex(c => c.TenantId)
            .HasDatabaseName("IX_Customers_TenantId");
    }
}
