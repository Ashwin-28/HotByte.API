using HotByte.API.DTOs;

namespace HotByte.API.Services;

public interface IRestaurantService
{
    Task<IEnumerable<RestaurantResponseDTO>> GetAllAsync();
    Task<RestaurantResponseDTO?> GetByIdAsync(int id);
    Task<RestaurantResponseDTO> CreateAsync(CreateRestaurantDTO dto);
    Task<RestaurantResponseDTO?> UpdateAsync(int id, CreateRestaurantDTO dto);
    Task<bool> DeleteAsync(int id);
}
