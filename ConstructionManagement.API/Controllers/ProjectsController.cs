using ConstructionManagement.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConstructionManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly ConstructionManagementDbContext _context;

        public ProjectsController(ConstructionManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var projects = await _context.Projects.ToListAsync();

            return Ok(projects);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult>GetProject(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid Project ID");
            }
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound("Project not found");
            }
            return Ok(project);
        }

        [HttpPost]
        public async Task<ActionResult<Project>> PostProject(Project project)
        {
            if (string.IsNullOrWhiteSpace(project.Location))
            {
                return BadRequest("Location is required.");
            }

            if (project.StartDate.HasValue &&
                project.EndDate.HasValue &&
                project.EndDate < project.StartDate)
            {
                return BadRequest("End date cannot be earlier than start date.");
            }

            if (project.Budget.HasValue && project.Budget < 0)
            {
                return BadRequest("Budget cannot be negative.");
            }

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return StatusCode(201, project);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, Project project)
        {
            if (id <= 0)
            {
                return BadRequest("Project ID must be greater than zero.");
            }

            if (id != project.ProjectId)
            {
                return BadRequest("Project ID mismatch.");
            }

            if (string.IsNullOrWhiteSpace(project.Location))
            {
                return BadRequest("Location is required.");
            }

            if (project.StartDate.HasValue &&
                project.EndDate.HasValue &&
                project.EndDate < project.StartDate)
            {
                return BadRequest("End date cannot be earlier than start date.");
            }

            if (project.Budget.HasValue && project.Budget < 0)
            {
                return BadRequest("Budget cannot be negative.");
            }

            var existingProject = await _context.Projects.FindAsync(id);

            if (existingProject == null)
            {
                return NotFound("Project not found.");
            }

            existingProject.Location = project.Location;
            existingProject.StartDate = project.StartDate;
            existingProject.EndDate = project.EndDate;
            existingProject.Budget = project.Budget;
            existingProject.Status = project.Status;

            await _context.SaveChangesAsync();

            return Ok(existingProject);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Project ID must be greater than zero.");
            }

            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound("Project not found.");
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
