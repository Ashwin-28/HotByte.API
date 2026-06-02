using HotByte.API.DTOs;

namespace HotByte.API.Services;

public interface ICartService
{
    Task<CartResponseDTO> GetCartAsync(int userId);
    Task<CartResponseDTO> AddToCartAsync(int userId, AddToCartDTO dto);
    Task<CartResponseDTO> UpdateCartItemAsync(int userId, int cartItemId, UpdateCartItemDTO dto);
    Task<bool> RemoveCartItemAsync(int userId, int cartItemId);
    Task<bool> ClearCartAsync(int userId);
}
