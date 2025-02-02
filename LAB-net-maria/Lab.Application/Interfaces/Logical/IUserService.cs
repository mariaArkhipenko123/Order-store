using Lab.Application.Models.DTOs.Secure;
using Lab.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Lab.Application.Interfaces.Logical
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterAsync(АuthorDTO authorDTO);
        Task<(IdentityResult Result, string? Token, string? RefreshToken)> LoginAsync(АuthorDTO authorDTO);
        Task<(IdentityResult Result, string? Token, string? RefreshToken)> LoginWithGoogle(GoogleLoginDTO googleLoginParams);
        Task<(IdentityResult Result, string? Token, string? RefreshToken)> LoginWithFirebase(FirebaseLoginDTO firebaseLoginParams);
        Task AddUserAsync(User user);
        Task DeleteUserAsync(Guid userId);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(Guid userId);
        Task UpdateUserAsync(User user);
    }
}