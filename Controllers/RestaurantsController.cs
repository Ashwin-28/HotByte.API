using HotByte.API.DTOs;
using HotByte.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotByte.API.Controllers;

[ApiController]
[Route("api/v1/restaurants")]
public class RestaurantsController : ControllerBase
{
    private readonly IRestaurantService _restaurantService;

    public RestaurantsController(IRestaurantService restaurantService)
    {
        _restaurantService = restaurantService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var restaurants = await _restaurantService.GetAllAsync();
        return Ok(restaurants);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var restaurant = await _restaurantService.GetByIdAsync(id);
        return restaurant is null
            ? NotFound(new { message = "Restaurant not found." })
            : Ok(restaurant);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateRestaurantDTO dto)
    {
        var restaurant = await _restaurantService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = restaurant.RestaurantId }, restaurant);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Restaurant,Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateRestaurantDTO dto)
    {
        var restaurant = await _restaurantService.UpdateAsync(id, dto);
        return restaurant is null
            ? NotFound(new { message = "Restaurant not found." })
            : Ok(restaurant);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _restaurantService.DeleteAsync(id);
        return result ? NoContent() : NotFound(new { message = "Restaurant not found." });
    }
}
