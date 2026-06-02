using HotByte.API.Data;
using HotByte.API.DTOs;
using HotByte.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HotByte.API.Services;

public class CartService : ICartService
{
    private readonly ApplicationDbContext _context;

    public CartService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CartResponseDTO> GetCartAsync(int userId)
    {
        var cart = await GetOrCreateCartAsync(userId);
        return MapToResponse(cart);
    }

    public async Task<CartResponseDTO> AddToCartAsync(int userId, AddToCartDTO dto)
    {
        var cart = await GetOrCreateCartAsync(userId);

        var menuItem = await _context.MenuItems.FindAsync(dto.MenuItemId)
            ?? throw new KeyNotFoundException($"Menu item with ID {dto.MenuItemId} not found.");

        var existingItem = cart.CartItems
            .FirstOrDefault(ci => ci.MenuItemId == dto.MenuItemId);

        if (existingItem is not null)
        {
            existingItem.Quantity += dto.Quantity;
        }
        else
        {
            var cartItem = new CartItem
            {
                CartId = cart.CartId,
                MenuItemId = dto.MenuItemId,
                Quantity = dto.Quantity,
                UnitPrice = menuItem.DiscountPrice ?? menuItem.Price
            };
            cart.CartItems.Add(cartItem);
        }

        cart.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        // Reload with navigation properties
        cart = await GetOrCreateCartAsync(userId);
        return MapToResponse(cart);
    }

    public async Task<CartResponseDTO> UpdateCartItemAsync(int userId, int cartItemId, UpdateCartItemDTO dto)
    {
        var cart = await GetOrCreateCartAsync(userId);

        var cartItem = cart.CartItems.FirstOrDefault(ci => ci.CartItemId == cartItemId)
            ?? throw new KeyNotFoundException($"Cart item with ID {cartItemId} not found.");

        cartItem.Quantity = dto.Quantity;
        cart.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        cart = await GetOrCreateCartAsync(userId);
        return MapToResponse(cart);
    }

    public async Task<bool> RemoveCartItemAsync(int userId, int cartItemId)
    {
        var cart = await GetOrCreateCartAsync(userId);

        var cartItem = cart.CartItems.FirstOrDefault(ci => ci.CartItemId == cartItemId);
        if (cartItem is null) return false;

        _context.CartItems.Remove(cartItem);
        cart.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ClearCartAsync(int userId)
    {
        var cart = await _context.Carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart is null) return false;

        _context.CartItems.RemoveRange(cart.CartItems);
        cart.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    private async Task<Cart> GetOrCreateCartAsync(int userId)
    {
        var cart = await _context.Carts
            .Include(c => c.CartItems)
                .ThenInclude(ci => ci.MenuItem)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart is null)
        {
            cart = new Cart { UserId = userId };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            cart = await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.MenuItem)
                .FirstAsync(c => c.UserId == userId);
        }

        return cart;
    }

    private static CartResponseDTO MapToResponse(Cart cart)
    {
        var items = cart.CartItems.Select(ci => new CartItemDTO(
            ci.CartItemId,
            ci.MenuItemId,
            ci.MenuItem.Name,
            ci.MenuItem.ImageUrl,
            ci.UnitPrice,
            ci.Quantity,
            ci.UnitPrice * ci.Quantity
        )).ToList();

        return new CartResponseDTO(
            cart.CartId,
            items,
            items.Sum(i => i.ItemTotal),
            items.Sum(i => i.Quantity)
        );
    }
}
