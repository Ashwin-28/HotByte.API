using HotByte.API.DTOs;

namespace HotByte.API.Services;

public interface IAuthService
{
    Task<AuthResponseDTO> RegisterAsync(RegisterUserDTO dto);
    Task<AuthResponseDTO> LoginAsync(LoginDTO dto);
}
