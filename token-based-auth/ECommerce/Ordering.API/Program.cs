using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Handlers.CommandHandler;
using Ordering.Core.Repositories.Command;
using Ordering.Core.Repositories.Command.Base;
using Ordering.Core.Repositories.Query;
using Ordering.Core.Repositories.Query.Base;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Repository.Command;
using Ordering.Infrastructure.Repository.Command.Base;
using Ordering.Infrastructure.Repository.Query;
using Ordering.Infrastructure.Repository.Query.Base;
using System.Reflection;
//using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


// Configuration for Sqlite
builder.Services.AddDbContext<OrderingContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register dependencies
builder.Services.AddMediatR(typeof(CreateCustomerHandler).GetTypeInfo().Assembly);
builder.Services.AddScoped(typeof(IQueryRepository<>), typeof(QueryRepository<>));
builder.Services.AddTransient<ICustomerQueryRepository, CustomerQueryRepository>();
builder.Services.AddScoped(typeof(ICommandRepository<>), typeof(CommandRepository<>));
builder.Services.AddTransient<ICustomerCommandRepository, CustomerCommandRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
