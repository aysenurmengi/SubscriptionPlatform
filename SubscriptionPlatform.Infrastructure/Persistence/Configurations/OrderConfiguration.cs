using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubscriptionPlatform.Domain.Entities;
using SubscriptionPlatform.Domain.Enums;

namespace SubscriptionPlatform.Infrastructure.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.ShippingAddress)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(o => o.TrackingNumber)
                   .HasMaxLength(150);

            builder.Property(o => o.Status)
                   .HasConversion<string>()
                   .IsRequired();
            
            builder.Property(o => o.ShippingStatus)
                   .HasConversion<string>()
                   .IsRequired();

            builder.Property(o => o.IsSubscriptionRenewal)
                   .IsRequired();


            builder.HasOne<Customer>() //order - customer
                   .WithMany(c => c.Orders) // customer -> orders
                   .HasForeignKey(o => o.CustomerId)
                   .IsRequired(); 
            
            builder.HasOne<Subscription>()
                   .WithMany()
                   .HasForeignKey(o => o.SubscriptionId)
                   .IsRequired(false);
        }
    }
}