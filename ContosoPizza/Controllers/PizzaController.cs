using ContosoPizza.Models;
using ContosoPizza.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContosoPizza.Controllers;

[ApiController]
[Route("[controller]")]
public class PizzaController : ControllerBase
{
    private readonly PizzaService _pizzaService;
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="pizzaService"></param>
    public PizzaController(PizzaService pizzaService)
    {
        _pizzaService = pizzaService;
    }

    // Get All
    [HttpGet]
    public async Task<ActionResult<List<Pizza>>> GetAllAsync() => await _pizzaService.GetAllAsync();

    // Get by Id
    [HttpGet("{id}")]
    public async Task<ActionResult<Pizza>> GetAsync(int id)
    {
        var pizza = await _pizzaService.GetAsync(id);
        if(pizza is null)
        {
            return NotFound();
        }
        return pizza;
    }

    // Post
    [HttpPost]
    public async Task<IActionResult> CreateAsync(Pizza pizza)
    {
        await _pizzaService.AddAsync(pizza);
        return CreatedAtAction("Get", new { id = pizza.Id }, pizza);
    }

    // Put
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(int id, Pizza pizza)
    {
        if(id != pizza.Id)
        {
            return BadRequest();
        }

        if(await _pizzaService.AnyAsync(id) == false)
        {
            return NotFound();
        }

        await _pizzaService.UpdateAsync(pizza);
        return NoContent();
    }

    // Delete
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        if(await _pizzaService.DeleteAsync(id) == false)
        {
            return NotFound();
        }

        return NoContent();
    }

}