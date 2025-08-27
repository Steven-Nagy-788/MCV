using CrudWithMsSql.Models;
using CrudWithMsSql.Storage;

namespace CrudWithMsSql.Controllers
{
    public class EmployeeController
    {
        private readonly EmployeeRepository _employeeRepo;
        private readonly DepartmentRepository _departmentRepo;

        public EmployeeController(EmployeeRepository employeeRepo, DepartmentRepository departmentRepo)
        {
            _employeeRepo = employeeRepo;
            _departmentRepo = departmentRepo;
        }

        public void ShowMenu()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("=== EMPLOYEE MANAGEMENT ===");
                Console.WriteLine("1. View All Employees");
                Console.WriteLine("2. Create New Employee");
                Console.WriteLine("3. Update Employee");
                Console.WriteLine("4. Delete Employee");
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
            Console.WriteLine("=== ALL EMPLOYEES ===");
            var employees = _employeeRepo.GetAll();
            
            if (employees.Any())
            {
                Console.WriteLine($"{"ID",-5} {"Name",-20} {"Salary",-10} {"Department",-15}");
                Console.WriteLine(new string('-', 55));
                foreach (var emp in employees)
                {
                    var empId = emp.Id?.ToString() ?? "N/A";
                    var empName = emp.Name ?? "Unknown";
                    var empSalary = emp.Salary?.ToString() ?? "0";
                    var deptName = emp.Department?.Name ?? "None";
                    
                    Console.WriteLine($"{empId,-5} {empName,-20} ${empSalary,-9} {deptName,-15}");
                }
            }
            else
            {
                Console.WriteLine("No employees found.");
            }
        }

        public void Create()
        {
            Console.Clear();
            Console.WriteLine("=== CREATE NEW EMPLOYEE ===");
            
            // Name validation
            string name;
            do
            {
                Console.Write("Enter employee name: ");
                name = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("Error: Name cannot be empty! Please try again.");
                }
            } while (string.IsNullOrWhiteSpace(name));
            
            // Salary validation
            int salary;
            do
            {
                Console.Write("Enter salary (numbers only): ");
                var salaryInput = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(salaryInput))
                {
                    Console.WriteLine("Error: Salary cannot be empty! Please try again.");
                    continue;
                }
                if (!int.TryParse(salaryInput, out salary) || salary < 0)
                {
                    Console.WriteLine("Error: Please enter a valid positive number for salary!");
                    continue;
                }
                break;
            } while (true);

            // Department validation
            var departments = _departmentRepo.GetAll();
            if (!departments.Any())
            {
                Console.WriteLine("Error: No departments available! Please create a department first.");
                return;
            }

            Console.WriteLine("\nAvailable Departments:");
            foreach (var dept in departments)
            {
                Console.WriteLine($"{dept.Id} - {dept.Name}");
            }
            
            int deptId;
            do
            {
                Console.Write("Enter department ID: ");
                var deptInput = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(deptInput))
                {
                    Console.WriteLine("Error: Department ID cannot be empty! Please try again.");
                    continue;
                }
                if (!int.TryParse(deptInput, out deptId))
                {
                    Console.WriteLine("Error: Please enter a valid number for department ID!");
                    continue;
                }
                if (!departments.Any(d => d.Id == deptId))
                {
                    Console.WriteLine("Error: Department not found! Please choose from the list above.");
                    continue;
                }
                break;
            } while (true);

            var employee = new Employee
            {
                Name = name,
                Salary = salary,
                DepartmentId = deptId
            };

            _employeeRepo.Create(employee);
            Console.WriteLine("Employee created successfully!");
        }

        public void Update()
        {
            Console.Clear();
            Console.WriteLine("=== UPDATE EMPLOYEE ===");
            ViewAll();
            
            Console.Write("\nEnter employee ID to update: ");
            var idInput = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(idInput) || !int.TryParse(idInput, out int id))
            {
                Console.WriteLine("Error: Please enter a valid employee ID!");
                return;
            }

            var employees = _employeeRepo.GetAll();
            var employee = employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                Console.WriteLine("Error: Employee not found!");
                return;
            }

            Console.WriteLine($"Current: {employee.Name} - ${employee.Salary}");
            
            // Name validation
            Console.Write("Enter new name (or press Enter to keep current): ");
            var newName = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(newName))
            {
                employee.Name = newName;
            }

            // Salary validation
            Console.Write("Enter new salary (or press Enter to keep current): ");
            var salaryInput = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(salaryInput))
            {
                if (int.TryParse(salaryInput, out int newSalary) && newSalary >= 0)
                {
                    employee.Salary = newSalary;
                }
                else
                {
                    Console.WriteLine("Warning: Invalid salary entered. Keeping current salary.");
                }
            }

            _employeeRepo.Update(employee);
            Console.WriteLine("Employee updated successfully!");
        }

        public void Delete()
        {
            Console.Clear();
            Console.WriteLine("=== DELETE EMPLOYEE ===");
            ViewAll();
            
            Console.Write("\nEnter employee ID to delete: ");
            var idInput = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(idInput) || !int.TryParse(idInput, out int id))
            {
                Console.WriteLine("Error: Please enter a valid employee ID!");
                return;
            }

            // Check if employee exists
            var employees = _employeeRepo.GetAll();
            var employee = employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                Console.WriteLine("Error: Employee not found!");
                return;
            }

            Console.WriteLine($"Employee to delete: {employee.Name} - ${employee.Salary}");
            Console.Write("Are you sure you want to delete this employee? (y/N): ");
            var confirmation = Console.ReadLine()?.Trim().ToLower();
            if (confirmation == "y" || confirmation == "yes")
            {
                _employeeRepo.Delete(id);
                Console.WriteLine("Employee deleted successfully!");
            }
            else
            {
                Console.WriteLine("Delete cancelled.");
            }
        }
    }
}
