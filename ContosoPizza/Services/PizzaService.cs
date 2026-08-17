using ContosoPizza.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Services;

public class PizzaService
{
    private readonly PizzaDb _db;

    public PizzaService(PizzaDb db)
    {
        _db = db;
    }

    public async Task<List<Pizza>> GetAllAsync()
    {
        return await _db.Pizzas.ToListAsync();
    }

    public async Task<Pizza?> GetAsync(int id)
    {
        return await _db.Pizzas.FindAsync(id);
    }

    /// <summary>
    /// Pizzaが存在するかどうかを確認
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<bool> AnyAsync(int id)
    {
        return await _db.Pizzas.AnyAsync(p => p.Id == id);
    }

    /// <summary>
    /// Pizzaを追加
    /// </summary>
    /// <param name="pizza"></param>
    public async Task AddAsync(Pizza pizza)
    {
        _db.Pizzas.Add(pizza);
        await _db.SaveChangesAsync();
    }

    /// <summary>
    /// Pizzaを削除する。対象が無ければ false を返す。
    /// </summary>
    /// <param name="id"></param>
    public async Task<bool> DeleteAsync(int id)
    {
        var pizza = await _db.Pizzas.FindAsync(id);
        if(pizza is null)
        {
            return false;
        }
        _db.Pizzas.Remove(pizza);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task UpdateAsync(Pizza pizza)
    {
        _db.Pizzas.Update(pizza);
        await _db.SaveChangesAsync();
    }
}