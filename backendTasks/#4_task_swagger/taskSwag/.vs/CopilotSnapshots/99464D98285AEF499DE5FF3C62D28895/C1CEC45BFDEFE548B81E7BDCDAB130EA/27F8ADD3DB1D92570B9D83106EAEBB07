using Microsoft.EntityFrameworkCore;
using taskSwag.Models;
namespace taskSwag.Models
{
    public class UserDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Location> Locations { get; set; }
    }
}