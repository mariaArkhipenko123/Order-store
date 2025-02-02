using Google.Apis.Auth;
using Lab.Application.Interfaces.Auth;
using Lab.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Lab.Infrastructure.Services
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly UserManager<User> _userManager;
        public GoogleAuthService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<User> AuthenticateWithGoogleAsync(string idToken)
        {
            var payload = await VerifyGoogleTokenAsync(idToken);
            if (payload == null)
            {
                throw new Exception("Invalid Google ID token");
            }
            var user = await _userManager.FindByEmailAsync(payload.Email);
            if (user == null)
            {
                user = new User
                {
                    Email = payload.Email,
                    UserName = payload.Email,
                    Issuer = payload.Issuer,
                };
                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    throw new Exception("Failed to create user");
                }
                return user;
            }
            else
            {
                if (payload.Issuer == user.Issuer) return user;
                throw new Exception("User has been already registered in another way");
            }
        }
        private async Task<GoogleJsonWebSignature.Payload> VerifyGoogleTokenAsync(string idToken)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
                return payload;
            }
            catch (Exception ex)
            {
                throw new Exception("Error validating Google ID token", ex);
            }
        }
    }
}
