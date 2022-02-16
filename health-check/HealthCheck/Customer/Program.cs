using Customer.API.Db;
using Customer.API.Utility;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Sql server health check with name "customersql" with custom healtcheck
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("customersql");


// Create DbContext
builder.Services.AddDbContext<CustomerDbContext>(options =>
       options.UseSqlServer(
           builder.Configuration.GetConnectionString("CustomerDBConnection"))
    );

builder.Services.AddHealthChecks()
    .AddDbContextCheck<CustomerDbContext>("customerdbcontext");

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

var options = new HealthCheckOptions();
options.ResultStatusCodes[HealthStatus.Healthy] = StatusCodes.Status200OK;
options.ResultStatusCodes[HealthStatus.Degraded] = StatusCodes.Status200OK;
options.ResultStatusCodes[HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable;
options.ResponseWriter = HealthCheckResponse.CustomResponseWriter;
options.Predicate = healthcheck => healthcheck.Name == "customersql";

app.UseHealthChecks("/customersql", options);
//.RequireAuthorization();


app.UseHealthChecks("/customerdbcontext", new HealthCheckOptions()
{
    // Supress cache headers
    AllowCachingResponses = false,

    // Customize the HTTP Status code
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy]= StatusCodes.Status503ServiceUnavailable
    },

    // filters the health checks so that only those tagged with sql
    Predicate = healthCheck => healthCheck.Name == "customerdbcontext",

    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
});


// Cofigure for health check
app.MapHealthChecks("/hc", new HealthCheckOptions()
{
    //Predicate = _ => true,
    Predicate = r => r.Name.Contains("self"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
});

//a basic health probe configuration that reports the app's availability
//to process requests (liveness) is sufficient to discover the status of the app.
app.MapHealthChecks("/liveness", new HealthCheckOptions()
{
    Predicate = r => r.Name.Contains("self"),
});

app.Run();
