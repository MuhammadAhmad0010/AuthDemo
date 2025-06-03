using AuthDemos.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthDemo.Infrastructure.EFCore
{
    public class AuthDemoDbContext : IdentityDbContext
    {
        public AuthDemoDbContext(DbContextOptions options) : base(options) 
        {
        }

        public DbSet<UserRefreshTokens> UserRefreshTokens { get; set; }
    }
}
