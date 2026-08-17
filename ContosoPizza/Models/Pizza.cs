using Microsoft.EntityFrameworkCore;
namespace ContosoPizza.Models;

public class Pizza
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsGlutenFree { get; set; }
}

public class PizzaDb : DbContext
{
    public PizzaDb(DbContextOptions<PizzaDb> options) : base(options) {}
    public DbSet<Pizza> Pizzas { get; set; } = null!;
}