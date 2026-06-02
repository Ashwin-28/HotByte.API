using HotByte.API.DTOs;

namespace HotByte.API.Services;

public interface IOrderService
{
    Task<OrderResponseDTO> PlaceOrderAsync(int userId, PlaceOrderDTO dto);
    Task<IEnumerable<OrderResponseDTO>> GetUserOrdersAsync(int userId);
    Task<OrderResponseDTO?> GetOrderByIdAsync(int orderId, int userId);
    Task<IEnumerable<OrderResponseDTO>> GetRestaurantOrdersAsync(int restaurantId);
    Task<IEnumerable<OrderResponseDTO>> GetAllOrdersAsync();
    Task<OrderResponseDTO?> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusDTO dto);
    Task<bool> CancelOrderAsync(int orderId, int userId);
}
