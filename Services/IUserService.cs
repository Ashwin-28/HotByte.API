using HotByte.API.DTOs;

namespace HotByte.API.Services;

public interface IUserService
{
    Task<UserProfileDTO> GetProfileAsync(int userId);
    Task<UserProfileDTO> UpdateProfileAsync(int userId, UpdateUserDTO dto);
    Task<IEnumerable<UserProfileDTO>> GetAllUsersAsync();
    Task<bool> DeleteUserAsync(int userId);
    Task<IEnumerable<UserAddressResponseDTO>> GetUserAddressesAsync(int userId);
    Task<UserAddressResponseDTO> AddUserAddressAsync(int userId, CreateUserAddressDTO dto);
    Task<bool> DeleteUserAddressAsync(int userId, int addressId);
}
