using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubscriptionPlatform.Domain.Entities;

namespace SubscriptionPlatform.Infrastructure.Persistence.Configurations
{
    public class SubscriptionPlanConfiguration : IEntityTypeConfiguration<SubscriptionPlan>
    {
        public void Configure(EntityTypeBuilder<SubscriptionPlan> builder)
        {
            builder.HasKey(sp => sp.Id);

            builder.Property(sp => sp.Name)
                   .IsRequired()
                   .HasMaxLength(250);

            builder.Property(sp => sp.Price)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();
            
            builder.Property(sp => sp.IsActive)
                   .IsRequired();

            builder.HasMany(sp => sp.PlanItems) 
                   .WithOne(spi => spi.SubscriptionPlan) 
                   .HasForeignKey(spi => spi.SubscriptionPlanId)
                   .OnDelete(DeleteBehavior.Cascade); // ana varlık silindiğinde bağımlı varlıklar silinsin(Cascade)
            
            builder.HasIndex(sp => sp.Name)
                   .IsUnique();

            builder.ToTable(t => t.HasCheckConstraint("CH_Plan_Price_Positive", "Price >= 0")); // fiyat için kontrol kısıtlaması
        }
    }
}