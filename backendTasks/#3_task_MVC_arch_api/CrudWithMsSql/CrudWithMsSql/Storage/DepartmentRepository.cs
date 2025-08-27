using CrudWithMsSql.Models;
using Microsoft.EntityFrameworkCore;

namespace CrudWithMsSql.Storage
{
    public class DepartmentRepository
    {
        private readonly AppDbContext _context;
        public DepartmentRepository(AppDbContext context) => _context = context;

        public List<Department> GetAll()
        {
            return _context.Departments.Include(d => d.Employees).ToList();
        }

        public void Create(Department department)
        {
            _context.Departments.Add(department);
            _context.SaveChanges();
        }

        public void Update(Department department)
        {
            _context.Departments.Update(department);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var dep = _context.Departments.Find(id);
            if (dep != null)
            {
                _context.Departments.Remove(dep);
                _context.SaveChanges();
            }
        }
    }
}
