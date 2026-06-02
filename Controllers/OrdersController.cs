using System.Security.Claims;
using HotByte.API.DTOs;
using HotByte.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotByte.API.Controllers;

[ApiController]
[Route("api/v1/orders")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

    [HttpPost]
    public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderDTO dto)
    {
        try
        {
            var order = await _orderService.PlaceOrderAsync(GetUserId(), dto);
            return CreatedAtAction(nameof(GetOrderById), new { id = order.OrderId }, order);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetUserOrders()
    {
        var orders = await _orderService.GetUserOrdersAsync(GetUserId());
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var order = await _orderService.GetOrderByIdAsync(id, GetUserId());
        return order is null
            ? NotFound(new { message = "Order not found." })
            : Ok(order);
    }

    [HttpGet("restaurant")]
    [Authorize(Roles = "Restaurant,Admin")]
    public async Task<IActionResult> GetRestaurantOrders([FromQuery] int restaurantId)
    {
        var orders = await _orderService.GetRestaurantOrdersAsync(restaurantId);
        return Ok(orders);
    }

    [HttpPatch("{id}/status")]
    [Authorize(Roles = "Restaurant,Admin")]
    public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusDTO dto)
    {
        var order = await _orderService.UpdateOrderStatusAsync(id, dto);
        return order is null
            ? NotFound(new { message = "Order not found." })
            : Ok(order);
    }

    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        return Ok(orders);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> CancelOrder(int id)
    {
        var result = await _orderService.CancelOrderAsync(id, GetUserId());
        return result
            ? Ok(new { message = "Order cancelled successfully." })
            : BadRequest(new { message = "Order cannot be cancelled. It may not exist or is no longer pending." });
    }
}
