using CrudWithMsSql.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using CrudWithMsSql.Controllers;

// Build configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var services = new ServiceCollection();

// Add configuration
services.AddSingleton<IConfiguration>(configuration);

// Add DbContext
services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

services.AddScoped<EmployeeRepository>();
services.AddScoped<DepartmentRepository>();

services.AddScoped<EmployeeController>();
services.AddScoped<DepartmentController>();

var provider = services.BuildServiceProvider();

try
{
    using var context = provider.GetRequiredService<AppDbContext>();
    await context.Database.EnsureCreatedAsync();
    Console.WriteLine("Database connection established successfully.\n");

    bool running = true;
    while (running)
    {
        Console.Clear();
        Console.WriteLine("=== EMPLOYEE MANAGEMENT SYSTEM ===");
        Console.WriteLine("1. Manage Employees");
        Console.WriteLine("2. Manage Departments");
        Console.WriteLine("0. Exit");
        Console.Write("Choose an option: ");

        var choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                var empController = provider.GetRequiredService<EmployeeController>();
                empController.ShowMenu();
                break;
            case "2":
                var deptController = provider.GetRequiredService<DepartmentController>();
                deptController.ShowMenu();
                break;
            case "0":
                running = false;
                Console.WriteLine("Goodbye!");
                break;
            default:
                Console.WriteLine("Invalid choice! Press any key to continue...");
                Console.ReadKey();
                break;
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
}
