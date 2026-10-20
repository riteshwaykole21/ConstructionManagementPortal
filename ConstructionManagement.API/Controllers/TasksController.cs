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
    }
   
}
