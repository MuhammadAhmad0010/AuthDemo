using AuthDemo.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;

namespace AuthDemo.API.ServiceCollectionExtensions
{
    public static class DbContextConfig
    {
        public static IServiceCollection AddAuthDemoDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddDbContext<AuthDemoDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
