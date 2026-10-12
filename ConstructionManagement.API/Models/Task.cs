using System;
using System.Collections.Generic;

namespace ConstructionManagement.API.Models;

public partial class Task
{
    public int TaskId { get; set; }

    public string TaskName { get; set; } = null!;

    public string? Description { get; set; }

    public int ProjectId { get; set; }

    public int? AssignedTo { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? DueDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual Employee? AssignedToNavigation { get; set; }

    public virtual Project Project { get; set; } = null!;
}
