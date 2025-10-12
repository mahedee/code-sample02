using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using HRM.API.Models;
using HRM.API.DTOs;
using HRM.API;
using HRM.API.Data;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HRM.Tests.Controllers;

/// <summary>
/// Integration tests for the Employees API controller.
/// Tests all CRUD operations and error scenarios.
/// </summary>
public class EmployeesControllerTests : IClassFixture<TestWebApplicationFactory<HRM.API.Program>>
{
    private readonly HttpClient _client;
    private readonly TestWebApplicationFactory<HRM.API.Program> _factory;
    private readonly JsonSerializerOptions _jsonOptions;

    public EmployeesControllerTests(TestWebApplicationFactory<HRM.API.Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _jsonOptions = new JsonSerializerOptions 
        { 
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
        };

        // Seed test data
        SeedDatabase();
    }

    private void SeedDatabase()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HrmDbContext>();
        _factory.SeedTestData(context);
    }

    [Fact]
    public async Task GetEmployees_ReturnsAllEmployees_WithCorrectCount()
    {
        // Act
        var response = await _client.GetAsync("/api/employees");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        var employees = JsonSerializer.Deserialize<EmployeeDto[]>(content, _jsonOptions);

        employees.Should().NotBeNull();
        employees.Should().HaveCount(4);
        employees.Should().Contain(e => e.FirstName == "John" && e.LastName == "Doe");
        employees.Should().Contain(e => e.FirstName == "Jane" && e.LastName == "Smith");
        employees.Should().Contain(e => e.FirstName == "Bob" && e.LastName == "Johnson");
        employees.Should().Contain(e => e.FirstName == "Alice" && e.LastName == "Williams");
    }

    [Fact]
    public async Task GetEmployees_ReturnsCorrectContentType()
    {
        // Act
        var response = await _client.GetAsync("/api/employees");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/json");
    }

    [Theory]
    [InlineData(1, "John", "Doe", "john.doe@test.com", 75000)]
    [InlineData(2, "Jane", "Smith", "jane.smith@test.com", 80000)]
    [InlineData(3, "Bob", "Johnson", "bob.johnson@test.com", 70000)]
    [InlineData(4, "Alice", "Williams", "alice.williams@test.com", 65000)]
    public async Task GetEmployee_WithValidId_ReturnsCorrectEmployee(int id, string expectedFirstName, string expectedLastName, string expectedEmail, decimal expectedSalary)
    {
        // Act
        var response = await _client.GetAsync($"/api/employees/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        var employee = JsonSerializer.Deserialize<EmployeeDto>(content, _jsonOptions);

        employee.Should().NotBeNull();
        employee!.EmployeeId.Should().Be(id);
        employee.FirstName.Should().Be(expectedFirstName);
        employee.LastName.Should().Be(expectedLastName);
        employee.Email.Should().Be(expectedEmail);
        employee.Salary.Should().Be(expectedSalary);
    }

    [Theory]
    [InlineData(999)]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetEmployee_WithInvalidId_ReturnsNotFound(int invalidId)
    {
        // Act
        var response = await _client.GetAsync($"/api/employees/{invalidId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PostEmployee_WithValidData_CreatesEmployeeAndReturnsCreated()
    {
        // Arrange
        var newEmployee = new CreateEmployeeDto
        {
            FirstName = "Michael",
            LastName = "Brown",
            Email = "michael.brown@test.com",
            PhoneNumber = "555-123-4567",
            HireDate = DateTime.Now,
            Salary = 72000,
            DepartmentId = 1
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/employees", newEmployee);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var content = await response.Content.ReadAsStringAsync();
        var createdEmployee = JsonSerializer.Deserialize<EmployeeDto>(content, _jsonOptions);

        createdEmployee.Should().NotBeNull();
        createdEmployee!.FirstName.Should().Be("Michael");
        createdEmployee.LastName.Should().Be("Brown");
        createdEmployee.Email.Should().Be("michael.brown@test.com");
        createdEmployee.Salary.Should().Be(72000);
        createdEmployee.EmployeeId.Should().BeGreaterThan(0);

        // Verify Location header
        response.Headers.Location.Should().NotBeNull();
        response.Headers.Location!.ToString().Should().Contain($"/api/employees/{createdEmployee.EmployeeId}");
    }

    [Theory]
    [InlineData("", "Brown", "valid@test.com", "555-1234", 70000, 1)] // Empty first name
    [InlineData("Michael", "", "valid@test.com", "555-1234", 70000, 1)] // Empty last name
    [InlineData("Michael", "Brown", "", "555-1234", 70000, 1)] // Empty email
    [InlineData("Michael", "Brown", "invalid-email", "555-1234", 70000, 1)] // Invalid email format
    [InlineData("Michael", "Brown", "valid@test.com", "555-1234", 0, 1)] // Zero salary
    [InlineData("Michael", "Brown", "valid@test.com", "555-1234", -1000, 1)] // Negative salary
    [InlineData("Michael", "Brown", "valid@test.com", "555-1234", 70000, 999)] // Invalid department ID
    public async Task PostEmployee_WithInvalidData_ReturnsBadRequest(string firstName, string lastName, string email, string phoneNumber, decimal salary, int departmentId)
    {
        // Arrange
        var invalidEmployee = new CreateEmployeeDto
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            PhoneNumber = phoneNumber,
            HireDate = DateTime.Now,
            Salary = salary,
            DepartmentId = departmentId
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/employees", invalidEmployee);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PostEmployee_WithDuplicateEmail_ReturnsBadRequest()
    {
        // Arrange - Try to create employee with existing email
        var duplicateEmployee = new CreateEmployeeDto
        {
            FirstName = "Duplicate",
            LastName = "Employee",
            Email = "john.doe@test.com", // This already exists in test data
            PhoneNumber = "555-9999",
            HireDate = DateTime.Now,
            Salary = 75000,
            DepartmentId = 1
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/employees", duplicateEmployee);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PutEmployee_WithValidData_UpdatesEmployeeAndReturnsNoContent()
    {
        // Arrange
        var updatedEmployee = new UpdateEmployeeDto
        {
            FirstName = "Johnathan",
            LastName = "Doe",
            Email = "johnathan.doe@test.com",
            PhoneNumber = "555-111-2222",
            HireDate = DateTime.Now.AddYears(-2),
            Salary = 82000,
            DepartmentId = 2
        };

        // Act
        var response = await _client.PutAsJsonAsync("/api/employees/1", updatedEmployee);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify the update by getting the employee
        var getResponse = await _client.GetAsync("/api/employees/1");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await getResponse.Content.ReadAsStringAsync();
        var employee = JsonSerializer.Deserialize<EmployeeDto>(content, _jsonOptions);

        employee!.FirstName.Should().Be("Johnathan");
        employee.Email.Should().Be("johnathan.doe@test.com");
        employee.Salary.Should().Be(82000);
        employee.DepartmentId.Should().Be(2);
    }

    [Fact]
    public async Task PutEmployee_WithNonExistentId_ReturnsNotFound()
    {
        // Arrange
        var employee = new UpdateEmployeeDto
        {
            FirstName = "Non-existent",
            LastName = "Employee",
            Email = "nonexistent@test.com",
            PhoneNumber = "555-0000",
            HireDate = DateTime.Now,
            Salary = 70000,
            DepartmentId = 1
        };

        // Act
        var response = await _client.PutAsJsonAsync("/api/employees/999", employee);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteEmployee_WithValidId_DeletesEmployeeAndReturnsNoContent()
    {
        // First, create an employee to delete
        var newEmployee = new CreateEmployeeDto
        {
            FirstName = "Temporary",
            LastName = "Employee",
            Email = "temp.employee@test.com",
            PhoneNumber = "555-0000",
            HireDate = DateTime.Now,
            Salary = 65000,
            DepartmentId = 1
        };

        var createResponse = await _client.PostAsJsonAsync("/api/employees", newEmployee);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var content = await createResponse.Content.ReadAsStringAsync();
        var createdEmployee = JsonSerializer.Deserialize<EmployeeDto>(content, _jsonOptions);

        // Act - Delete the employee
        var deleteResponse = await _client.DeleteAsync($"/api/employees/{createdEmployee!.EmployeeId}");

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify deletion by trying to get the employee
        var getResponse = await _client.GetAsync($"/api/employees/{createdEmployee.EmployeeId}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteEmployee_WithNonExistentId_ReturnsNotFound()
    {
        // Act
        var response = await _client.DeleteAsync("/api/employees/999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetEmployees_ResponseTime_ShouldBeFast()
    {
        // Arrange
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        // Act
        var response = await _client.GetAsync("/api/employees");

        // Assert
        stopwatch.Stop();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(1000); // Should respond within 1 second
    }

    [Fact]
    public async Task GetEmployeesByDepartment_FiltersCorrectly()
    {
        // Act - Get employees from IT department (ID: 1)
        var response = await _client.GetAsync("/api/employees?departmentId=1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        var employees = JsonSerializer.Deserialize<EmployeeDto[]>(content, _jsonOptions);

        employees.Should().NotBeNull();
        employees.Should().OnlyContain(e => e.DepartmentId == 1);
        employees.Should().HaveCountGreaterThan(0);
    }

    [Fact]
    public async Task API_ShouldHandleConcurrentEmployeeRequests()
    {
        // Arrange
        var tasks = new List<Task<HttpResponseMessage>>();

        // Act - Make multiple concurrent requests
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(_client.GetAsync("/api/employees"));
        }

        var responses = await Task.WhenAll(tasks);

        // Assert
        responses.Should().HaveCount(10);
        responses.Should().OnlyContain(r => r.StatusCode == HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetEmployees_WithPagination_ReturnsCorrectData()
    {
        // Act - Request first 2 employees
        var response = await _client.GetAsync("/api/employees?pageNumber=1&pageSize=2");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        var employees = JsonSerializer.Deserialize<EmployeeDto[]>(content, _jsonOptions);

        employees.Should().NotBeNull();
        employees!.Should().HaveCountLessThanOrEqualTo(2);
    }

    [Fact]
    public async Task PostEmployee_WithFutureHireDate_ReturnsBadRequest()
    {
        // Arrange
        var futureEmployee = new CreateEmployeeDto
        {
            FirstName = "Future",
            LastName = "Employee",
            Email = "future@test.com",
            PhoneNumber = "555-FUTURE",
            HireDate = DateTime.Now.AddYears(1), // Future date
            Salary = 70000,
            DepartmentId = 1
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/employees", futureEmployee);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}