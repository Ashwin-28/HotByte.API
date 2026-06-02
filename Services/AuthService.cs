using HotByte.API.Data;
using HotByte.API.DTOs;

namespace HotByte.API.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;

    public AuthService(ApplicationDbContext context)
    {
        _context = context;
    }

    // TODO: Implement authentication logic later
    public Task<AuthResponseDTO> RegisterAsync(RegisterUserDTO dto)
    {
        throw new NotImplementedException("Auth registration will be implemented later.");
    }

    public Task<AuthResponseDTO> LoginAsync(LoginDTO dto)
    {
        throw new NotImplementedException("Auth login will be implemented later.");
    }
}
