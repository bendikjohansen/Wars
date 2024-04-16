using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Wars.Users.Domain;

namespace Wars.Users.Infrastructure.Data;

internal class UsersDbContext(DbContextOptions<UsersDbContext> options) : IdentityDbContext(options)
{
    public required DbSet<ApplicationUser> ApplicationUsers { get; init; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("Users");
        base.OnModelCreating(builder);
    }
}
