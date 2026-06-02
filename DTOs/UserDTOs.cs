namespace HotByte.API.DTOs;

public record UserProfileDTO(
    int UserId,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string? Gender,
    string? Address,
    string CreatedAt
);

public record UpdateUserDTO(
    string? FirstName = null,
    string? LastName = null,
    string? PhoneNumber = null,
    string? Address = null,
    string? Gender = null
);
