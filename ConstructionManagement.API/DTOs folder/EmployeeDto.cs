using System.ComponentModel.DataAnnotations;

namespace ConstructionManagement.API.DTOs
{
    public class EmployeeDto
    {
        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "Full name must be between 2 and 100 characters.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        public string? Designation { get; set; }

        public DateOnly? JoiningDate { get; set; }

        public bool IsActive { get; set; }
    }
}
