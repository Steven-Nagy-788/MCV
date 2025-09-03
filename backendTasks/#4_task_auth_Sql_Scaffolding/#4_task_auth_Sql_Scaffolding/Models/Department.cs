using System;
using System.Collections.Generic;

namespace _4_task_auth_Sql_Scaffolding.Models;

public partial class Department
{
    public int DepId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
