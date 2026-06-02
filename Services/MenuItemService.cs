using HotByte.API.Data;
using HotByte.API.DTOs;
using HotByte.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HotByte.API.Services;

public class MenuItemService : IMenuItemService
{
    private readonly ApplicationDbContext _context;

    public MenuItemService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MenuItemResponseDTO>> GetAllAsync()
    {
        return await _context.MenuItems
            .Include(m => m.Category)
            .Include(m => m.Restaurant)
            .Where(m => m.IsAvailable)
            .Select(m => MapToResponse(m))
            .ToListAsync();
    }

    public async Task<MenuItemResponseDTO?> GetByIdAsync(int id)
    {
        var item = await _context.MenuItems
            .Include(m => m.Category)
            .Include(m => m.Restaurant)
            .FirstOrDefaultAsync(m => m.MenuItemId == id);

        return item is null ? null : MapToResponse(item);
    }

    public async Task<IEnumerable<MenuItemResponseDTO>> GetByRestaurantAsync(int restaurantId)
    {
        return await _context.MenuItems
            .Include(m => m.Category)
            .Include(m => m.Restaurant)
            .Where(m => m.RestaurantId == restaurantId && m.IsAvailable)
            .Select(m => MapToResponse(m))
            .ToListAsync();
    }

    public async Task<IEnumerable<MenuItemResponseDTO>> GetByCategoryAsync(int categoryId)
    {
        return await _context.MenuItems
            .Include(m => m.Category)
            .Include(m => m.Restaurant)
            .Where(m => m.CategoryId == categoryId && m.IsAvailable)
            .Select(m => MapToResponse(m))
            .ToListAsync();
    }

    public async Task<IEnumerable<MenuItemResponseDTO>> SearchAsync(string query)
    {
        return await _context.MenuItems
            .Include(m => m.Category)
            .Include(m => m.Restaurant)
            .Where(m => m.IsAvailable && m.Name.Contains(query))
            .Select(m => MapToResponse(m))
            .ToListAsync();
    }

    public async Task<MenuItemResponseDTO> CreateAsync(int restaurantId, CreateMenuItemDTO dto)
    {
        var item = new MenuItem
        {
            RestaurantId = restaurantId,
            CategoryId = dto.CategoryId,
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            DiscountPrice = dto.DiscountPrice,
            IsVeg = dto.IsVeg,
            TasteInfo = dto.TasteInfo,
            Calories = dto.Calories,
            Proteins = dto.Proteins,
            Fats = dto.Fats,
            Carbohydrates = dto.Carbohydrates,
            CookingTime = dto.CookingTime,
            AvailableTime = dto.AvailableTime,
            ImageUrl = dto.ImageUrl
        };

        _context.MenuItems.Add(item);
        await _context.SaveChangesAsync();

        // Reload with navigation properties
        await _context.Entry(item).Reference(m => m.Category).LoadAsync();
        await _context.Entry(item).Reference(m => m.Restaurant).LoadAsync();

        return MapToResponse(item);
    }

    public async Task<MenuItemResponseDTO?> UpdateAsync(int id, int restaurantId, CreateMenuItemDTO dto)
    {
        var item = await _context.MenuItems
            .Include(m => m.Category)
            .Include(m => m.Restaurant)
            .FirstOrDefaultAsync(m => m.MenuItemId == id && m.RestaurantId == restaurantId);

        if (item is null) return null;

        item.CategoryId = dto.CategoryId;
        item.Name = dto.Name;
        item.Description = dto.Description;
        item.Price = dto.Price;
        item.DiscountPrice = dto.DiscountPrice;
        item.IsVeg = dto.IsVeg;
        item.TasteInfo = dto.TasteInfo;
        item.Calories = dto.Calories;
        item.Proteins = dto.Proteins;
        item.Fats = dto.Fats;
        item.Carbohydrates = dto.Carbohydrates;
        item.CookingTime = dto.CookingTime;
        item.AvailableTime = dto.AvailableTime;
        item.ImageUrl = dto.ImageUrl;

        await _context.SaveChangesAsync();

        // Reload navigation properties
        await _context.Entry(item).Reference(m => m.Category).LoadAsync();

        return MapToResponse(item);
    }

    public async Task<bool> ToggleAvailabilityAsync(int id, int restaurantId)
    {
        var item = await _context.MenuItems
            .FirstOrDefaultAsync(m => m.MenuItemId == id && m.RestaurantId == restaurantId);

        if (item is null) return false;

        item.IsAvailable = !item.IsAvailable;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var item = await _context.MenuItems.FindAsync(id);
        if (item is null) return false;

        _context.MenuItems.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }

    private static MenuItemResponseDTO MapToResponse(MenuItem m) => new(
        m.MenuItemId, m.Name, m.Description,
        m.Category.Name, m.Restaurant.Name,
        m.Price, m.DiscountPrice, m.IsVeg, m.TasteInfo,
        m.Calories, m.CookingTime, m.AvailableTime,
        m.IsAvailable, m.ImageUrl
    );
}
