using Microsoft.EntityFrameworkCore;

namespace taskMCV.Models
{
    public class TodoDbContext: DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options) {}
        public DbSet<Todo> Todos { get; set; }
    }
}
