using Scalar.AspNetCore;
using ContosoPizza.Models;
using Microsoft.EntityFrameworkCore;
using ContosoPizza.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Pizzas")
                    ?? throw new InvalidOperationException("Connection string 'Pizzas' not found.");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//builder.Services.AddDbContext<PizzaDb>(options => options.UseInMemoryDatabase("Items"));
//builder.Services.AddSqlite<PizzaDb>(connectionString);
builder.Services.AddDbContext<PizzaDb>(options => options.UseNpgsql(connectionString));
builder.Services.AddScoped<PizzaService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PizzaDb>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
