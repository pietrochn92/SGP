using Application.DTOs.Authentication;

namespace Application.Repository.Authentication
{
    public interface IAuthRepository
    {
        Task<string> Register(RegistrationDto registrationRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<bool> AssignRole(string email, string roleName);
    }
}