using System.Reflection;
using CleanArch.API.Filters;
using CleanArch.API.Services;
using CleanArch.Application;
using CleanArch.Application.Common.Interfaces;
using CleanArch.Core.Models;
using CleanArch.Infrastructure;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Sinks.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true)
    .AddEnvironmentVariables();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(builder.Configuration["ElasticConfiguration:Uri"]))
    {
        AutoRegisterTemplate = true,
        IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name?.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
    })
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();


builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();


builder.Services.AddControllers(options =>
        options.Filters.Add<ApiExceptionFilterAttribute>()
    )
    .ConfigureApiBehaviorOptions(opt =>
    {
        opt.SuppressModelStateInvalidFilter = true;
    })
    .AddJsonOptions(jsonOptions =>
    {
        jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
    })
    .AddNewtonsoftJson(opt =>
    {
        opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
        opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    })
    .AddFluentValidation(flv =>
    {
        flv.DisableDataAnnotationsValidation = true;
        flv.AutomaticValidationEnabled = false;
    });

var identityConfiguration =
    builder.Configuration.GetSection("IdentityServerConfiguration").Get<IdentityServerConfiguration>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddIdentityServerAuthentication(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.Authority = identityConfiguration.Authority;
        options.RequireHttpsMetadata = identityConfiguration.RequireHttpsMetaData;
        options.ApiName = identityConfiguration.ApiName;

        //TODO: This option must be removed from Production. This invoked to by pass SSL Certificate verification
        options.JwtBackChannelHandler = new HttpClientHandler()
        {
            ServerCertificateCustomValidationCallback = delegate { return true; }
        };
    });


//Enable CORS//Cross site resource sharing
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        b => b.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
    );
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = identityConfiguration.ApiDisplayName, Version = "v1" });
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri($"{identityConfiguration.Authority}connect/authorize"),
                TokenUrl = new Uri($"{identityConfiguration.Authority}connect/token"),
                Scopes = new Dictionary<string, string> {
                    {identityConfiguration.ApiName, identityConfiguration.ApiDisplayName},
                    {"openid"," Open Id" },
                    {"profile", "Profile"},
                    {"roles","Roles"}
                }
            }
        }
    });

    options.OperationFilter<AuthorizeCheckOperationFilter>();
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseRouting();
//Must be between app.UseRouting() and app.UseEndPoints()
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", identityConfiguration.ApiDisplayName);
    c.OAuthClientId(identityConfiguration.SwaggerUIClientId);
    c.OAuthAppName(identityConfiguration.ApiDisplayName);
    c.OAuthUsePkce();
});

app.MapControllers();

app.Run();
