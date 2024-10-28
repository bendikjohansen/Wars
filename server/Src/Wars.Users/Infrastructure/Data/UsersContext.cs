using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Wars.Users.Domain;

namespace Wars.Users.Infrastructure.Data;

internal class UsersContext(DbContextOptions<UsersContext> options) : IdentityDbContext(options)
{
    public DbSet<ApplicationUser> ApplicationUsers => Set<ApplicationUser>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("Users");
        base.OnModelCreating(builder);
    }
}
