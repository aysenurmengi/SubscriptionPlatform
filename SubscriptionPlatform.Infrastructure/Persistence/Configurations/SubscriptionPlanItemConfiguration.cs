using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubscriptionPlatform.Domain.Entities;

namespace SubscriptionPlatform.Infrastructure.Persistence.Configurations
{
    public class SubscriptionPlanItemConfiguration : IEntityTypeConfiguration<SubscriptionPlanItem>
    {
        public void Configure(EntityTypeBuilder<SubscriptionPlanItem> builder)
        {
            builder.HasKey(i => i.Id);
            
            builder.Property(i => i.Quantity)
                   .IsRequired();
            
            builder.ToTable(t => t.HasCheckConstraint("CH_PlanItem_Quantity_Positive", "\"Quantity\" >= 1"));

            builder.HasOne(i => i.SubscriptionPlan) //
                   .WithMany(p => p.PlanItems)
                   .HasForeignKey(i => i.SubscriptionPlanId)
                   .IsRequired();

            builder.HasOne(i => i.Product)
                   .WithMany(p => p.PlanItems)
                   .HasForeignKey(i => i.ProductId)
                   .IsRequired();

            builder.HasIndex(i => new { i.SubscriptionPlanId, i.ProductId })
                   .IsUnique();
        }
    }
}