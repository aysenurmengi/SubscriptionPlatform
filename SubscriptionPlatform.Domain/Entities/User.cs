using SubscriptionPlatform.Domain.Common;
using SubscriptionPlatform.Domain.Enums;

namespace SubscriptionPlatform.Domain.Entities
{
    public class User : BaseEntity
{
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string FullName { get; set; }
    public UserRole Role { get; set; } = UserRole.Customer; // varsayılan olarak Customer
    public bool IsActive { get; set; } = true;
}
}