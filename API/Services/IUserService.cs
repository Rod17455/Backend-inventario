using API.Dtos;
using API.Helpers.Errors;

namespace API.Services;

public interface IUserService
{
    Task<ManagementResponse> RegisterAsync(RegisterDto model);
    Task<DatosUsuarioDto> GetTokenAsync(LoginDto model);
    Task<ManagementResponse> AddRoleAsync(AddRoleDto model);
    Task<DatosUsuarioDto> RefreshTokenAsync(string refreshToken);
}
