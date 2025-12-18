using SubscriptionPlatform.Domain.Entities;

namespace SubscriptionPlatform.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}