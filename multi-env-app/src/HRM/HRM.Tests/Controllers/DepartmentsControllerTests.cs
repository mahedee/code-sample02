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
/// Integration tests for the Departments API controller.
/// Tests all CRUD operations and error scenarios.
/// </summary>
public class DepartmentsControllerTests : IClassFixture<TestWebApplicationFactory<HRM.API.Program>>
{
    private readonly HttpClient _client;
    private readonly TestWebApplicationFactory<HRM.API.Program> _factory;
    private readonly JsonSerializerOptions _jsonOptions;

    public DepartmentsControllerTests(TestWebApplicationFactory<HRM.API.Program> factory)
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
    public async Task GetDepartments_ReturnsAllDepartments_WithCorrectCount()
    {
        // Act
        var response = await _client.GetAsync("/api/departments");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        var departments = JsonSerializer.Deserialize<DepartmentDto[]>(content, _jsonOptions);

        departments.Should().NotBeNull();
        departments.Should().HaveCount(4);
        departments.Should().Contain(d => d.Name == "IT");
        departments.Should().Contain(d => d.Name == "HR");
        departments.Should().Contain(d => d.Name == "Finance");
        departments.Should().Contain(d => d.Name == "Marketing");
    }

    [Fact]
    public async Task GetDepartments_ReturnsCorrectContentType()
    {
        // Act
        var response = await _client.GetAsync("/api/departments");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/json");
    }

    [Theory]
    [InlineData(1, "IT", "Information Technology", true)]
    [InlineData(2, "HR", "Human Resources", true)]
    [InlineData(3, "Finance", "Finance Department", false)]
    [InlineData(4, "Marketing", "Marketing Department", true)]
    public async Task GetDepartment_WithValidId_ReturnsCorrectDepartment(int id, string expectedName, string expectedDescription, bool expectedIsActive)
    {
        // Act
        var response = await _client.GetAsync($"/api/departments/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        var department = JsonSerializer.Deserialize<DepartmentDto>(content, _jsonOptions);

        department.Should().NotBeNull();
        department!.DepartmentId.Should().Be(id);
        department.Name.Should().Be(expectedName);
        department.Description.Should().Be(expectedDescription);
        department.IsActive.Should().Be(expectedIsActive);
    }

    [Theory]
    [InlineData(999)]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetDepartment_WithInvalidId_ReturnsNotFound(int invalidId)
    {
        // Act
        var response = await _client.GetAsync($"/api/departments/{invalidId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PostDepartment_WithValidData_CreatesDepartmentAndReturnsCreated()
    {
        // Arrange
        var newDepartment = new CreateDepartmentDto
        {
            Name = "Research & Development",
            Description = "Research and Development Department",
            IsActive = true
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/departments", newDepartment);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var content = await response.Content.ReadAsStringAsync();
        var createdDepartment = JsonSerializer.Deserialize<DepartmentDto>(content, _jsonOptions);

        createdDepartment.Should().NotBeNull();
        createdDepartment!.Name.Should().Be("Research & Development");
        createdDepartment.Description.Should().Be("Research and Development Department");
        createdDepartment.IsActive.Should().BeTrue();
        createdDepartment.DepartmentId.Should().BeGreaterThan(0);

        // Verify Location header
        response.Headers.Location.Should().NotBeNull();
        response.Headers.Location!.ToString().Should().Contain($"/api/departments/{createdDepartment.DepartmentId}");
    }

    [Theory]
    [InlineData("", "Valid Description", true)] // Empty name
    [InlineData("Valid Name", "", false)] // Empty description
    public async Task PostDepartment_WithInvalidData_ReturnsBadRequest(string name, string description, bool isActive)
    {
        // Arrange
        var invalidDepartment = new CreateDepartmentDto
        {
            Name = name,
            Description = description,
            IsActive = isActive
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/departments", invalidDepartment);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PostDepartment_WithDuplicateName_ReturnsBadRequest()
    {
        // Arrange - Try to create department with existing name
        var duplicateDepartment = new CreateDepartmentDto
        {
            Name = "IT", // This already exists in test data
            Description = "Duplicate IT Department",
            IsActive = true
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/departments", duplicateDepartment);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PutDepartment_WithValidData_UpdatesDepartmentAndReturnsNoContent()
    {
        // Arrange
        var updatedDepartment = new UpdateDepartmentDto
        {
            Name = "Information Technology",
            Description = "Updated IT Department with modern technologies",
            IsActive = true
        };

        // Act
        var response = await _client.PutAsJsonAsync("/api/departments/1", updatedDepartment);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify the update by getting the department
        var getResponse = await _client.GetAsync("/api/departments/1");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await getResponse.Content.ReadAsStringAsync();
        var department = JsonSerializer.Deserialize<DepartmentDto>(content, _jsonOptions);

        department!.Name.Should().Be("Information Technology");
        department.Description.Should().Be("Updated IT Department with modern technologies");
        department.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task PutDepartment_WithMismatchedId_ReturnsBadRequest()
    {
        // Arrange
        var department = new UpdateDepartmentDto
        {
            Name = "Test Department",
            Description = "Test Description",
            IsActive = true
        };

        // Act
        var response = await _client.PutAsJsonAsync("/api/departments/1", department);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PutDepartment_WithNonExistentId_ReturnsNotFound()
    {
        // Arrange
        var department = new UpdateDepartmentDto
        {
            Name = "Non-existent Department",
            Description = "This department does not exist",
            IsActive = true
        };

        // Act
        var response = await _client.PutAsJsonAsync("/api/departments/999", department);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteDepartment_WithValidId_DeletesDepartmentAndReturnsNoContent()
    {
        // First, create a department to delete
        var newDepartment = new CreateDepartmentDto
        {
            Name = "Temporary Department",
            Description = "Department to be deleted",
            IsActive = true
        };

        var createResponse = await _client.PostAsJsonAsync("/api/departments", newDepartment);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var content = await createResponse.Content.ReadAsStringAsync();
        var createdDepartment = JsonSerializer.Deserialize<DepartmentDto>(content, _jsonOptions);

        // Act - Delete the department
        var deleteResponse = await _client.DeleteAsync($"/api/departments/{createdDepartment!.DepartmentId}");

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify deletion by trying to get the department
        var getResponse = await _client.GetAsync($"/api/departments/{createdDepartment.DepartmentId}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteDepartment_WithNonExistentId_ReturnsNotFound()
    {
        // Act
        var response = await _client.DeleteAsync("/api/departments/999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteDepartment_WithEmployees_ReturnsBadRequest()
    {
        // Act - Try to delete department with employees (ID 1 has employees in test data)
        var response = await _client.DeleteAsync("/api/departments/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetDepartments_ResponseTime_ShouldBeFast()
    {
        // Arrange
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        // Act
        var response = await _client.GetAsync("/api/departments");

        // Assert
        stopwatch.Stop();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(1000); // Should respond within 1 second
    }

    [Fact]
    public async Task API_ShouldHandleConcurrentRequests()
    {
        // Arrange
        var tasks = new List<Task<HttpResponseMessage>>();

        // Act - Make multiple concurrent requests
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(_client.GetAsync("/api/departments"));
        }

        var responses = await Task.WhenAll(tasks);

        // Assert
        responses.Should().HaveCount(10);
        responses.Should().OnlyContain(r => r.StatusCode == HttpStatusCode.OK);
    }
}