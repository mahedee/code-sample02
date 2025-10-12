using Microsoft.EntityFrameworkCore;
using HRM.API.Data;
using HRM.API.DTOs;
using HRM.API.Models;

namespace HRM.API.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly HrmDbContext _context;

        public EmployeeService(HrmDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
        {
            var employees = await _context.Employees
                .Include(e => e.Department)
                .ToListAsync();

            return employees.Select(e => new EmployeeDto
            {
                EmployeeId = e.EmployeeId,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
                HireDate = e.HireDate,
                Salary = e.Salary,
                DepartmentId = e.DepartmentId,
                DepartmentName = e.Department?.Name,
                CreatedDate = e.CreatedDate,
                UpdatedDate = e.UpdatedDate
            });
        }

        public async Task<EmployeeDto?> GetEmployeeByIdAsync(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null) return null;

            return new EmployeeDto
            {
                EmployeeId = employee.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                HireDate = employee.HireDate,
                Salary = employee.Salary,
                DepartmentId = employee.DepartmentId,
                DepartmentName = employee.Department?.Name,
                CreatedDate = employee.CreatedDate,
                UpdatedDate = employee.UpdatedDate
            };
        }

        public async Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto createEmployeeDto)
        {
            // Business validations
            if (createEmployeeDto.Salary <= 0)
            {
                throw new InvalidOperationException("Salary must be greater than 0.");
            }
            if (createEmployeeDto.HireDate > DateTime.Now)
            {
                throw new InvalidOperationException("Hire date cannot be in the future.");
            }
            var deptExists = await _context.Departments.AnyAsync(d => d.DepartmentId == createEmployeeDto.DepartmentId);
            if (!deptExists)
            {
                throw new InvalidOperationException("Invalid department.");
            }
            var emailExists = await _context.Employees.AnyAsync(e => e.Email == createEmployeeDto.Email);
            if (emailExists)
            {
                throw new InvalidOperationException("Email must be unique.");
            }

            var employee = new Employee
            {
                FirstName = createEmployeeDto.FirstName,
                LastName = createEmployeeDto.LastName,
                Email = createEmployeeDto.Email,
                PhoneNumber = createEmployeeDto.PhoneNumber,
                HireDate = createEmployeeDto.HireDate,
                Salary = createEmployeeDto.Salary,
                DepartmentId = createEmployeeDto.DepartmentId,
                CreatedDate = DateTime.UtcNow
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            // Fetch the created employee with department info
            var createdEmployee = await _context.Employees
                .Include(e => e.Department)
                .FirstAsync(e => e.EmployeeId == employee.EmployeeId);

            return new EmployeeDto
            {
                EmployeeId = createdEmployee.EmployeeId,
                FirstName = createdEmployee.FirstName,
                LastName = createdEmployee.LastName,
                Email = createdEmployee.Email,
                PhoneNumber = createdEmployee.PhoneNumber,
                HireDate = createdEmployee.HireDate,
                Salary = createdEmployee.Salary,
                DepartmentId = createdEmployee.DepartmentId,
                DepartmentName = createdEmployee.Department?.Name,
                CreatedDate = createdEmployee.CreatedDate,
                UpdatedDate = createdEmployee.UpdatedDate
            };
        }

        public async Task<EmployeeDto?> UpdateEmployeeAsync(int id, UpdateEmployeeDto updateEmployeeDto)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return null;

            // Business validations
            if (updateEmployeeDto.Salary <= 0)
            {
                throw new InvalidOperationException("Salary must be greater than 0.");
            }
            if (updateEmployeeDto.HireDate > DateTime.Now)
            {
                throw new InvalidOperationException("Hire date cannot be in the future.");
            }
            var deptExists = await _context.Departments.AnyAsync(d => d.DepartmentId == updateEmployeeDto.DepartmentId);
            if (!deptExists)
            {
                throw new InvalidOperationException("Invalid department.");
            }
            var emailExists = await _context.Employees.AnyAsync(e => e.Email == updateEmployeeDto.Email && e.EmployeeId != id);
            if (emailExists)
            {
                throw new InvalidOperationException("Email must be unique.");
            }

            employee.FirstName = updateEmployeeDto.FirstName;
            employee.LastName = updateEmployeeDto.LastName;
            employee.Email = updateEmployeeDto.Email;
            employee.PhoneNumber = updateEmployeeDto.PhoneNumber;
            employee.HireDate = updateEmployeeDto.HireDate;
            employee.Salary = updateEmployeeDto.Salary;
            employee.DepartmentId = updateEmployeeDto.DepartmentId;
            employee.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Fetch the updated employee with department info
            var updatedEmployee = await _context.Employees
                .Include(e => e.Department)
                .FirstAsync(e => e.EmployeeId == id);

            return new EmployeeDto
            {
                EmployeeId = updatedEmployee.EmployeeId,
                FirstName = updatedEmployee.FirstName,
                LastName = updatedEmployee.LastName,
                Email = updatedEmployee.Email,
                PhoneNumber = updatedEmployee.PhoneNumber,
                HireDate = updatedEmployee.HireDate,
                Salary = updatedEmployee.Salary,
                DepartmentId = updatedEmployee.DepartmentId,
                DepartmentName = updatedEmployee.Department?.Name,
                CreatedDate = updatedEmployee.CreatedDate,
                UpdatedDate = updatedEmployee.UpdatedDate
            };
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return false;

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployeesByDepartmentAsync(int departmentId)
        {
            var employees = await _context.Employees
                .Include(e => e.Department)
                .Where(e => e.DepartmentId == departmentId)
                .ToListAsync();

            return employees.Select(e => new EmployeeDto
            {
                EmployeeId = e.EmployeeId,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
                HireDate = e.HireDate,
                Salary = e.Salary,
                DepartmentId = e.DepartmentId,
                DepartmentName = e.Department?.Name,
                CreatedDate = e.CreatedDate,
                UpdatedDate = e.UpdatedDate
            });
        }
    }
}