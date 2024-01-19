using ConcurrencyHandling.API.Data;
using ConcurrencyHandling.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConcurrencyHandling.API.Controllers
{

    // Concurrency check using hypermedia
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployee()
        {
            if (_context.Employee == null)
            {
                return NotFound();
            }
            return await _context.Employee.ToListAsync();
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            if (_context.Employee == null)
            {
                return NotFound();
            }
            var employee = await _context.Employee.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                id = employee.EmployeeID,
                name = employee.Name,
                salary = employee.Salary,
                rowVersion = employee.RowVersion,
                links = new[]
                {
                    new { rel = "edit", href = $"/employees/{id}/{employee.RowVersion}", method ="PUT" },
                    new { rel = "delete", href = $"/employees/{id}/{employee.RowVersion}", method ="DELETE" }
                }
            });

            //employee;
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, string version, Employee employee)
        {
            if (id != employee.EmployeeID)
            {
                return BadRequest();
            }

            var existingEmployee = await _context.Employee.FindAsync(id);
            if (existingEmployee == null)
            {
                return NotFound();
            }

            var currentRowVersion = existingEmployee.RowVersion.ToString();

            if (currentRowVersion != version)
            {
                return Conflict();
            }

            _context.Entry(existingEmployee).CurrentValues.SetValues(employee);
            existingEmployee.RowVersion = Guid.NewGuid();
            _context.Entry(existingEmployee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            if (_context.Employee == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Employee'  is null.");
            }
            employee.RowVersion = Guid.NewGuid();
            _context.Employee.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = employee.EmployeeID }, employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id, string version)
        {
            if (_context.Employee == null)
            {
                return NotFound();
            }
            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            if (employee.RowVersion.ToString() != version)
            {
                return Conflict();
            }

            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
