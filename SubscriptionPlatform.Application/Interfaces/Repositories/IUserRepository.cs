
using SubscriptionPlatform.Domain.Entities;

namespace SubscriptionPlatform.Application.Interfaces.Repositories;
public interface IUserRepository : IGenericRepository<User>
{
   Task<User> GetByEmailAsync(string email);
}
