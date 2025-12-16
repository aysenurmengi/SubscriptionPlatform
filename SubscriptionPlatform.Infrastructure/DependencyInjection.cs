using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SubscriptionPlatform.Application.Interfaces;
using SubscriptionPlatform.Application.Interfaces.Repositories;
using SubscriptionPlatform.Infrastructure.Persistence;
using SubscriptionPlatform.Infrastructure.Services;

namespace SubscriptionPlatform.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            services.AddTransient<IPaymentService, FakePaymentService>();

            return services;
        }
    }
}