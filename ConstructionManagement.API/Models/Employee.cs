using System;
using System.Collections.Generic;

namespace ConstructionManagement.API.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Designation { get; set; }

    public DateOnly? JoiningDate { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
