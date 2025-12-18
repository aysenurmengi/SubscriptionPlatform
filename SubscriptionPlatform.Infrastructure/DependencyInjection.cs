using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SubscriptionPlatform.Application.Interfaces;
using SubscriptionPlatform.Application.Interfaces.Repositories;
using SubscriptionPlatform.Infrastructure.Configuration;
using SubscriptionPlatform.Infrastructure.Persistence;
using SubscriptionPlatform.Infrastructure.Services;

namespace SubscriptionPlatform.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
            );

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IIdentityService, IdentityService>();

            //buraya isteğe bağlı repository eklemeleri yapılabilir
            //services.AddScoped<IUserRepository, UserRepository>(); gibi..
            
            services.AddTransient<IPaymentService, FakePaymentService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddScoped<IFulfillmentService, FulfillmentService>();

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

            return services;
        }
    }
}