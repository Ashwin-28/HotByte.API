using System.ComponentModel.DataAnnotations;

namespace HotByte.API.DTOs;

public record AddToCartDTO(
    [Required] int MenuItemId,
    [Required, Range(1, int.MaxValue)] int Quantity
);

public record UpdateCartItemDTO(
    [Required, Range(1, int.MaxValue)] int Quantity
);

public record CartResponseDTO(
    int CartId,
    List<CartItemDTO> Items,
    decimal TotalAmount,
    int ItemCount
);

public record CartItemDTO(
    int CartItemId,
    int MenuItemId,
    string ItemName,
    string? ImageUrl,
    decimal UnitPrice,
    int Quantity,
    decimal ItemTotal
);
