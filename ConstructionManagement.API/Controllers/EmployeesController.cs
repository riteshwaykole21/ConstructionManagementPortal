using ConstructionManagement.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using ConstructionManagement.API.DTOs;

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
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _context.Employees.ToListAsync();
            return Ok(employees);
        }
        [HttpPost]

        public async Task<IActionResult> CreateEmployee(EmployeeDto employeeDto)
        {
            var employee = new Employee
            {
                FullName = employeeDto.FullName,
                Email = employeeDto.Email,
                Designation = employeeDto.Designation,
                JoiningDate = employeeDto.JoiningDate,
                IsActive = employeeDto.IsActive
            };

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
        public async Task<IActionResult> UpdateEmployee(int id, EmployeeDto employeeDto)
        {
            var existingEmployee = await _context.Employees.FindAsync(id);

            if (existingEmployee == null)
            {
                return NotFound();
            }

            existingEmployee.FullName = employeeDto.FullName;
            existingEmployee.Email = employeeDto.Email;
            existingEmployee.Designation = employeeDto.Designation;
            existingEmployee.JoiningDate = employeeDto.JoiningDate;
            existingEmployee.IsActive = employeeDto.IsActive;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
} 

