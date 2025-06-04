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
        public DbSet<Author> Author {  get; set; }
        public DbSet<Books> Books { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<BookCategory> BookCategory { get; set; }
    }
}
