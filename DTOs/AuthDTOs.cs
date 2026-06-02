using System.ComponentModel.DataAnnotations;

namespace HotByte.API.DTOs;

// TODO: Implement authentication DTOs later
public record RegisterUserDTO(
    [Required, MinLength(2), MaxLength(50)] string FirstName,
    [Required, MinLength(2), MaxLength(50)] string LastName,
    [Required, EmailAddress] string Email,
    [Required, MinLength(8)] string Password,
    [Required] string ConfirmPassword,
    [Required, MaxLength(15)] string PhoneNumber,
    string? Gender = null,
    string? Address = null
);

public record LoginDTO(
    [Required, EmailAddress] string Email,
    [Required] string Password
);

public record AuthResponseDTO(
    string Token,
    string ExpiresAt,
    int UserId,
    string Name,
    string Role,
    string Email
);
