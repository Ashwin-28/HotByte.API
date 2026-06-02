using System.ComponentModel.DataAnnotations;

namespace HotByte.API.DTOs;

public record PlaceOrderDTO(
    int? AddressId,
    CreateUserAddressDTO? NewAddress,
    [Required] string PaymentMethod
);

public record OrderResponseDTO(
    int OrderId,
    string RestaurantName,
    List<OrderItemDTO> Items,
    decimal TotalAmount,
    string DeliveryAddress,
    string Status,
    string PaymentMethod,
    string PaymentStatus,
    string? EstimatedDelivery,
    string PlacedAt
);

public record OrderItemDTO(
    int OrderItemId,
    int MenuItemId,
    string ItemName,
    int Quantity,
    decimal UnitPrice,
    decimal ItemTotal
);

public record UpdateOrderStatusDTO(
    [Required] string Status,
    DateTime? EstimatedDelivery = null
);
