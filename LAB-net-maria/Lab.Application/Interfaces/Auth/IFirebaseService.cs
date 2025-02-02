using Lab.Domain.Entities;

namespace Lab.Application.Interfaces.Auth
{
    public interface IFirebaseService
    {
        Task<User> AuthenticateWithFirebase(string IdFirebaseToken);
        Task<string> RegisterUserAsync(string email, string password, string uid);
    }
}