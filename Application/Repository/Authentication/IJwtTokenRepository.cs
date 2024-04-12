using Domain.Models.Authentication;

namespace Application.Repository.Authentication
{
    public interface IJwtTokenRepository
    {
        string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles);
    }
}