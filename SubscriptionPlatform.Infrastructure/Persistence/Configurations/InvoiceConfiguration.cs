using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SubscriptionPlatform.Domain.Entities;
using SubscriptionPlatform.Domain.Enums; // PaymentStatus için gerekli

namespace SubscriptionPlatform.Infrastructure.Persistence.Configurations
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Amount)
                   .HasColumnType("decimal(18,2)") 
                   .IsRequired();
            
            builder.Property(i => i.PaymentStatus)
                   .HasConversion<string>() 
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(i => i.IssueDate)
                   .IsRequired();
            
            builder.Property(i => i.DueDate)
                   .IsRequired();

            builder.HasOne(i => i.Subscription)
                   .WithMany() 
                   .HasForeignKey(i => i.SubscriptionId) 
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable(t => t.HasCheckConstraint("CH_Invoice_Amount_Positive", "Amount >= 0"));
        }
    }
}