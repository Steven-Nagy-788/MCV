using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using taskMCV.Models;
using taskMCV.Controllers;

// Build configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

// Setup services
var services = new ServiceCollection();
services.AddDbContext<TodoDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
services.AddScoped<TodoController>();

var serviceProvider = services.BuildServiceProvider();

// Ensure database is created
using (var scope = serviceProvider.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
    context.Database.EnsureCreated();
}

Console.WriteLine("=== Todo Console Application ===");
Console.WriteLine();

while (true)
{
    ShowMenu();
    var choice = Console.ReadLine();

    using (var scope = serviceProvider.CreateScope())
    {
        var controller = scope.ServiceProvider.GetRequiredService<TodoController>();
        
        try
        {
            switch (choice)
            {
                case "1":
                    await CreateTodo(controller);
                    break;
                case "2":
                    await ShowAllTodos(controller);
                    break;
                case "3":
                    await GetTodoById(controller);
                    break;
                case "4":
                    await UpdateTodo(controller);
                    break;
                case "5":
                    await DeleteTodo(controller);
                    break;
                case "6":
                    Console.WriteLine("Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
    
    Console.WriteLine("\nPress any key to continue...");
    Console.ReadKey();
    Console.Clear();
}

static void ShowMenu()
{
    Console.WriteLine("Choose an option:");
    Console.WriteLine("1. Create Todo");
    Console.WriteLine("2. Show All Todos");
    Console.WriteLine("3. Get Todo by ID");
    Console.WriteLine("4. Update Todo");
    Console.WriteLine("5. Delete Todo");
    Console.WriteLine("6. Exit");
    Console.Write("Enter your choice (1-6): ");
}

static async Task CreateTodo(TodoController controller)
{
    Console.WriteLine("\n=== Create New Todo ===");
    
    Console.Write("Enter todo title: ");
    var title = Console.ReadLine();
    
    // Validation
    if (string.IsNullOrWhiteSpace(title))
    {
        Console.WriteLine("Error: Title cannot be empty!");
        return;
    }
    
    if (title.Length > 200)
    {
        Console.WriteLine("Error: Title cannot exceed 200 characters!");
        return;
    }
    
    var todo = new Todo
    {
        Title = title.Trim(),
        IsCompleted = false
    };
    
    await controller.CreateTodo(todo);
    Console.WriteLine("Todo created successfully!");
}

static async Task ShowAllTodos(TodoController controller)
{
    Console.WriteLine("\n=== All Todos ===");
    
    var todos = await controller.GetAllTodos();
    
    if (todos.Count == 0)
    {
        Console.WriteLine("No todos found.");
        return;
    }
    
    Console.WriteLine($"{"ID",-5} {"Title",-30} {"Status",-10}");
    Console.WriteLine(new string('-', 50));
    
    foreach (var todo in todos)
    {
        var status = todo.IsCompleted ? "Completed" : "Pending";
        Console.WriteLine($"{todo.Id,-5} {todo.Title,-30} {status,-10}");
    }
}

static async Task GetTodoById(TodoController controller)
{
    Console.WriteLine("\n=== Get Todo by ID ===");
    
    Console.Write("Enter todo ID: ");
    var input = Console.ReadLine();
    
    // Validation
    if (!int.TryParse(input, out int id) || id <= 0)
    {
        Console.WriteLine("Error: Please enter a valid positive number!");
        return;
    }
    
    var todo = await controller.GetTodoById(id);
    
    if (todo == null)
    {
        Console.WriteLine($"Todo with ID {id} not found.");
        return;
    }
    
    Console.WriteLine($"ID: {todo.Id}");
    Console.WriteLine($"Title: {todo.Title}");
    Console.WriteLine($"Status: {(todo.IsCompleted ? "Completed" : "Pending")}");
}
static async Task UpdateTodo(TodoController controller)
{
    Console.WriteLine("\n=== Update Todo ===");
    
    Console.Write("Enter todo ID to update: ");
    var input = Console.ReadLine();
    
    // Validation
    if (!int.TryParse(input, out int id) || id <= 0)
    {
        Console.WriteLine("Error: Please enter a valid positive number!");
        return;
    }
    
    var existingTodo = await controller.GetTodoById(id);
    if (existingTodo == null)
    {
        Console.WriteLine($"Todo with ID {id} not found.");
        return;
    }
    
    Console.WriteLine($"Current title: {existingTodo.Title}");
    Console.Write("Enter new title (or press Enter to keep current): ");
    var newTitle = Console.ReadLine();
    
    if (!string.IsNullOrEmpty(newTitle))
    {
        if (newTitle.Length > 200)
        {
            Console.WriteLine("Error: Title cannot exceed 200 characters!");
            return;
        }
        existingTodo.Title = newTitle.Trim();
    }
    
    Console.WriteLine($"Current status: {(existingTodo.IsCompleted ? "Completed" : "Pending")}");
    Console.Write("Mark as completed? (y/n, or press Enter to keep current): ");
    var statusInput = Console.ReadLine()?.ToLower();
    
    if (statusInput == "y" || statusInput == "yes")
    {
        existingTodo.IsCompleted = true;
    }
    else if (statusInput == "n" || statusInput == "no")
    {
        existingTodo.IsCompleted = false;
    }
    
    await controller.UpdateTodo(id, existingTodo);
    Console.WriteLine("Todo updated successfully!");
}

static async Task DeleteTodo(TodoController controller)
{
    Console.WriteLine("\n=== Delete Todo ===");
    
    Console.Write("Enter todo ID to delete: ");
    var input = Console.ReadLine();
    
    // Validation
    if (!int.TryParse(input, out int id) || id <= 0)
    {
        Console.WriteLine("Error: Please enter a valid positive number!");
        return;
    }
    
    var existingTodo = await controller.GetTodoById(id);
    if (existingTodo == null)
    {
        Console.WriteLine($"Todo with ID {id} not found.");
        return;
    }
    
    Console.WriteLine($"Todo to delete: {existingTodo.Title}");
    Console.Write("Are you sure you want to delete this todo? (y/n): ");
    var confirmation = Console.ReadLine()?.ToLower();
    
    if (confirmation == "y" || confirmation == "yes")
    {
        await controller.DeleteTodo(id);
        Console.WriteLine("Todo deleted successfully!");
    }
    else
    {
        Console.WriteLine("Delete operation cancelled.");
    }
}
