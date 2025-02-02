using Lab.Application.Models.DTOs.Secure;
using Microsoft.AspNetCore.Identity;

namespace Lab.Application.Interfaces.Auth
{
    public interface IAuthUserService
    {
        Task<(IdentityResult Result, string? Token, string? RefreshToken)> LoginAsync(АuthorDTO authorDTO);
        Task<(IdentityResult Result, string? Token, string? RefreshToken)> LoginWithFirebase(FirebaseLoginDTO firebaseLoginParams);
        Task<(IdentityResult Result, string? Token, string? RefreshToken)> LoginWithGoogle(GoogleLoginDTO googleLoginParams);
        Task<(IdentityResult Result, string? Token, string? RefreshToken)> RefreshTokenAsync(string email);
        Task<IdentityResult> RegisterAsync(АuthorDTO authorDTO);
    }
}