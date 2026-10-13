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
        [HttpPost]
        public async Task<ActionResult<Project>> PostProject(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return StatusCode(201, project);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, Project project)
        {
            if (id != project.ProjectId)
            {
                return BadRequest("Project ID mismatch.");
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
