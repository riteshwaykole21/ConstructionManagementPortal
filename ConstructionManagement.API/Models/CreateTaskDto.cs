
using System;
using System.ComponentModel.DataAnnotations;

namespace ConstructionManagement.API.Models
{
    public class CreateTaskDto
    {
        [Required]
        [StringLength(150)]
        public string TaskName { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Range(1, int.MaxValue)]
        public int ProjectId { get; set; }

        public int? AssignedTo { get; set; }

        public DateOnly? StartDate { get; set; }

        public DateOnly? DueDate { get; set; }

        [Required]
        [StringLength(30)]
        public string Status { get; set; } = string.Empty;
    }
}
