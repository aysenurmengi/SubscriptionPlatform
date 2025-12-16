using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubscriptionPlatform.Domain.Entities;

namespace SubscriptionPlatform.Infrastructure.Persistence.Configurations
{
    public class PreferenceConfiguration : IEntityTypeConfiguration<Preference>
    {
        public void Configure(EntityTypeBuilder<Preference> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Key)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.Value)
                   .IsRequired()
                   .HasMaxLength(500);
            
            builder.HasOne(p => p.Customer) 
                   .WithMany(c => c.Preferences)
                   .HasForeignKey(p => p.CustomerId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(p => new { p.CustomerId, p.Key })
                   .IsUnique();
        }
    }
}