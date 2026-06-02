using HotByte.API.Data;
using HotByte.API.DTOs;
using HotByte.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HotByte.API.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserProfileDTO> GetProfileAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId)
            ?? throw new KeyNotFoundException($"User with ID {userId} not found.");

        return MapToProfile(user);
    }

    public async Task<UserProfileDTO> UpdateProfileAsync(int userId, UpdateUserDTO dto)
    {
        var user = await _context.Users.FindAsync(userId)
            ?? throw new KeyNotFoundException($"User with ID {userId} not found.");

        if (dto.FirstName is not null) user.FirstName = dto.FirstName;
        if (dto.LastName is not null) user.LastName = dto.LastName;
        if (dto.PhoneNumber is not null) user.PhoneNumber = dto.PhoneNumber;
        if (dto.Address is not null) user.Address = dto.Address;
        if (dto.Gender is not null) user.Gender = dto.Gender;

        await _context.SaveChangesAsync();
        return MapToProfile(user);
    }

    public async Task<IEnumerable<UserProfileDTO>> GetAllUsersAsync()
    {
        return await _context.Users
            .Select(u => new UserProfileDTO(
                u.UserId, u.FirstName, u.LastName, u.Email,
                u.PhoneNumber, u.Gender, u.Address,
                u.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
            ))
            .ToListAsync();
    }

    public async Task<bool> DeleteUserAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user is null) return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<UserAddressResponseDTO>> GetUserAddressesAsync(int userId)
    {
        var addresses = await _context.UserAddresses
            .Where(ua => ua.UserId == userId && ua.IsActive)
            .OrderByDescending(ua => ua.CreatedAt)
            .ToListAsync();

        return addresses.Select(MapToAddressResponse);
    }

    public async Task<UserAddressResponseDTO> AddUserAddressAsync(int userId, CreateUserAddressDTO dto)
    {
        var userExists = await _context.Users.AnyAsync(u => u.UserId == userId);
        if (!userExists)
            throw new KeyNotFoundException($"User with ID {userId} not found.");

        var address = new UserAddress
        {
            UserId = userId,
            AddressLabel = dto.AddressLabel,
            AddressLine = dto.AddressLine,
            City = dto.City,
            State = dto.State,
            PostalCode = dto.PostalCode,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.UserAddresses.Add(address);
        await _context.SaveChangesAsync();

        return MapToAddressResponse(address);
    }

    public async Task<bool> DeleteUserAddressAsync(int userId, int addressId)
    {
        var address = await _context.UserAddresses
            .FirstOrDefaultAsync(ua => ua.AddressId == addressId && ua.UserId == userId);

        if (address is null) return false;

        address.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }

    private static UserProfileDTO MapToProfile(User user) => new(
        user.UserId, user.FirstName, user.LastName, user.Email,
        user.PhoneNumber, user.Gender, user.Address,
        user.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
    );

    private static UserAddressResponseDTO MapToAddressResponse(UserAddress ua) => new(
        ua.AddressId,
        ua.UserId,
        ua.AddressLabel,
        ua.AddressLine,
        ua.City,
        ua.State,
        ua.PostalCode
    );
}
