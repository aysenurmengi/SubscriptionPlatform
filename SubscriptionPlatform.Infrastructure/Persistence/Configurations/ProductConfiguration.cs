using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubscriptionPlatform.Domain.Entities;

namespace SubscriptionPlatform.Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(250);

            builder.Property(p => p.Description)
                   .HasMaxLength(1000); 

            builder.Property(p => p.Price)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();
            
            builder.Property(p => p.IsActive)
                   .IsRequired();
            
            builder.Property(p => p.ImageUrl)
                   .HasMaxLength(500); 

            builder.HasOne(p => p.Inventory)
                   .WithOne(i => i.Product) 
                   .HasForeignKey<Inventory>(i => i.ProductId) 
                   .IsRequired();

            builder.HasMany(p => p.PlanItems) 
                   .WithOne(spi => spi.Product)
                   .HasForeignKey(spi => spi.ProductId) 
                   .OnDelete(DeleteBehavior.Restrict);
                   
            builder.HasIndex(p => p.Name);
        }
    }
}