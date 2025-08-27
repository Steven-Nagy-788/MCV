using CrudWithMsSql.Models;
using Microsoft.EntityFrameworkCore;

namespace CrudWithMsSql.Storage
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }

    }
}
