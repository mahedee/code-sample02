using Customer.API.Db;
using Customer.API.Utility;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

/*
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true)
    .AddEnvironmentVariables();
*/


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//string _connectionString = builder.Configuration.GetConnectionString("CustomerDBConnection");

// Register required services for health checks
//builder.Services.AddHealthChecks()
//    .AddCheck("sql", () => {

//        using (var connection = new SqlConnection(_connectionString))
//        {
//            try
//            {
//                connection.Open();
//            }catch (Exception ex)
//            {
//                return HealthCheckResult.Unhealthy();
//            }

//            return HealthCheckResult.Healthy();
//        }

//    });

// Sql server health check with name "customersql" with custom healtcheck
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("customersql");


// Sql server health check 
//builder.Services.AddHealthChecks()
//    .AddSqlServer(
//        builder.Configuration.GetConnectionString("CustomerDBConnection"), name: "directSqlTest");

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


//app.UseHealthChecks("/directsqltest", new HealthCheckOptions()
//{
//    // Supress cache headers
//    AllowCachingResponses = false,

//    // Customize the HTTP Status code
//    ResultStatusCodes =
//    {
//        [HealthStatus.Healthy] = StatusCodes.Status200OK,
//        [HealthStatus.Degraded] = StatusCodes.Status200OK,
//        [HealthStatus.Unhealthy]= StatusCodes.Status503ServiceUnavailable
//    },

//    // filters the health checks so that only those tagged with sql
//    Predicate = healthCheck => healthCheck.Name == "directsqltest",

//ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
//    //ResponseWriter = HealthCheckResponse.CustomResponseWriter
//});



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

//a basic health probe configuration that reports the app's availability to process requests (liveness) is sufficient to discover the status of the app.
app.MapHealthChecks("/liveness", new HealthCheckOptions()
{
    Predicate = r => r.Name.Contains("self"),
});



app.Run();
