using HealthChecks.UI.Client;
using Location.API.Persistence;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Sql server health check 
builder.Services.AddHealthChecks()
    .AddSqlServer(
        builder.Configuration.GetConnectionString("LocationDBConnection"));

// Create DbContext
builder.Services.AddDbContext<LocationDbContext>(options =>
       options.UseSqlServer(
           builder.Configuration.GetConnectionString("LocationDBConnection"))
    );

// DbContext health check for EF core
builder.Services.AddHealthChecks()
    .AddDbContextCheck<LocationDbContext>("locationdbcontext");

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

// Cofigure for health check
app.MapHealthChecks("/hc", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
});

//a basic health probe configuration that reports the app's availability to process requests (liveness) is sufficient to discover the status of the app.
app.MapHealthChecks("/liveness", new HealthCheckOptions()
{
    Predicate = r => r.Name.Contains("self"),
});

app.Run();
