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
    }
   
}
