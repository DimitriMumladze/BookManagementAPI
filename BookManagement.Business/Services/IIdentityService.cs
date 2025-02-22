using BookManagement.Business.DTOs.Auth;

namespace BookManagement.Business.Services;

public interface IIdentityService
{
    Task<LoginDto> Login(LoginRequest request);
    Task<RegisterDto> Register(RegisterRequest command);
}