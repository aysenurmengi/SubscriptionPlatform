namespace SubscriptionPlatform.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<string?> LoginAsync(string email, string password);
        Task<(bool IsSuccess, string[] Errors)> RegisterUserAsync(string email, string password, string firstName, string lastName);
        Task<bool> IsInRoleAsync(string userId, string role);
    }
}