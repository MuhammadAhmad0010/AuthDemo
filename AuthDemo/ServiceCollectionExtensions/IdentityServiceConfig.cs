using AuthDemo.Infrastructure.EFCore;
using Microsoft.AspNetCore.Identity;

namespace AuthDemo.API.ServiceCollectionExtensions
{
    public static class IdentityServiceConfig
    {
        public static IServiceCollection AddIdentityService(this IServiceCollection services)
        {
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                  options.Password.RequireDigit = true;
                  options.Password.RequireLowercase = true;
                  options.Password.RequireNonAlphanumeric = false;
                  options.Password.RequireUppercase = true;
                  options.Password.RequiredLength = 8;
                  options.Password.RequiredUniqueChars = 1;
                  options.User.RequireUniqueEmail = true;
                  options.SignIn.RequireConfirmedEmail = false;

                  //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                  //options.Lockout.MaxFailedAccessAttempts = 5;
                  //options.Lockout.AllowedForNewUsers = true;
            })
                .AddEntityFrameworkStores<AuthDemoDbContext>()
                .AddDefaultTokenProviders();

            return services;

        }
    }
}
