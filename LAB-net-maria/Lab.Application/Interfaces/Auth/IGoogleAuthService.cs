using Lab.Domain.Entities;

namespace Lab.Application.Interfaces.Auth
{
    public interface IGoogleAuthService
    {
        Task<User> AuthenticateWithGoogleAsync(string idToken);
    }
}