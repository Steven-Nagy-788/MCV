using System;
using System.Collections.Generic;

namespace _4_task_auth_Sql_Scaffolding.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? Salary { get; set; }

    public int? DepId { get; set; }

    public virtual Department? Dep { get; set; }
}
