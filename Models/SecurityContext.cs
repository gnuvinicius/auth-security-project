using Microsoft.EntityFrameworkCore;

namespace Security.Models;

public class SecurityContext : DbContext
{
    public SecurityContext(DbContextOptions<SecurityContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
}
