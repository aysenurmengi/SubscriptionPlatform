using SubscriptionPlatform.Application.Interfaces;
using SubscriptionPlatform.Application.Interfaces.Repositories;
using SubscriptionPlatform.Domain.Entities;
using SubscriptionPlatform.Domain.Enums;

namespace SubscriptionPlatform.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public IdentityService(IUnitOfWork unitOfWork, IJwtTokenGenerator jwtTokenGenerator)
        {
            _unitOfWork = unitOfWork;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<string?> LoginAsync(string email, string password)
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                return null;
            }

            if (user.PasswordHash != password) 
            {
                return null;
            }

            return _jwtTokenGenerator.GenerateToken(user);
        }

        public async Task<(bool IsSuccess, string[] Errors)> RegisterUserAsync(string email, string password, string firstName, string lastName, string role)
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            if (users.Any(u => u.Email == email))
            {
                return (false, new[] { "Bu email adresi zaten kullanımda." });
            }

            if (!Enum.TryParse<UserRole>(role, true, out var parsedRole))
            {
                parsedRole = UserRole.Customer;
            }

            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Email = email,
                FullName = $"{firstName} {lastName}",
                PasswordHash = password, 
                Role = parsedRole,
                IsActive = true
            };

            await _unitOfWork.Users.AddAsync(newUser);
            await _unitOfWork.CompleteAsync();

            return (true, Array.Empty<string>());
        }

        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            if (!Guid.TryParse(userId, out var guidId)) return false;

            var user = await _unitOfWork.Users.GetByIdAsync(guidId);
            if (user == null) return false;

            return user.Role.ToString().Equals(role, StringComparison.OrdinalIgnoreCase);
        }
    }
}