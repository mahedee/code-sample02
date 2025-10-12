using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HRM.API.Migrations
{
    /// <inheritdoc />
    public partial class AddComprehensiveSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 1,
                column: "Description",
                value: "Manages employee relations, recruitment, and benefits");

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 2,
                column: "Description",
                value: "Manages IT infrastructure, software development, and technical support");

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 3,
                column: "Description",
                value: "Handles financial planning, accounting, and budget management");

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "DepartmentId", "CreatedDate", "Description", "IsActive", "Name", "UpdatedDate" },
                values: new object[,]
                {
                    { 4, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Manages marketing campaigns, brand management, and customer outreach", true, "Marketing", null },
                    { 5, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Handles customer acquisition, sales processes, and revenue generation", true, "Sales", null },
                    { 6, new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Focuses on innovation, product development, and research initiatives", true, "Research & Development", null },
                    { 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manages day-to-day operations, logistics, and process optimization", false, "Operations", null }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "CreatedDate", "DepartmentId", "Email", "FirstName", "HireDate", "LastName", "PhoneNumber", "Salary", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), 1, "sarah.johnson@company.com", "Sarah", new DateTime(2023, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Johnson", "+1-555-0101", 75000m, null },
                    { 2, new DateTime(2023, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "michael.chen@company.com", "Michael", new DateTime(2023, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Chen", "+1-555-0102", 68000m, null },
                    { 3, new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Utc), 2, "emma.wilson@company.com", "Emma", new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Wilson", "+1-555-0201", 95000m, null },
                    { 4, new DateTime(2023, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), 2, "david.rodriguez@company.com", "David", new DateTime(2023, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Rodriguez", "+1-555-0202", 88000m, null },
                    { 5, new DateTime(2023, 9, 5, 0, 0, 0, 0, DateTimeKind.Utc), 2, "jessica.taylor@company.com", "Jessica", new DateTime(2023, 9, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Taylor", "+1-555-0203", 72000m, null },
                    { 6, new DateTime(2022, 11, 15, 0, 0, 0, 0, DateTimeKind.Utc), 3, "robert.anderson@company.com", "Robert", new DateTime(2022, 11, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Anderson", "+1-555-0301", 82000m, null },
                    { 7, new DateTime(2023, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "lisa.martinez@company.com", "Lisa", new DateTime(2023, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Martinez", "+1-555-0302", 71000m, null },
                    { 8, new DateTime(2024, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), 4, "amanda.thompson@company.com", "Amanda", new DateTime(2024, 2, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Thompson", "+1-555-0401", 65000m, null },
                    { 9, new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, "james.brown@company.com", "James", new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Brown", "+1-555-0402", 58000m, null },
                    { 10, new DateTime(2024, 2, 20, 0, 0, 0, 0, DateTimeKind.Utc), 5, "kevin.davis@company.com", "Kevin", new DateTime(2024, 2, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Davis", "+1-555-0501", 70000m, null },
                    { 11, new DateTime(2024, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), 5, "rachel.miller@company.com", "Rachel", new DateTime(2024, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Miller", "+1-555-0502", 63000m, null },
                    { 12, new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), 6, "alan.kumar@company.com", "Dr. Alan", new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Kumar", "+1-555-0601", 105000m, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 1,
                column: "Description",
                value: "HR Department");

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 2,
                column: "Description",
                value: "IT Department");

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 3,
                column: "Description",
                value: "Finance Department");
        }
    }
}
