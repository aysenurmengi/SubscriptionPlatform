using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubscriptionPlatform.Domain.Entities;

namespace SubscriptionPlatform.Infrastructure.Persistence.Configurations
{
    public class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
    {
        public void Configure(EntityTypeBuilder<Inventory> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.StockQuantity)
                   .IsRequired();
                   
            builder.Property(i => i.LowStockThreshold)
                   .IsRequired();
            
            builder.ToTable(t => t.HasCheckConstraint("CH_Inventory_StockQuantity_Positive", "\"StockQuantity\" >= 0"));

            builder.HasOne(i => i.Product) // inventory - product
                   .WithOne(p => p.Inventory)
                   .HasForeignKey<Inventory>(i => i.ProductId)
                   .IsRequired();

            builder.HasIndex(i => i.ProductId)
                   .IsUnique();
        }
    }
}