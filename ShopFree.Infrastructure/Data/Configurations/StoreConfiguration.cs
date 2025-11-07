using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopFree.Domain.Entities;

namespace ShopFree.Infrastructure.Data.Configurations;

public class StoreConfiguration : IEntityTypeConfiguration<Store>
{
    public void Configure(EntityTypeBuilder<Store> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Description)
            .HasMaxLength(500);

        builder.Property(s => s.Subdomain)
            .HasMaxLength(50);

        builder.HasIndex(s => s.Subdomain)
            .IsUnique()
            .HasFilter("[Subdomain] IS NOT NULL");

        builder.Property(s => s.LogoUrl)
            .HasMaxLength(500);

        builder.HasMany(s => s.Products)
            .WithOne(p => p.Store)
            .HasForeignKey(p => p.StoreId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.Orders)
            .WithOne(o => o.Store)
            .HasForeignKey(o => o.StoreId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.PaymentMethods)
            .WithOne(pm => pm.Store)
            .HasForeignKey(pm => pm.StoreId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

