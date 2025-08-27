# CrudWithMsSql - Employee Management System

A comprehensive console-based CRUD application using **MVC Architecture** with .NET 8, Entity Framework Core, and SQL Server. This project demonstrates proper software architecture, input validation, and database operations.

---

## 📁 Project Structure Overview

```
CrudWithMsSql/
├── Controllers/           # MVC Controllers (User Interface & Business Logic)
│   ├── EmployeeController.cs
│   └── DepartmentController.cs
├── Models/               # Data Models (Entity Classes)
│   ├── Employee.cs
│   └── Department.cs
├── Storage/              # Data Access Layer (Repository Pattern)
│   ├── AppDbContext.cs
│   ├── EmployeeRepository.cs
│   └── DepartmentRepository.cs
├── Program.cs            # Application Entry Point
├── appsettings.json      # Configuration File
└── CrudWithMsSql.csproj  # Project Dependencies
```

---

## 🔧 File-by-File Breakdown

### 📄 **Program.cs** - Application Entry Point
**Purpose**: Bootstraps the application, sets up dependency injection, and runs the main menu loop.

**Key Parts**:
```csharp
// Configuration Setup
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();
```
- **What it does**: Reads configuration from `appsettings.json` file
- **Why**: Allows external configuration management

```csharp
// Dependency Injection Container
var services = new ServiceCollection();
services.AddSingleton<IConfiguration>(configuration);
services.AddDbContext<AppDbContext>(options => ...);
services.AddScoped<EmployeeRepository>();
services.AddScoped<DepartmentRepository>();
services.AddScoped<EmployeeController>();
services.AddScoped<DepartmentController>();
```
- **What it does**: Registers all services for dependency injection
- **Why**: Enables loose coupling and testability

```csharp
// Database Initialization
using var context = provider.GetRequiredService<AppDbContext>();
await context.Database.EnsureCreatedAsync();
```
- **What it does**: Creates database if it doesn't exist
- **Why**: Automatic database setup for new installations

```csharp
// Main Menu Loop
while (running)
{
    Console.Clear();
    // Display menu options
    // Handle user input
    // Route to appropriate controller
}
```
- **What it does**: Main application navigation
- **Why**: Provides user interface for choosing between Employee and Department management

---

### 🎮 **Controllers/** - MVC Controllers

#### **EmployeeController.cs** - Employee Management
**Purpose**: Handles all employee-related operations and user interactions.

**Key Methods**:

##### `ShowMenu()`
```csharp
public void ShowMenu()
{
    bool running = true;
    while (running)
    {
        // Display submenu options (1-4 + 0)
        // Handle user choice
        // Route to specific CRUD operation
    }
}
```
- **What it does**: Displays employee submenu with options 1-4 for CRUD operations
- **Why**: Provides organized navigation for employee operations

##### `ViewAll()`
```csharp
public void ViewAll()
{
    var employees = _employeeRepo.GetAll();
    // Display formatted employee list with null handling
    Console.WriteLine($"{empId,-5} {empName,-20} ${empSalary,-9} {deptName,-15}");
}
```
- **What it does**: Retrieves and displays all employees in a formatted table
- **Why**: Allows users to see current employee data
- **Validation**: Handles null values gracefully

##### `Create()`
```csharp
public void Create()
{
    // Name validation loop
    do {
        Console.Write("Enter employee name: ");
        name = Console.ReadLine()?.Trim();
        if (string.IsNullOrWhiteSpace(name))
            Console.WriteLine("Error: Name cannot be empty!");
    } while (string.IsNullOrWhiteSpace(name));
    
    // Salary validation loop
    do {
        Console.Write("Enter salary (numbers only): ");
        if (!int.TryParse(salaryInput, out salary) || salary < 0)
            Console.WriteLine("Error: Please enter a valid positive number!");
    } while (true);
    
    // Department ID validation
    // Create employee object
    // Save to database
}
```
- **What it does**: Creates new employee with comprehensive validation
- **Validation Features**:
  - Name: Cannot be null, empty, or whitespace
  - Salary: Must be valid positive integer (no strings allowed)
  - Department: Must exist in database
- **Why**: Ensures data integrity and prevents invalid entries

##### `Update()`
```csharp
public void Update()
{
    // ID validation
    // Employee existence check
    // Optional field updates with validation
    // Database update
}
```
- **What it does**: Updates existing employee with optional field changes
- **Validation**: ID must be valid, employee must exist, salary must be numeric if provided

##### `Delete()`
```csharp
public void Delete()
{
    // ID validation
    // Employee existence check
    // Confirmation prompt
    // Database deletion
}
```
- **What it does**: Safely deletes employee with confirmation
- **Safety Features**: Shows employee details before deletion, requires explicit confirmation

#### **DepartmentController.cs** - Department Management
**Purpose**: Handles all department-related operations.

**Key Methods** (similar structure to Employee but with department-specific logic):

##### `Create()`
- **Additional Validation**: Checks for duplicate department names
- **Purpose**: Prevents creating departments with identical names

##### `Delete()`
- **Additional Safety**: Warns if department has employees
- **Purpose**: Informs user about cascading deletes (employees will be deleted too)

---

### 📊 **Models/** - Data Models

#### **Employee.cs** - Employee Entity
```csharp
public class Employee
{
    public int? Id { get; set; }              // Primary Key (nullable for new records)
    public string? Name { get; set; }         // Employee name
    public int? Salary { get; set; }          // Employee salary
    public int? DepartmentId { get; set; }    // Foreign Key to Department
    public Department? Department { get; set; } // Navigation property
}
```
- **What it does**: Defines the structure of employee data
- **Nullable Properties**: Allows EF Core to handle new records and null values
- **Navigation Property**: Enables easy access to related department data

#### **Department.cs** - Department Entity
```csharp
public class Department
{
    public int? Id { get; set; }                    // Primary Key
    public string? Name { get; set; }               // Department name
    public List<Employee>? Employees { get; set; }  // Collection of employees
}
```
- **What it does**: Defines the structure of department data
- **Collection Property**: Allows access to all employees in the department

---

### 💾 **Storage/** - Data Access Layer

#### **AppDbContext.cs** - Entity Framework Context
```csharp
public class AppDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Table mappings
        modelBuilder.Entity<Employee>()
            .ToTable("employees")
            .HasKey(e => e.Id);
            
        // Column mappings (C# property -> DB column)
        modelBuilder.Entity<Employee>()
            .Property(e => e.Id)
            .HasColumnName("ID");
            
        // Relationships
        modelBuilder.Entity<Employee>()
            .HasOne(e => e.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DepartmentId);
    }
}
```
- **What it does**: Manages database connection and entity mappings
- **Key Features**:
  - Maps C# properties to database columns
  - Defines relationships between entities
  - Handles database table naming conventions

#### **EmployeeRepository.cs** - Employee Data Access
```csharp
public class EmployeeRepository
{
    private readonly AppDbContext _context;
    
    public List<Employee> GetAll()
    {
        return _context.Employees.Include(e => e.Department).ToList();
    }
    
    public void Create(Employee employee) { /* Add and save */ }
    public void Update(Employee employee) { /* Update and save */ }
    public void Delete(int id) { /* Find, remove, and save */ }
}
```
- **What it does**: Provides CRUD operations for Employee entity
- **Include()**: Loads related Department data (prevents N+1 queries)
- **Why Repository Pattern**: Separates data access from business logic

#### **DepartmentRepository.cs** - Department Data Access
- **Similar to EmployeeRepository** but for Department entity
- **Include()**: Loads related Employees collection

---

### ⚙️ **appsettings.json** - Configuration
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=employ;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True;"
  }
}
```
- **What it does**: Stores application configuration externally
- **Connection String Parts**:
  - `Server=.`: Local SQL Server instance
  - `Database=employ`: Database name
  - `Trusted_Connection=True`: Use Windows authentication
  - `Encrypt=False`: Disable SSL (for local development)

### 📦 **CrudWithMsSql.csproj** - Project File
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.8" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="9.0.8" />
<PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="9.0.8" />
```
- **What it does**: Defines project dependencies and build settings
- **Key Packages**:
  - EF Core: Object-relational mapping
  - SQL Server provider: Database connectivity
  - Configuration: JSON file reading

---

## 🛡️ Validation Features

### Input Validation
1. **Name Fields**: Cannot be null, empty, or whitespace-only
2. **Salary**: Must be valid positive integers (rejects strings like "abc")
3. **IDs**: Must be valid integers and exist in database
4. **Duplicate Prevention**: Department names must be unique

### Error Handling
- **Graceful Degradation**: Invalid input shows error and prompts retry
- **User-Friendly Messages**: Clear error descriptions
- **Null Safety**: All null values handled with default displays

### Safety Features
- **Confirmation Dialogs**: Delete operations require explicit confirmation
- **Cascade Warnings**: Warns when deleting departments with employees
- **Data Integrity**: Foreign key relationships maintained

---

## 🏗️ Architecture Benefits

### MVC Pattern
- **Models**: Clean data representation
- **Views**: Console interface (handled by controllers)
- **Controllers**: Business logic and user interaction

### Repository Pattern
- **Separation of Concerns**: Data access isolated from business logic
- **Testability**: Easy to mock repositories for unit tests
- **Maintainability**: Database changes don't affect controllers

### Dependency Injection
- **Loose Coupling**: Components depend on abstractions
- **Extensibility**: Easy to swap implementations
- **Testing**: Components can be tested in isolation

---

## 🚀 How to Run

1. **Prerequisites**: .NET 8.0 SDK, SQL Server
2. **Run**: `dotnet run`
3. **Navigate**: Choose 1 for Employees, 2 for Departments
4. **CRUD**: Each section has options 1-4 for Create, Read, Update, Delete

The application automatically creates the database on first run and provides comprehensive validation to ensure data integrity throughout all operations.
