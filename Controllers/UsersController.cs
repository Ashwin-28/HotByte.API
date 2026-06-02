using System.Security.Claims;
using HotByte.API.DTOs;
using HotByte.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotByte.API.Controllers;

[ApiController]
[Route("api/v1/users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        try
        {
            var profile = await _userService.GetProfileAsync(GetUserId());
            return Ok(profile);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserDTO dto)
    {
        try
        {
            var profile = await _userService.UpdateProfileAsync(GetUserId(), dto);
            return Ok(profile);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var result = await _userService.DeleteUserAsync(id);
        return result ? NoContent() : NotFound(new { message = "User not found." });
    }

    [HttpGet("addresses")]
    public async Task<IActionResult> GetAddresses()
    {
        var addresses = await _userService.GetUserAddressesAsync(GetUserId());
        return Ok(addresses);
    }

    [HttpPost("addresses")]
    public async Task<IActionResult> AddAddress([FromBody] CreateUserAddressDTO dto)
    {
        try
        {
            var address = await _userService.AddUserAddressAsync(GetUserId(), dto);
            return CreatedAtAction(nameof(GetAddresses), address);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("addresses/{id}")]
    public async Task<IActionResult> DeleteAddress(int id)
    {
        var result = await _userService.DeleteUserAddressAsync(GetUserId(), id);
        return result
            ? Ok(new { message = "Address deleted successfully." })
            : NotFound(new { message = "Address not found or does not belong to you." });
    }
}
