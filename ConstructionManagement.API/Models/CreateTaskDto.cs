using System;
using System.ComponentModel.DataAnnotations;

namespace ConstructionManagement.API.Models
{
    public class CreateTaskDto
    {
        [Required]
        public string TaskName { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Range(1, int.MaxValue)]
        public int ProjectId { get; set; }

        public int? AssignedTo { get; set; }

        public DateOnly? StartDate { get; set; }

        public DateOnly? DueDate { get; set; }

        [Required]
        public string Status { get; set; } = string.Empty;
    }
}