using HotByte.API.DTOs;

namespace HotByte.API.Services;

public interface IMenuItemService
{
    Task<IEnumerable<MenuItemResponseDTO>> GetAllAsync();
    Task<MenuItemResponseDTO?> GetByIdAsync(int id);
    Task<IEnumerable<MenuItemResponseDTO>> GetByRestaurantAsync(int restaurantId);
    Task<IEnumerable<MenuItemResponseDTO>> GetByCategoryAsync(int categoryId);
    Task<IEnumerable<MenuItemResponseDTO>> SearchAsync(string query);
    Task<MenuItemResponseDTO> CreateAsync(int restaurantId, CreateMenuItemDTO dto);
    Task<MenuItemResponseDTO?> UpdateAsync(int id, int restaurantId, CreateMenuItemDTO dto);
    Task<bool> ToggleAvailabilityAsync(int id, int restaurantId);
    Task<bool> DeleteAsync(int id);
}
