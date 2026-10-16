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
        [HttpPost]
        public async Task<IActionResult> CreateEmployee(Employee employee)
        {
            _context.Employees.Add(employee);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetEmployees),
                new { id = employee.EmployeeId },
                employee
            );
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return BadRequest("Employee ID mismatch.");
            }

            var existingEmployee = await _context.Employees
                .FindAsync(id);

            if (existingEmployee == null)
            {
                return NotFound();
            }

            existingEmployee.FullName = employee.FullName;
            existingEmployee.Email = employee.Email;
            existingEmployee.Designation = employee.Designation;
            existingEmployee.JoiningDate = employee.JoiningDate;
            existingEmployee.IsActive = employee.IsActive;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
