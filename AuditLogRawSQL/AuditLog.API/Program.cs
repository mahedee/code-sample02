using AuditLog.API.AuditTrail.Implementations;
using AuditLog.API.AuditTrail.Interfaces;
using AuditLog.API.Persistence;
using AuditLog.API.Repositories.Implementations;
using AuditLog.API.Repositories.Interfaces;
using AuditLog.API.Services.Implementations;
using AuditLog.API.Services.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddTransient(_ => new DBConnector(connectionString));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IReportingRepository, ReportingRepository>();
builder.Services.AddScoped<IReportingService, ReportingService>();
builder.Services.AddScoped(typeof(IAuditTrail<>), typeof(AuditTrail<>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
