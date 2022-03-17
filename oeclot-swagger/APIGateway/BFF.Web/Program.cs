using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;
using MMLib.SwaggerForOcelot.DependencyInjection;
using BFF.Web.Config;
using Ocelot.Provider.Polly;

var builder = WebApplication.CreateBuilder(args);

var routes = "Routes";

builder.Configuration.AddOcelotWithSwaggerSupport(options => {
    options.Folder = routes;
});

builder.Services.AddOcelot(builder.Configuration).AddPolly();
builder.Services.AddSwaggerForOcelot(builder.Configuration);

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    //.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
    .AddOcelot(routes, builder.Environment)
    .AddEnvironmentVariables();


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Swagger for ocelot
//builder.Services.AddSwaggerForOcelot(builder.Configuration);
//builder.Services.AddSwaggerForOcelot();
builder.Services.AddSwaggerGen();

//For ocelot
//builder.Services.AddOcelot()
    
    // Added for caching
    //.AddCacheManager(x => {
    //    x.WithDictionaryHandle();
    //});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    // app.UseSwaggerUI();
    //app.UseSwaggerForOcelotUI();
}

//app.UseOcelot();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.UseSwaggerForOcelotUI(options =>
{
    //string swaggerJsonBasePath = string.IsNullOrWhiteSpace(options.RoutePrefix) ? "." : "..";
    //options.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "My API");
    //options.SwaggerEndpoint($"{swaggerJsonBasePath}/customer/swagger/v1/swagger.json", "OrderProcessing.Customer");
    //options.SwaggerEndpoint($"{swaggerJsonBasePath}/product/swagger/v1/swagger.json", "OrderProcessing.Product");
    //options.DownstreamSwaggerEndPointBasePath = "/swagger/docs";
    options.PathToSwaggerGenerator = "/swagger/docs";
    options.ReConfigureUpstreamSwaggerJson = AlterUpstream.AlterUpstreamSwaggerJson;

}).UseOcelot().Wait();

app.MapControllers();

app.Run();
