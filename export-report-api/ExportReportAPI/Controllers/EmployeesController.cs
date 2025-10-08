using ExportReportAPI.Data;
using ExportReportAPI.Models;
using ExportReportAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExportReportAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IExportService _exportService;

        public EmployeesController(ApplicationDbContext context, IExportService exportService)
        {
            _context = context;
            _exportService = exportService;
        }

        /// <summary>
        /// Get all employees
        /// </summary>
        /// <returns>List of employees</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();
        }

        /// <summary>
        /// Get employee by ID
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>Employee details</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        /// <summary>
        /// Export employees to Excel format
        /// </summary>
        /// <returns>Excel file download</returns>
        [HttpGet("export/excel")]
        public async Task<IActionResult> ExportToExcel()
        {
            var employees = await _context.Employees.ToListAsync();
            var result = await _exportService.ExportToExcelAsync(employees);
            
            return File(result.Data, result.ContentType, result.FileName);
        }

        /// <summary>
        /// Export employees to PDF format
        /// </summary>
        /// <returns>PDF file download</returns>
        [HttpGet("export/pdf")]
        public async Task<IActionResult> ExportToPdf()
        {
            var employees = await _context.Employees.ToListAsync();
            var result = await _exportService.ExportToPdfAsync(employees);
            
            return File(result.Data, result.ContentType, result.FileName);
        }

        /// <summary>
        /// Export employees to Word format
        /// </summary>
        /// <returns>Word document file download</returns>
        [HttpGet("export/word")]
        public async Task<IActionResult> ExportToWord()
        {
            var employees = await _context.Employees.ToListAsync();
            var result = await _exportService.ExportToWordAsync(employees);
            
            return File(result.Data, result.ContentType, result.FileName);
        }

        /// <summary>
        /// Create a new employee
        /// </summary>
        /// <param name="employee">Employee data</param>
        /// <returns>Created employee</returns>
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }

        /// <summary>
        /// Update an existing employee
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <param name="employee">Updated employee data</param>
        /// <returns>No content on success</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Delete an employee
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>No content on success</returns>
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

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}