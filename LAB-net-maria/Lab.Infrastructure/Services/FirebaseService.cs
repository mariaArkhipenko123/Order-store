using FirebaseAdmin.Auth;
using Lab.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using FirebaseAdmin;
using Lab.Application.Interfaces.Auth;

namespace Lab.Infrastructure.Services
{
    public class FirebaseService : IFirebaseService
    {
        private readonly UserManager<User> _userManager;

        public FirebaseService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> RegisterUserAsync(string uid, string email, string password)
        {
            try
            {
                var user = await FirebaseAuth.DefaultInstance.CreateUserAsync(new UserRecordArgs()
                {
                    Uid = uid,  
                    Email = email,
                    Password = password,
                });

                return user.Uid; 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating new user: {ex.Message}");
                return null;
            }
        }
        public async Task<User> AuthenticateWithFirebase(string IdFirebaseToken)
        {
            var decodedToken = await ValidateToken(IdFirebaseToken);
            if (decodedToken.Claims.ContainsKey("email"))
            {
                var email = decodedToken.Claims["email"].ToString();
                var user = await _userManager.FindByEmailAsync(email);
                if (decodedToken.Issuer == user.Issuer) return user;
            }
            else
            {
                var user = await _userManager.FindByIdAsync(decodedToken.Uid);
                if (decodedToken.Issuer == user.Issuer) return user;
            }
            throw new Exception("User not found or token issuer mismatch");
        }

        private async Task<FirebaseToken> ValidateToken(string IdFirebaseToken)
        {
            try
            {
                var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(IdFirebaseToken);
                return decodedToken;
            }
            catch (FirebaseAuthException ex)
            {
                throw new Exception("Invalid Firebase ID token", ex);
            }
        }
    }

}

