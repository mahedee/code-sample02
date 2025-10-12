using Microsoft.EntityFrameworkCore;
using HRM.API.Models;

namespace HRM.API.Data
{
    public class HrmDbContext : DbContext
    {
        public HrmDbContext(DbContextOptions<HrmDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Employee entity
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmployeeId);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.Salary).HasColumnType("decimal(18,2)");
                
                // Configure relationship
                entity.HasOne(e => e.Department)
                      .WithMany(d => d.Employees)
                      .HasForeignKey(e => e.DepartmentId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Department entity
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(d => d.DepartmentId);
                entity.Property(d => d.Name).IsRequired().HasMaxLength(100);
                entity.Property(d => d.Description).HasMaxLength(500);
            });

            // Seed data
            modelBuilder.Entity<Department>().HasData(
                new Department 
                { 
                    DepartmentId = 1, 
                    Name = "Human Resources", 
                    Description = "Manages employee relations, recruitment, and benefits", 
                    IsActive = true,
                    CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Department 
                { 
                    DepartmentId = 2, 
                    Name = "Information Technology", 
                    Description = "Manages IT infrastructure, software development, and technical support", 
                    IsActive = true,
                    CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Department 
                { 
                    DepartmentId = 3, 
                    Name = "Finance", 
                    Description = "Handles financial planning, accounting, and budget management", 
                    IsActive = true,
                    CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Department 
                { 
                    DepartmentId = 4, 
                    Name = "Marketing", 
                    Description = "Manages marketing campaigns, brand management, and customer outreach", 
                    IsActive = true,
                    CreatedDate = new DateTime(2024, 1, 15, 0, 0, 0, DateTimeKind.Utc)
                },
                new Department 
                { 
                    DepartmentId = 5, 
                    Name = "Sales", 
                    Description = "Handles customer acquisition, sales processes, and revenue generation", 
                    IsActive = true,
                    CreatedDate = new DateTime(2024, 2, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Department 
                { 
                    DepartmentId = 6, 
                    Name = "Research & Development", 
                    Description = "Focuses on innovation, product development, and research initiatives", 
                    IsActive = true,
                    CreatedDate = new DateTime(2024, 3, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Department 
                { 
                    DepartmentId = 7, 
                    Name = "Operations", 
                    Description = "Manages day-to-day operations, logistics, and process optimization", 
                    IsActive = false,
                    CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );

            // Seed employees
            modelBuilder.Entity<Employee>().HasData(
                // HR Department
                new Employee
                {
                    EmployeeId = 1,
                    FirstName = "Sarah",
                    LastName = "Johnson",
                    Email = "sarah.johnson@company.com",
                    PhoneNumber = "+1-555-0101",
                    HireDate = new DateTime(2023, 3, 15, 0, 0, 0, DateTimeKind.Utc),
                    Salary = 75000m,
                    DepartmentId = 1,
                    CreatedDate = new DateTime(2023, 3, 15, 0, 0, 0, DateTimeKind.Utc)
                },
                new Employee
                {
                    EmployeeId = 2,
                    FirstName = "Michael",
                    LastName = "Chen",
                    Email = "michael.chen@company.com",
                    PhoneNumber = "+1-555-0102",
                    HireDate = new DateTime(2023, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                    Salary = 68000m,
                    DepartmentId = 1,
                    CreatedDate = new DateTime(2023, 6, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                // IT Department
                new Employee
                {
                    EmployeeId = 3,
                    FirstName = "Emma",
                    LastName = "Wilson",
                    Email = "emma.wilson@company.com",
                    PhoneNumber = "+1-555-0201",
                    HireDate = new DateTime(2022, 8, 10, 0, 0, 0, DateTimeKind.Utc),
                    Salary = 95000m,
                    DepartmentId = 2,
                    CreatedDate = new DateTime(2022, 8, 10, 0, 0, 0, DateTimeKind.Utc)
                },
                new Employee
                {
                    EmployeeId = 4,
                    FirstName = "David",
                    LastName = "Rodriguez",
                    Email = "david.rodriguez@company.com",
                    PhoneNumber = "+1-555-0202",
                    HireDate = new DateTime(2023, 1, 20, 0, 0, 0, DateTimeKind.Utc),
                    Salary = 88000m,
                    DepartmentId = 2,
                    CreatedDate = new DateTime(2023, 1, 20, 0, 0, 0, DateTimeKind.Utc)
                },
                new Employee
                {
                    EmployeeId = 5,
                    FirstName = "Jessica",
                    LastName = "Taylor",
                    Email = "jessica.taylor@company.com",
                    PhoneNumber = "+1-555-0203",
                    HireDate = new DateTime(2023, 9, 5, 0, 0, 0, DateTimeKind.Utc),
                    Salary = 72000m,
                    DepartmentId = 2,
                    CreatedDate = new DateTime(2023, 9, 5, 0, 0, 0, DateTimeKind.Utc)
                },
                // Finance Department
                new Employee
                {
                    EmployeeId = 6,
                    FirstName = "Robert",
                    LastName = "Anderson",
                    Email = "robert.anderson@company.com",
                    PhoneNumber = "+1-555-0301",
                    HireDate = new DateTime(2022, 11, 15, 0, 0, 0, DateTimeKind.Utc),
                    Salary = 82000m,
                    DepartmentId = 3,
                    CreatedDate = new DateTime(2022, 11, 15, 0, 0, 0, DateTimeKind.Utc)
                },
                new Employee
                {
                    EmployeeId = 7,
                    FirstName = "Lisa",
                    LastName = "Martinez",
                    Email = "lisa.martinez@company.com",
                    PhoneNumber = "+1-555-0302",
                    HireDate = new DateTime(2023, 4, 1, 0, 0, 0, DateTimeKind.Utc),
                    Salary = 71000m,
                    DepartmentId = 3,
                    CreatedDate = new DateTime(2023, 4, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                // Marketing Department
                new Employee
                {
                    EmployeeId = 8,
                    FirstName = "Amanda",
                    LastName = "Thompson",
                    Email = "amanda.thompson@company.com",
                    PhoneNumber = "+1-555-0401",
                    HireDate = new DateTime(2024, 2, 15, 0, 0, 0, DateTimeKind.Utc),
                    Salary = 65000m,
                    DepartmentId = 4,
                    CreatedDate = new DateTime(2024, 2, 15, 0, 0, 0, DateTimeKind.Utc)
                },
                new Employee
                {
                    EmployeeId = 9,
                    FirstName = "James",
                    LastName = "Brown",
                    Email = "james.brown@company.com",
                    PhoneNumber = "+1-555-0402",
                    HireDate = new DateTime(2024, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                    Salary = 58000m,
                    DepartmentId = 4,
                    CreatedDate = new DateTime(2024, 3, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                // Sales Department
                new Employee
                {
                    EmployeeId = 10,
                    FirstName = "Kevin",
                    LastName = "Davis",
                    Email = "kevin.davis@company.com",
                    PhoneNumber = "+1-555-0501",
                    HireDate = new DateTime(2024, 2, 20, 0, 0, 0, DateTimeKind.Utc),
                    Salary = 70000m,
                    DepartmentId = 5,
                    CreatedDate = new DateTime(2024, 2, 20, 0, 0, 0, DateTimeKind.Utc)
                },
                new Employee
                {
                    EmployeeId = 11,
                    FirstName = "Rachel",
                    LastName = "Miller",
                    Email = "rachel.miller@company.com",
                    PhoneNumber = "+1-555-0502",
                    HireDate = new DateTime(2024, 4, 10, 0, 0, 0, DateTimeKind.Utc),
                    Salary = 63000m,
                    DepartmentId = 5,
                    CreatedDate = new DateTime(2024, 4, 10, 0, 0, 0, DateTimeKind.Utc)
                },
                // R&D Department
                new Employee
                {
                    EmployeeId = 12,
                    FirstName = "Dr. Alan",
                    LastName = "Kumar",
                    Email = "alan.kumar@company.com",
                    PhoneNumber = "+1-555-0601",
                    HireDate = new DateTime(2024, 3, 15, 0, 0, 0, DateTimeKind.Utc),
                    Salary = 105000m,
                    DepartmentId = 6,
                    CreatedDate = new DateTime(2024, 3, 15, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}