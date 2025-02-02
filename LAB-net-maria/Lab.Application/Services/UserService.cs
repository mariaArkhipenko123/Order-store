using Lab.Application.Interfaces;
using Lab.Application.Interfaces.Auth;
using Lab.Application.Interfaces.Logical;
using Lab.Application.Models.DTOs.Secure;
using Lab.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Lab.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthUserService _authUserService; 

        public UserService(IUnitOfWork unitOfWork, IAuthUserService authUserService)
        {
            _unitOfWork = unitOfWork;
            _authUserService = authUserService;
        }

        public async Task<IdentityResult> RegisterAsync(АuthorDTO authorDTO)
        {
            return await _authUserService.RegisterAsync(authorDTO);
        }
        public async Task<(IdentityResult Result, string? Token, string? RefreshToken)> LoginAsync(АuthorDTO authorDTO)
        {
            return await _authUserService.LoginAsync(authorDTO);
        }
        public async Task<(IdentityResult Result, string? Token, string? RefreshToken)> LoginWithGoogle(GoogleLoginDTO googleLoginParams)
        {
            return await _authUserService.LoginWithGoogle(googleLoginParams);
        }
        public async Task<(IdentityResult Result, string? Token, string? RefreshToken)> LoginWithFirebase(FirebaseLoginDTO firebaseLoginParams)
        {
            return await _authUserService.LoginWithFirebase(firebaseLoginParams);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _unitOfWork.Users.GetAllAsync();
        }

        public async Task AddUserAsync(User user)
        {
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            await _unitOfWork.Users.DeleteAsync(userId);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            return await _unitOfWork.Users.GetByIdAsync(userId);
        }
    }
}
