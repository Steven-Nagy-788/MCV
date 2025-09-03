using Microsoft.EntityFrameworkCore;

namespace _4_task_auth_Sql_Scaffolding.Data
{
    public class CompanyDbContext : DbContext 
    {
        public CompanyDbContext(DbContextOptions<CompanyDbContext> options) : base(options)
        {
        }
        public DbSet<Models.Employee> Employees { get; set; }
        public DbSet<Models.Department> Departments { get; set; }
    }
}
