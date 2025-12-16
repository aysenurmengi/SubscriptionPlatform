using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubscriptionPlatform.Domain.Entities;

namespace SubscriptionPlatform.Infrastructure.Persistence.Configurations
{
    public class CustomerCOnfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

            builder.Property(c => c.LastName)
                    .IsRequired()   
                    .HasMaxLength(100);

            builder.Property(c => c.Email)
                    .IsRequired()
                    .HasMaxLength(256);

            builder.HasIndex(c => c.Email)
                    .IsUnique(); 

            builder.Property(c => c.PasswordHash)
                   .IsRequired();
            
            builder.HasMany(c => c.Subscriptions) // customer -> subscriptions
                   .WithOne() 
                   .HasForeignKey("CustomerId")
                   .IsRequired();

            builder.HasMany(c => c.Orders) // customer -> orders
                   .WithOne()
                   .HasForeignKey("CustomerId")
                   .IsRequired();

            builder.HasMany(c => c.Preferences) // customer -> preferences
                   .WithOne()
                   .HasForeignKey("CustomerId")
                   .IsRequired();
            
            
        }
    }
}