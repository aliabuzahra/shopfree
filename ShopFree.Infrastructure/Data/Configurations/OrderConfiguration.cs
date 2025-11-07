using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopFree.Domain.Entities;
using ShopFree.Domain.ValueObjects;

namespace ShopFree.Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.OrderNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(o => o.OrderNumber)
            .IsUnique();

        // Configure CustomerInfo value object
        builder.OwnsOne(o => o.Customer, customer =>
        {
            customer.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("CustomerName");

            customer.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("CustomerEmail");

            customer.Property(c => c.Phone)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("CustomerPhone");
        });

        // Configure Address value object
        builder.OwnsOne(o => o.ShippingAddress, address =>
        {
            address.Property(a => a.Street)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("ShippingStreet");

            address.Property(a => a.City)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("ShippingCity");

            address.Property(a => a.State)
                .HasMaxLength(100)
                .HasColumnName("ShippingState");

            address.Property(a => a.PostalCode)
                .HasMaxLength(20)
                .HasColumnName("ShippingPostalCode");

            address.Property(a => a.Country)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("ShippingCountry");
        });

        builder.Property(o => o.TotalAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(o => o.PaymentDetails)
            .HasMaxLength(500);

        builder.Property(o => o.Notes)
            .HasMaxLength(1000);

        builder.HasMany(o => o.OrderItems)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

