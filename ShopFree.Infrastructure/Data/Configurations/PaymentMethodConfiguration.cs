using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopFree.Domain.Entities;

namespace ShopFree.Infrastructure.Data.Configurations;

public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.HasKey(pm => pm.Id);

        builder.Property(pm => pm.Title)
            .HasMaxLength(200);

        builder.Property(pm => pm.Details)
            .HasMaxLength(1000);
    }
}

