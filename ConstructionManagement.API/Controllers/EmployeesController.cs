using ConstructionManagement.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConstructionManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ConstructionManagementDbContext _context;
        public EmployeesController(ConstructionManagementDbContext context) 
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult>GetEmployees()
        {
            var employees = await _context.Employees.ToListAsync();
            return Ok(employees);
        }
    }
}
