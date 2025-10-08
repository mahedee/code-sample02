using ExportReportAPI.Data;
using ExportReportAPI.Generator.Interfaces;
using ExportReportAPI.Generator.Templates;
using ExportReportAPI.Models;
using ExportReportAPI.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("EmployeeDb"));

// Register template services
builder.Services.AddScoped<IExcelTemplate, ExcelTemplate>();
builder.Services.AddScoped<IPDFTemplate, PDFTemplate>();
builder.Services.AddScoped<IWordTemplate, WordTemplate>();

// Register main export service
builder.Services.AddScoped<IExportService, ExportService>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { 
        Title = "Export Report API", 
        Version = "v1",
        Description = "ASP.NET Core Web API for exporting reports in multiple formats (Excel, PDF, Word)"
    });
    
    // Include XML comments for better documentation
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Configure EPPlus license
OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

var app = builder.Build();

// Seed database with sample data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    DatabaseSeeder.SeedDatabase(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Export Report API V1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
