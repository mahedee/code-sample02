using Microsoft.EntityFrameworkCore;
using HRM.API.Data;
using HRM.API.DTOs;
using HRM.API.Models;

namespace HRM.API.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly HrmDbContext _context;

        public DepartmentService(HrmDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync()
        {
            var departments = await _context.Departments
                .Include(d => d.Employees)
                .ToListAsync();

            return departments.Select(d => new DepartmentDto
            {
                DepartmentId = d.DepartmentId,
                Name = d.Name,
                Description = d.Description,
                IsActive = d.IsActive,
                EmployeeCount = d.Employees.Count,
                CreatedDate = d.CreatedDate,
                UpdatedDate = d.UpdatedDate
            });
        }

        public async Task<DepartmentDto?> GetDepartmentByIdAsync(int id)
        {
            var department = await _context.Departments
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.DepartmentId == id);

            if (department == null) return null;

            return new DepartmentDto
            {
                DepartmentId = department.DepartmentId,
                Name = department.Name,
                Description = department.Description,
                IsActive = department.IsActive,
                EmployeeCount = department.Employees.Count,
                CreatedDate = department.CreatedDate,
                UpdatedDate = department.UpdatedDate
            };
        }

        public async Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto createDepartmentDto)
        {
            // Validate unique department name
            var existing = await _context.Departments.AnyAsync(d => d.Name == createDepartmentDto.Name);
            if (existing)
            {
                throw new InvalidOperationException("Department name must be unique.");
            }

            if (string.IsNullOrWhiteSpace(createDepartmentDto.Description))
            {
                throw new InvalidOperationException("Description is required.");
            }

            var department = new Department
            {
                Name = createDepartmentDto.Name,
                Description = createDepartmentDto.Description,
                IsActive = createDepartmentDto.IsActive,
                CreatedDate = DateTime.UtcNow
            };

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return new DepartmentDto
            {
                DepartmentId = department.DepartmentId,
                Name = department.Name,
                Description = department.Description,
                IsActive = department.IsActive,
                EmployeeCount = 0,
                CreatedDate = department.CreatedDate,
                UpdatedDate = department.UpdatedDate
            };
        }

        public async Task<DepartmentDto?> UpdateDepartmentAsync(int id, UpdateDepartmentDto updateDepartmentDto)
        {
            var department = await _context.Departments
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.DepartmentId == id);
            
            if (department == null) return null;

            // Check for duplicate name on different department
            var nameExists = await _context.Departments.AnyAsync(d => d.Name == updateDepartmentDto.Name && d.DepartmentId != id);
            if (nameExists)
            {
                throw new InvalidOperationException("Department name must be unique.");
            }

            if (string.IsNullOrWhiteSpace(updateDepartmentDto.Description))
            {
                throw new InvalidOperationException("Description is required.");
            }

            department.Name = updateDepartmentDto.Name;
            department.Description = updateDepartmentDto.Description;
            department.IsActive = updateDepartmentDto.IsActive;
            department.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new DepartmentDto
            {
                DepartmentId = department.DepartmentId,
                Name = department.Name,
                Description = department.Description,
                IsActive = department.IsActive,
                EmployeeCount = department.Employees.Count,
                CreatedDate = department.CreatedDate,
                UpdatedDate = department.UpdatedDate
            };
        }

        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            var department = await _context.Departments
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.DepartmentId == id);
            
            if (department == null) return false;

            // Check if department has employees
            if (department.Employees.Any())
            {
                throw new InvalidOperationException("Cannot delete department with existing employees.");
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<DepartmentDto>> GetActiveDepartmentsAsync()
        {
            var departments = await _context.Departments
                .Include(d => d.Employees)
                .Where(d => d.IsActive)
                .ToListAsync();

            return departments.Select(d => new DepartmentDto
            {
                DepartmentId = d.DepartmentId,
                Name = d.Name,
                Description = d.Description,
                IsActive = d.IsActive,
                EmployeeCount = d.Employees.Count,
                CreatedDate = d.CreatedDate,
                UpdatedDate = d.UpdatedDate
            });
        }
    }
}