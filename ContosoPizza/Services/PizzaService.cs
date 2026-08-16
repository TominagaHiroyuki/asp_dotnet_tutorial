using ContosoPizza.Models;
using System.Collections.Generic;

namespace ContosoPizza.Services;

public static class PizzaService
{
    static List<Pizza> Pizzas { get; set; } = new();
    static int nextId = 3;
    static PizzaService()
    {
        Pizzas.Add(new Pizza { Id = 1, Name = "Classic Italian", IsGlutenFree = false });
        Pizzas.Add(new Pizza { Id = 2, Name = "Veggie", IsGlutenFree = true });
    }

    public static List<Pizza> GetAll() => Pizzas;
    public static Pizza? Get(int id) => Pizzas.FirstOrDefault(p => p.Id == id);

    /// <summary>
    /// Pizzaを追加
    /// </summary>
    /// <param name="pizza"></param>
    public static void Add(Pizza pizza)
    {
        pizza.Id = nextId++;
        Pizzas.Add(pizza);
    }

    /// <summary>
    /// Pizzaを削除
    /// </summary>
    /// <param name="id"></param>
    public static void Delete(int id)
    {
        var pizza = Get(id);
        if(pizza is null)
        {
            return;
        }

        Pizzas.Remove(pizza);
    }

    public static void Update(Pizza pizza)
    {
        var index = Pizzas.FindIndex(p => p.Id == pizza.Id);
        if(index <= -1)
        {
            return;
        }

        Pizzas[index] = pizza;
    }
}