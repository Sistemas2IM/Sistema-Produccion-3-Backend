using Sistema_Produccion_3_Backend.DTO.LoginAuth;

namespace Sistema_Produccion_3_Backend.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> AuthResponse(LoginDto login);
        Task<AuthResponse> RefreshTokenResponse(TokenDto refreshToken, string user);
        Task<RegisterResponse> RegisterUser(RegisterDto register);
        Task<RegisterResponse> UpdateUser(UpdateUserDto update);
    }
}
