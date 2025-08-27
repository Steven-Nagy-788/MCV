using CrudWithMsSql.Models;
using Microsoft.EntityFrameworkCore;

namespace CrudWithMsSql.Storage
{
    public class EmployeeRepository
    {
        private readonly AppDbContext _context;
        public EmployeeRepository(AppDbContext context) => _context = context;

        public List<Employee> GetAll()
        {
            return _context.Employees.Include(e => e.Department).ToList();
        }

        public void Create(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
        }

        public void Update(Employee employee)
        {
            _context.Employees.Update(employee);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var emp = _context.Employees.Find(id);
            if (emp != null)
            {
                _context.Employees.Remove(emp);
                _context.SaveChanges();
            }
        }
    }
}
