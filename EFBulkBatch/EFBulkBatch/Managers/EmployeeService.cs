using EFBulkBatch.Db;
using EFBulkBatch.Models;
using EFCore.BulkExtensions;

namespace EFBulkBatch.Managers
{
    public class EmployeeService
    {
        private readonly ApplicationDbContext _dbContext;
        private DateTime _startTime;
        private TimeSpan _elapsedTime;

        public EmployeeService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TimeSpan> AddBulkDataAsync()
        {
            // C# 9.0 feature: Target-typed new expressions
            List<Employee> employees = new();
            _startTime = DateTime.Now;

            for(int i = 0; i < 100000; i++)
            {
                employees.Add(new Employee
                {
                    Name = $"Employee {i}",
                    Designation = $"Designation {i}",
                    Department = $"Department {i}"
                });
            }

         
            // Use BulkInsertAsync to add multiple entities
            await _dbContext.BulkInsertAsync(employees);
            _elapsedTime = DateTime.Now - _startTime;
            return _elapsedTime;
        }


        public async Task<TimeSpan> UpdateBulkDataAsync()
        {
            // C# 9.0 feature: Target-typed new expressions
            List<Employee> employees = new();
            _startTime = DateTime.Now;
            for(int i = 0; i < 100000; i++)
            {
                employees.Add(new Employee
                {
                    Id = i + 1,
                    Name = $"Update Employee {i}",
                    Designation = $"Update Designation {i}",
                    Department = $"Update Department {i}"
                });
            }

            // Use BulkUpdateAsync to update multiple entities
            await _dbContext.BulkUpdateAsync(employees);
            _elapsedTime = DateTime.Now - _startTime;
            return _elapsedTime;
        }

        public async Task<TimeSpan> DeleteBulkDataAsync()
        {
            // C# 9.0 feature: Target-typed new expressions
            List<Employee> employees = _dbContext.Employees.ToList();
            _startTime = DateTime.Now;

            // Use BulkDeleteAsync to delete multiple entities
            await _dbContext.BulkDeleteAsync(employees);
            _elapsedTime = DateTime.Now - _startTime;
            return _elapsedTime;
        }


    }
}
