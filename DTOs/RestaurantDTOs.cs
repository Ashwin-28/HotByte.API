using System.ComponentModel.DataAnnotations;

namespace HotByte.API.DTOs;

public record CreateRestaurantDTO(
    [Required, MaxLength(100)] string Name,
    [Required] string Location,
    [Required] string PhoneNumber,
    [Required, EmailAddress] string Email,
    [Required, MinLength(8)] string Password,
    string? Description = null,
    string? ImageUrl = null
);

public record RestaurantResponseDTO(
    int RestaurantId,
    string Name,
    string Location,
    string PhoneNumber,
    string? Description,
    string? ImageUrl,
    bool IsActive
);
