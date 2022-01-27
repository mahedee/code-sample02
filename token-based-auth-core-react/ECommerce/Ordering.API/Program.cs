using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Ordering.Application.Commands.Customers;
using Ordering.Application.Common.Interfaces;
using Ordering.Application.Common.Security;
using Ordering.Application.Handlers.CommandHandler;
using Ordering.Application.Handlers.CommandHandler.User.Create;
using Ordering.Core.Repositories.Command;
using Ordering.Core.Repositories.Command.Base;
using Ordering.Core.Repositories.Query;
using Ordering.Core.Repositories.Query.Base;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Identity;
using Ordering.Infrastructure.Repository.Command;
using Ordering.Infrastructure.Repository.Command.Base;
using Ordering.Infrastructure.Repository.Query;
using Ordering.Infrastructure.Repository.Query.Base;
using Ordering.Infrastructure.Services;
using System.Reflection;
using System.Text;
//using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// For authentication
var _key = builder.Configuration["Jwt:Key"];
var _issuer = builder.Configuration["Jwt:Issuer"];
var _audience = builder.Configuration["Jwt:Audience"];
var _expirtyMinutes = builder.Configuration["Jwt:ExpiryMinutes"];

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = _audience,
        ValidIssuer = _issuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key))
        //ValidateIssuerSigningKey = true,
        //ValidateIssuer = false,
        //ValidateAudience = false,
        //IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key))
    };
});

// Dependency injection with key
builder.Services.AddSingleton<ITokenGenerator>(new TokenGenerator(_key, _issuer, _audience, _expirtyMinutes));
//builder.Services.AddSingleton<ITokenGenerator>(new TokenGenerator(_key));

// Include Infrastructur Dependency
builder.Services.AddInfrastructure(builder.Configuration);



// Configuration for Sqlite
//builder.Services.AddDbContext<OrderingContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register dependencies
builder.Services.AddMediatR(typeof(CreateCustomerCommandHandler).GetTypeInfo().Assembly);
builder.Services.AddMediatR(typeof(CreateUserCommandHandler).GetTypeInfo().Assembly);


//Enable CORS//Cross site resource sharing
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
        //.AllowCredentials()
        );
});


//builder.Services.AddScoped(typeof(IQueryRepository<>), typeof(QueryRepository<>));
//builder.Services.AddTransient<ICustomerQueryRepository, CustomerQueryRepository>();
//builder.Services.AddScoped(typeof(ICommandRepository<>), typeof(CommandRepository<>));
//builder.Services.AddTransient<ICustomerCommandRepository, CustomerCommandRepository>();
//builder.Services.AddScoped<IIdentityService, IdentityService>();

//builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
//    .AddEntityFrameworkStores<OrderingContext>()
//    .AddDefaultTokenProviders();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{

    // To enable authorization using swagger (Jwt)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer {token}\"",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
                {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}

                    }
                });

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Must be betwwen app.UseRouting() and app.UseEndPoints()
app.UseCors("CorsPolicy");

// Added for authentication
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
