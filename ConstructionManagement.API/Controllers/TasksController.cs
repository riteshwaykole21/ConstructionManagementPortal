using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConstructionManagement.API.Models;

namespace ConstructionManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ConstructionManagementDbContext _context;

        public TasksController(ConstructionManagementDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            var tasks = await _context.Tasks.ToListAsync();

            return Ok(tasks);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid Task ID");
            }

            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound("Task not found");
            }

            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(CreateTaskDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.TaskName))
            {
                return BadRequest("Task Name is required");
            }

            if (string.IsNullOrWhiteSpace(dto.Status))
            {
                return BadRequest("Status is required");
            }

            if (dto.ProjectId <= 0)
            {
                return BadRequest("Valid Project ID is required");
            }

            var projectExists = await _context.Projects
                .AnyAsync(p => p.ProjectId == dto.ProjectId);

            if (!projectExists)
            {
                return BadRequest("Project does not exist");
            }

            if (dto.AssignedTo.HasValue)
            {
                var employeeExists = await _context.Employees
                    .AnyAsync(e => e.EmployeeId == dto.AssignedTo.Value);

                if (!employeeExists)
                {
                    return BadRequest("Assigned Employee does not exist");
                }
            }

            if (dto.StartDate.HasValue &&
                dto.DueDate.HasValue &&
                dto.DueDate < dto.StartDate)
            {
                return BadRequest("Due Date cannot be earlier than Start Date");
            }

            var task = new ConstructionManagement.API.Models.Task
            {
                TaskName = dto.TaskName,
                Description = dto.Description,
                ProjectId = dto.ProjectId,
                AssignedTo = dto.AssignedTo,
                StartDate = dto.StartDate,
                DueDate = dto.DueDate,
                Status = dto.Status
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetTask),
                new { id = task.TaskId },
                task);
        }
    }
}
