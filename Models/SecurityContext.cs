using Microsoft.EntityFrameworkCore;

namespace Security.Models;

public class SecurityContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public SecurityContext(DbContextOptions<SecurityContext> options) : base(options) { }
}
