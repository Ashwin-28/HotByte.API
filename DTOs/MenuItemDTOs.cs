using System.ComponentModel.DataAnnotations;

namespace HotByte.API.DTOs;

public record CreateMenuItemDTO(
    [Required, MaxLength(100)] string Name,
    string? Description,
    [Required] int CategoryId,
    [Required] decimal Price,
    decimal? DiscountPrice,
    [Required] bool IsVeg,
    string? TasteInfo = null,
    int? Calories = null,
    decimal? Proteins = null,
    decimal? Fats = null,
    decimal? Carbohydrates = null,
    int? CookingTime = null,
    string? AvailableTime = null,
    string? ImageUrl = null
);

public record MenuItemResponseDTO(
    int MenuItemId,
    string Name,
    string? Description,
    string CategoryName,
    string RestaurantName,
    decimal Price,
    decimal? DiscountPrice,
    bool IsVeg,
    string? TasteInfo,
    int? Calories,
    int? CookingTime,
    string? AvailableTime,
    bool IsAvailable,
    string? ImageUrl
);
