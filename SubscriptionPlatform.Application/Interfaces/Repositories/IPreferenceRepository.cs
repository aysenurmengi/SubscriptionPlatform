using SubscriptionPlatform.Domain.Entities;

namespace SubscriptionPlatform.Application.Interfaces.Repositories
{
    public interface IPreferenceRepository : IGenericRepository<Preference>
    {
        Task<IReadOnlyList<Preference>> GetAllByCustomerIdAsync(Guid customerId);
        Task AddRangeAsync(IEnumerable<Preference> entities); //birden fazla kayıt eklemek için
        Task DeleteRangeAsync(IEnumerable<Preference> entities); // "" ""  "" silmek için
    }
}