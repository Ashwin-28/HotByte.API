using System.Security.Claims;
using HotByte.API.DTOs;
using HotByte.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotByte.API.Controllers;

[ApiController]
[Route("api/v1/cart")]
[Authorize]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        var cart = await _cartService.GetCartAsync(GetUserId());
        return Ok(cart);
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartDTO dto)
    {
        try
        {
            var cart = await _cartService.AddToCartAsync(GetUserId(), dto);
            return Ok(cart);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPut("update/{cartItemId}")]
    public async Task<IActionResult> UpdateCartItem(int cartItemId, [FromBody] UpdateCartItemDTO dto)
    {
        try
        {
            var cart = await _cartService.UpdateCartItemAsync(GetUserId(), cartItemId, dto);
            return Ok(cart);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("remove/{cartItemId}")]
    public async Task<IActionResult> RemoveCartItem(int cartItemId)
    {
        var result = await _cartService.RemoveCartItemAsync(GetUserId(), cartItemId);
        return result ? NoContent() : NotFound(new { message = "Cart item not found." });
    }

    [HttpDelete("clear")]
    public async Task<IActionResult> ClearCart()
    {
        var result = await _cartService.ClearCartAsync(GetUserId());
        return result ? NoContent() : NotFound(new { message = "Cart not found." });
    }
}
