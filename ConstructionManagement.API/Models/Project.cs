using System;
using System.Collections.Generic;

namespace ConstructionManagement.API.Models;

public partial class Project
{
    public int ProjectId { get; set; }

    public string Location { get; set; } = null!;

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public decimal? Budget { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
