using System.ComponentModel.DataAnnotations;

namespace HotByte.API.DTOs;

public record UserAddressResponseDTO(
    int AddressId,
    int UserId,
    string AddressLabel,
    string AddressLine,
    string City,
    string State,
    string PostalCode
);

public record CreateUserAddressDTO(
    [Required, MaxLength(50)] string AddressLabel, // e.g. "Home", "Work", "Office", "Other"
    [Required, MaxLength(255)] string AddressLine,
    [Required, MaxLength(100)] string City,
    [Required, MaxLength(100)] string State,
    [Required, MaxLength(20)] string PostalCode
);
