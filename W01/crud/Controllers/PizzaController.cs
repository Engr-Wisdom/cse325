using ContosoPizza.Models;
using ContosoPizza.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContosoPizza.Controllers;

[ApiController]
[Route("[controller]")]
public class PizzaController : ControllerBase
{
    public PizzaController()
    {
    }
    
    // GET all pizzas
    [HttpGet]
    public ActionResult<List<Pizza>> GetAll()
    {
        return PizzaService.GetAll();
    }

    // GET pizza by ID
    [HttpGet("{id}")]
    public ActionResult<Pizza> Get(int id)
    {
        var pizza = PizzaService.Get(id);

        if (pizza is null)
            return NotFound();

        return pizza;
    }

    // POST - create a new pizza
    [HttpPost]
    public IActionResult Create(Pizza pizza)
    {
        PizzaService.Add(pizza);

        return CreatedAtAction(nameof(Get), new { id = pizza.Id }, pizza);
    }

    // PUT - update an existing pizza
    [HttpPut("{id}")]
    public IActionResult Update(int id, Pizza pizza)
    {
        var existingPizza = PizzaService.Get(id);

        if (existingPizza is null)
            return NotFound();

        pizza.Id = id;

        PizzaService.Update(pizza);

        return NoContent();
    }

    // DELETE - delete a pizza
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var pizza = PizzaService.Get(id);

        if (pizza is null)
            return NotFound();

        PizzaService.Delete(id);

        return NoContent();
    }
}