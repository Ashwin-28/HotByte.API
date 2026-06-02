using HotByte.API.Data;
using HotByte.API.DTOs;
using HotByte.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HotByte.API.Services;

public class RestaurantService : IRestaurantService
{
    private readonly ApplicationDbContext _context;

    public RestaurantService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RestaurantResponseDTO>> GetAllAsync()
    {
        return await _context.Restaurants
            .Where(r => r.IsActive)
            .Select(r => MapToResponse(r))
            .ToListAsync();
    }

    public async Task<RestaurantResponseDTO?> GetByIdAsync(int id)
    {
        var restaurant = await _context.Restaurants.FindAsync(id);
        return restaurant is null ? null : MapToResponse(restaurant);
    }

    public async Task<RestaurantResponseDTO> CreateAsync(CreateRestaurantDTO dto)
    {
        var restaurant = new Restaurant
        {
            Name = dto.Name,
            Location = dto.Location,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Description = dto.Description,
            ImageUrl = dto.ImageUrl
        };

        _context.Restaurants.Add(restaurant);
        await _context.SaveChangesAsync();
        return MapToResponse(restaurant);
    }

    public async Task<RestaurantResponseDTO?> UpdateAsync(int id, CreateRestaurantDTO dto)
    {
        var restaurant = await _context.Restaurants.FindAsync(id);
        if (restaurant is null) return null;

        restaurant.Name = dto.Name;
        restaurant.Location = dto.Location;
        restaurant.PhoneNumber = dto.PhoneNumber;
        restaurant.Email = dto.Email;
        restaurant.Description = dto.Description;
        restaurant.ImageUrl = dto.ImageUrl;

        await _context.SaveChangesAsync();
        return MapToResponse(restaurant);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var restaurant = await _context.Restaurants.FindAsync(id);
        if (restaurant is null) return false;

        restaurant.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }

    private static RestaurantResponseDTO MapToResponse(Restaurant r) => new(
        r.RestaurantId, r.Name, r.Location, r.PhoneNumber,
        r.Description, r.ImageUrl, r.IsActive
    );
}
