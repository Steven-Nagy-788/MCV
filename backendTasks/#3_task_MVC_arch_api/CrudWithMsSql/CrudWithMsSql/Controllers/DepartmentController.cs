using CrudWithMsSql.Models;
using CrudWithMsSql.Storage;

namespace CrudWithMsSql.Controllers
{
    public class DepartmentController
    {
        private readonly DepartmentRepository _departmentRepo;

        public DepartmentController(DepartmentRepository departmentRepo)
        {
            _departmentRepo = departmentRepo;
        }

        public void ShowMenu()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("=== DEPARTMENT MANAGEMENT ===");
                Console.WriteLine("1. View All Departments");
                Console.WriteLine("2. Create New Department");
                Console.WriteLine("3. Update Department");
                Console.WriteLine("4. Delete Department");
                Console.WriteLine("0. Back to Main Menu");
                Console.Write("Choose an option: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ViewAll();
                        break;
                    case "2":
                        Create();
                        break;
                    case "3":
                        Update();
                        break;
                    case "4":
                        Delete();
                        break;
                    case "0":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice!");
                        break;
                }

                if (running)
                {
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        public void ViewAll()
        {
            Console.Clear();
            var departments = _departmentRepo.GetAll();
            
            if (departments.Any())
            {;
                foreach (var dept in departments)
                {   
                    Console.WriteLine(dept);
                }
            }
            else
            {
                Console.WriteLine("No departments found.");
            }
        }

        public void Create()
        {
            Console.Clear();
            
            string name;
            do
            {
                Console.Write("Enter department name: ");
                name = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("Error: Department name cannot be empty! Please try again.");
                }
            } while (string.IsNullOrWhiteSpace(name));

            var existingDepts = _departmentRepo.GetAll();
            if (existingDepts.Any(d => d.Name?.ToLower() == name.ToLower()))
            {
                Console.WriteLine("Error: A department with this name already exists!");
                return;
            }
            var department = new Department
            {
                Name = name
            };

            _departmentRepo.Create(department);
        }

        public void Update()
        {
            Console.Clear();
            ViewAll();
            
            Console.Write("\nEnter department ID to update: ");
            var idInput = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(idInput) || !int.TryParse(idInput, out int id))
            {
                Console.WriteLine("Error: Please enter a valid department ID!");
                return;
            }

            var departments = _departmentRepo.GetAll();
            var department = departments.FirstOrDefault(d => d.Id == id);
            if (department == null)
            {
                Console.WriteLine("Error: Department not found!");
                return;
            }

            Console.WriteLine($"Current name: {department.Name}");
            
            string newName;
            do
            {
                Console.Write("Enter new name: ");
                newName = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(newName))
                {
                    Console.WriteLine("Error: Department name cannot be empty! Please try again.");
                }
            } while (string.IsNullOrWhiteSpace(newName));

            // Check if new name already exists (excluding current department)
            if (departments.Any(d => d.Id != id && d.Name?.ToLower() == newName.ToLower()))
            {
                Console.WriteLine("Error: A department with this name already exists!");
                return;
            }

            department.Name = newName;
            _departmentRepo.Update(department);
            Console.WriteLine("Department updated successfully!");
        }

        public void Delete()
        {
            Console.Clear();
            Console.WriteLine("=== DELETE DEPARTMENT ===");
            ViewAll();
            
            Console.Write("\nEnter department ID to delete: ");
            var idInput = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(idInput) || !int.TryParse(idInput, out int id))
            {
                Console.WriteLine("Error: Please enter a valid department ID!");
                return;
            }

            var departments = _departmentRepo.GetAll();
            var department = departments.FirstOrDefault(d => d.Id == id);
            if (department == null)
            {
                Console.WriteLine("Error: Department not found!");
                return;
            }

            // Check if department has employees
            if (department.Employees != null && department.Employees.Any())
            {
                Console.WriteLine($"Warning: This department has {department.Employees.Count} employee(s).");
                Console.WriteLine("Deleting the department will also delete all employees in it!");
            }

            Console.WriteLine($"Department to delete: {department.Name}");
            Console.Write("Are you sure you want to delete this department? (y/N): ");
            var confirmation = Console.ReadLine()?.Trim().ToLower();
            if (confirmation == "y" || confirmation == "yes")
            {
                _departmentRepo.Delete(id);
                Console.WriteLine("Department deleted successfully!");
            }
            else
            {
                Console.WriteLine("Delete cancelled.");
            }
        }
    }
}
