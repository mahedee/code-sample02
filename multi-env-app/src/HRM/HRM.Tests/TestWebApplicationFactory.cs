using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using HRM.API.Data;
using HRM.API.Models;
using System.Linq;

namespace HRM.Tests;

/// <summary>
/// Custom test web application factory for HRM API integration tests.
/// This class configures an in-memory database and seeds test data.
/// </summary>
public class TestWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove SQL Server database registration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<HrmDbContext>));
            
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Add in-memory database for testing
            var testDbName = $"TestDb_{Guid.NewGuid()}";
            services.AddDbContext<HrmDbContext>(options =>
            {
                options.UseInMemoryDatabase(testDbName);
                options.EnableSensitiveDataLogging();
            });
        });

        // Override environment to Testing to avoid conflicts
        builder.UseEnvironment("Testing");
    }

    /// <summary>
    /// Seeds the test database with sample data for testing purposes.
    /// Called from derived test classes after creating the HTTP client.
    /// </summary>
    /// <param name="context">The database context to seed</param>
    public void SeedTestData(HrmDbContext context)
    {
        // Reset data to ensure isolation between tests
        if (context.Departments.Any() || context.Employees.Any())
        {
            context.Employees.RemoveRange(context.Employees);
            context.Departments.RemoveRange(context.Departments);
            context.SaveChanges();
        }

        var departments = new[]
        {
            new Department 
            { 
                DepartmentId = 1, 
                Name = "IT", 
                Description = "Information Technology", 
                IsActive = true 
            },
            new Department 
            { 
                DepartmentId = 2, 
                Name = "HR", 
                Description = "Human Resources", 
                IsActive = true 
            },
            new Department 
            { 
                DepartmentId = 3, 
                Name = "Finance", 
                Description = "Finance Department", 
                IsActive = false 
            },
            new Department 
            { 
                DepartmentId = 4, 
                Name = "Marketing", 
                Description = "Marketing Department", 
                IsActive = true 
            }
        };

        context.Departments.AddRange(departments);

        var employees = new[]
        {
            new Employee 
            { 
                EmployeeId = 1, 
                FirstName = "John", 
                LastName = "Doe", 
                Email = "john.doe@test.com",
                PhoneNumber = "123-456-7890",
                HireDate = DateTime.Now.AddYears(-2),
                Salary = 75000,
                DepartmentId = 1
            },
            new Employee 
            { 
                EmployeeId = 2, 
                FirstName = "Jane", 
                LastName = "Smith", 
                Email = "jane.smith@test.com",
                PhoneNumber = "123-456-7891",
                HireDate = DateTime.Now.AddYears(-1),
                Salary = 80000,
                DepartmentId = 2
            },
            new Employee 
            { 
                EmployeeId = 3, 
                FirstName = "Bob", 
                LastName = "Johnson", 
                Email = "bob.johnson@test.com",
                PhoneNumber = "123-456-7892",
                HireDate = DateTime.Now.AddMonths(-6),
                Salary = 70000,
                DepartmentId = 1
            },
            new Employee 
            { 
                EmployeeId = 4, 
                FirstName = "Alice", 
                LastName = "Williams", 
                Email = "alice.williams@test.com",
                PhoneNumber = "123-456-7893",
                HireDate = DateTime.Now.AddMonths(-3),
                Salary = 65000,
                DepartmentId = 4
            }
        };

        context.Employees.AddRange(employees);
        context.SaveChanges();
    }
}