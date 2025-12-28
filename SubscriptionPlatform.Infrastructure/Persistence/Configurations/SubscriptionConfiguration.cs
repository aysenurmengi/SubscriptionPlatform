using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubscriptionPlatform.Domain.Entities;

namespace SubscriptionPlatform.Infrastructure.Persistence.Configurations
{
    public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.PaymentToken)
                   .IsRequired()
                   .HasMaxLength(256);

            builder.Property(s => s.PlanPrice)
                   .HasColumnType("decimal(18,2)") 
                   .IsRequired();

            builder.HasOne(s => s.Customer) // subscription - customer
                   .WithMany(c => c.Subscriptions) // customer -> subscriptions
                   .HasForeignKey(s => s.CustomerId); 
            
            builder.HasOne(s => s.SubscriptionPlan)
                   .WithMany()                      
                   .HasForeignKey(s => s.PlanId)   
                   .IsRequired()                   
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(s => s.PlanId);
            builder.HasIndex(s => s.CustomerId); // customerId index
        }
    }
}