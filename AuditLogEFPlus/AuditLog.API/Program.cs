using AuditLog.API.Models;
using AuditLog.API.Persistence;
using AuditLog.API.Repositories.Implementations;
using AuditLog.API.Repositories.Interfaces;
using AuditLog.API.Services.Implementations;
using AuditLog.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<AuditLogDBContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"))
);


// For Audit Log

AuditManager.DefaultConfiguration
.AuditEntryFactory = args =>
   new CustomAuditEntry() { AppplicationName = "AuditLogApp" };

AuditManager.DefaultConfiguration
            .AuditEntryPropertyFactory = args =>
                new CustomAuditEntryProperty() { AppplicationName = "AuditLogApp" };

AuditManager.DefaultConfiguration.AutoSavePreAction = (context, audit) => {
    ((AuditLogDBContext)context).AuditEntries.AddRange(audit.Entries.Cast<CustomAuditEntry>());
};


string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//builder.Services.AddTransient(_ => new DBConnector(connectionString));

builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();
builder.Services.AddTransient<ICustomerService, CustomerService>();
builder.Services.AddTransient<IReportingRepositry, ReportingRepository>();
builder.Services.AddTransient<IReportingService, ReportingService>();


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
