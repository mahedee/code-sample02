using ExportReportAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ExportReportAPI.Data
{
    /// <summary>
    /// Handles database seeding with initial data for development and testing
    /// </summary>
    public static class DatabaseSeeder
    {
        /// <summary>
        /// Seeds the database with sample employee data
        /// </summary>
        /// <param name="context">The database context to seed</param>
        public static void SeedDatabase(ApplicationDbContext context)
        {
            // Check if database has already been seeded
            if (context.Employees.Any())
                return;

            var employees = GetSampleEmployees();
            
            context.Employees.AddRange(employees);
            context.SaveChanges();
        }

        /// <summary>
        /// Seeds the database asynchronously with sample employee data
        /// </summary>
        /// <param name="context">The database context to seed</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public static async Task SeedDatabaseAsync(ApplicationDbContext context)
        {
            // Check if database has already been seeded
            if (await context.Employees.AnyAsync())
                return;

            var employees = GetSampleEmployees();
            
            await context.Employees.AddRangeAsync(employees);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Gets a collection of sample employee data for seeding
        /// </summary>
        /// <returns>Array of sample employees</returns>
        private static Employee[] GetSampleEmployees()
        {
            return new[]
            {
                new Employee
                {
                    FullName = "John Smith",
                    Designation = "Software Engineer",
                    Department = "IT",
                    Email = "john.smith@company.com",
                    Phone = "+1-555-0101",
                    Salary = 75000,
                    JoinDate = new DateTime(2020, 1, 15)
                },
                new Employee
                {
                    FullName = "Sarah Johnson",
                    Designation = "Senior Developer",
                    Department = "IT",
                    Email = "sarah.johnson@company.com",
                    Phone = "+1-555-0102",
                    Salary = 85000,
                    JoinDate = new DateTime(2019, 3, 20)
                },
                new Employee
                {
                    FullName = "Michael Brown",
                    Designation = "Project Manager",
                    Department = "PMO",
                    Email = "michael.brown@company.com",
                    Phone = "+1-555-0103",
                    Salary = 95000,
                    JoinDate = new DateTime(2018, 7, 10)
                },
                new Employee
                {
                    FullName = "Emily Davis",
                    Designation = "UI/UX Designer",
                    Department = "Design",
                    Email = "emily.davis@company.com",
                    Phone = "+1-555-0104",
                    Salary = 70000,
                    JoinDate = new DateTime(2021, 2, 5)
                },
                new Employee
                {
                    FullName = "David Wilson",
                    Designation = "DevOps Engineer",
                    Department = "IT",
                    Email = "david.wilson@company.com",
                    Phone = "+1-555-0105",
                    Salary = 80000,
                    JoinDate = new DateTime(2020, 8, 25)
                },
                new Employee
                {
                    FullName = "Lisa Anderson",
                    Designation = "Business Analyst",
                    Department = "Business",
                    Email = "lisa.anderson@company.com",
                    Phone = "+1-555-0106",
                    Salary = 72000,
                    JoinDate = new DateTime(2019, 11, 12)
                },
                new Employee
                {
                    FullName = "James Taylor",
                    Designation = "Team Lead",
                    Department = "IT",
                    Email = "james.taylor@company.com",
                    Phone = "+1-555-0107",
                    Salary = 90000,
                    JoinDate = new DateTime(2017, 5, 30)
                },
                new Employee
                {
                    FullName = "Jennifer Martinez",
                    Designation = "QA Engineer",
                    Department = "Quality",
                    Email = "jennifer.martinez@company.com",
                    Phone = "+1-555-0108",
                    Salary = 65000,
                    JoinDate = new DateTime(2020, 12, 1)
                }
            };
        }
    }
}