using HotByte.API.DTOs;
using HotByte.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotByte.API.Controllers;

[ApiController]
[Route("api/v1/menuitems")]
public class MenuItemsController : ControllerBase
{
    private readonly IMenuItemService _menuItemService;

    public MenuItemsController(IMenuItemService menuItemService)
    {
        _menuItemService = menuItemService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _menuItemService.GetAllAsync();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _menuItemService.GetByIdAsync(id);
        return item is null
            ? NotFound(new { message = "Menu item not found." })
            : Ok(item);
    }

    [HttpGet("restaurant/{id}")]
    public async Task<IActionResult> GetByRestaurant(int id)
    {
        var items = await _menuItemService.GetByRestaurantAsync(id);
        return Ok(items);
    }

    [HttpGet("category/{id}")]
    public async Task<IActionResult> GetByCategory(int id)
    {
        var items = await _menuItemService.GetByCategoryAsync(id);
        return Ok(items);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string q)
    {
        if (string.IsNullOrWhiteSpace(q))
            return BadRequest(new { message = "Search query is required." });

        var items = await _menuItemService.SearchAsync(q);
        return Ok(items);
    }

    [HttpPost]
    [Authorize(Roles = "Restaurant,Admin")]
    public async Task<IActionResult> Create([FromBody] CreateMenuItemDTO dto, [FromQuery] int restaurantId)
    {
        var item = await _menuItemService.CreateAsync(restaurantId, dto);
        return CreatedAtAction(nameof(GetById), new { id = item.MenuItemId }, item);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Restaurant,Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateMenuItemDTO dto, [FromQuery] int restaurantId)
    {
        var item = await _menuItemService.UpdateAsync(id, restaurantId, dto);
        return item is null
            ? NotFound(new { message = "Menu item not found." })
            : Ok(item);
    }

    [HttpPatch("{id}/availability")]
    [Authorize(Roles = "Restaurant,Admin")]
    public async Task<IActionResult> ToggleAvailability(int id, [FromQuery] int restaurantId)
    {
        var result = await _menuItemService.ToggleAvailabilityAsync(id, restaurantId);
        return result
            ? Ok(new { message = "Availability toggled successfully." })
            : NotFound(new { message = "Menu item not found." });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Restaurant,Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _menuItemService.DeleteAsync(id);
        return result ? NoContent() : NotFound(new { message = "Menu item not found." });
    }
}
