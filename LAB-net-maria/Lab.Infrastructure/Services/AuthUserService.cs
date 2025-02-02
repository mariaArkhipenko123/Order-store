using AutoMapper;
using Lab.Application.Interfaces;
using Lab.Application.Interfaces.Auth;
using Lab.Application.Interfaces.Mongo;
using Lab.Application.Models.DTOs.Secure;
using Lab.Domain.Entities;
using Lab.Domain.MongoEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Lab.Infrastructure.Services
{
    public class AuthUserService : IAuthUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtProvider _jwtProvider;
        private readonly IDistributedCache _cache;
        private readonly IGoogleAuthService _googleAuthService;
        private readonly IAuditLogHandler _auditLogHandler;
        private readonly IMetaDataHandler _metaDataHandler;
        private readonly IUserAccessRepository _userAccessRepository;
        private readonly IFirebaseService _firebaseService;
        public AuthUserService(UserManager<User> userManager, IJwtProvider jwtProvider, IDistributedCache cache,
            IAuditLogHandler auditLogHandler, IGoogleAuthService googleAuthService, IMetaDataHandler metaDataHandler, IUserAccessRepository userAccessRepository,
            IFirebaseService firebaseService)
        {
            _userManager = userManager;
            _jwtProvider = jwtProvider;
            _cache = cache;
            _auditLogHandler = auditLogHandler;
            _googleAuthService = googleAuthService;
            _metaDataHandler = metaDataHandler;
            _userAccessRepository = userAccessRepository;
            _firebaseService = firebaseService;
        }
        public async Task<IdentityResult> RegisterAsync(АuthorDTO authorDTO)
        {
            var user = new User()
            {
                Email = authorDTO.Email,
                UserName = authorDTO.Email
            };
            var result = await _userManager.CreateAsync(user, authorDTO.Password);

            if (result.Succeeded)
            {
                var UserId = user.Id;

               var firebaseUserId = await _firebaseService.RegisterUserAsync(UserId.ToString(), authorDTO.Email, authorDTO.Password);
                if (firebaseUserId == null)
                {
                    await _userManager.DeleteAsync(user);
                    return IdentityResult.Failed(new IdentityError { Description = "Failed to register user in Firebase." });
                }
                var userAccess = new UserAccess
                {
                    DeviceId = 1,
                    UserId = user.Id,
                    DeviceName = "test",
                    IP = "testIP",
                    Agent = "testAgent"
                };
                try
                {
                    await _userAccessRepository.AddAsync(userAccess);
                    await _auditLogHandler.CreateAuditLog(new AuditLog { UserId = user.Id, Action = nameof(AuditLogAction.Register) });
                    await _metaDataHandler.CreateMetaData(new MetaData { UserAccessId = userAccess.Id });
                }
                catch (Exception ex)
                {
                    await _userManager.DeleteAsync(user);
                    throw new Exception(ex.Message);
                };
                return result;
            }
            return IdentityResult.Failed(new IdentityError { Description = "Registration failed." });
        }
        public async Task<(IdentityResult Result, string? Token, string? RefreshToken)> LoginAsync(АuthorDTO authorDTO)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(authorDTO.Email);
                if (user == null || !await _userManager.CheckPasswordAsync(user, authorDTO.Password))
                {
                    return (IdentityResult.Failed(new IdentityError { Description = "Invalid email or password." }), null, null);
                }

                var userAccess = new UserAccess
                {
                    DeviceId = 1,
                    UserId = user.Id,
                    DeviceName = "test",
                    IP = "testIP",
                    Agent = "testAgent"
                };
                await _userAccessRepository.AddAsync(userAccess);
                await _auditLogHandler.CreateAuditLog(new AuditLog { UserId = user.Id, Action = nameof(AuditLogAction.Login) });
                await _metaDataHandler.CreateMetaData(new MetaData { UserAccessId = userAccess.Id });

                var claims = await _userManager.GetClaimsAsync(user);
                string newToken = _jwtProvider.CreateToken(user, claims);
                var refreshToken = new TokenDto() { Token = _jwtProvider.CreateRefreshToken() };

                await _cache.SetAsync(user.Id.ToString(), System.Text.Encoding.UTF8.GetBytes(refreshToken.Token));

                return (IdentityResult.Success, newToken, refreshToken.Token);
            }
            catch (Exception ex)
            {
                return (IdentityResult.Failed(new IdentityError { Description = "An error occurred during login." }), null, null);

            }
        }
        public async Task<(IdentityResult Result, string? Token, string? RefreshToken)> LoginWithGoogle(GoogleLoginDTO googleLoginParams)
        {
            try
            {
                var user = await _googleAuthService.AuthenticateWithGoogleAsync(googleLoginParams.IdToken);

                if (user != null)
                {
                    var userAccess = new UserAccess
                    {
                        DeviceId = 1,
                        UserId = user.Id,
                        DeviceName = "test",
                        IP = "testIP",
                        Agent = "testAgent"
                    };
                    await _userAccessRepository.AddAsync(userAccess);

                    await _auditLogHandler.CreateAuditLog(new AuditLog { UserId = user.Id, Action = nameof(AuditLogAction.LoginWithOAuth) });

                    await _metaDataHandler.CreateMetaData(new MetaData { UserAccessId = userAccess.Id });

                    var claims = await _userManager.GetClaimsAsync(user);

                    string newToken = _jwtProvider.CreateToken(user, claims);

                    var refreshToken = new TokenDto() { Token = _jwtProvider.CreateRefreshToken() };

                    await _cache.SetAsync(user.Id.ToString(), System.Text.Encoding.UTF8.GetBytes(refreshToken.Token));

                    return (IdentityResult.Success, newToken, refreshToken.Token);
                }

                var errors = new[] { new IdentityError { Description = "Failed to authenticate user." } };
                return (IdentityResult.Failed(errors), null, null);
            }
            catch (Exception ex)
            {
                return (IdentityResult.Failed(new IdentityError { Description = "An error occurred during Google login." }), null, null);
            }
        }

        public async Task<(IdentityResult Result, string? Token, string? RefreshToken)> LoginWithFirebase(FirebaseLoginDTO firebaseLoginParams)
        {
            try
            {
                var user = await _firebaseService.AuthenticateWithFirebase(firebaseLoginParams.IdFirebaseToken);
                if (user != null)
                {
                    var userAccess = new UserAccess
                    {
                        DeviceId = 1,
                        UserId = user.Id,
                        DeviceName = "test",
                        IP = "testIP",
                        Agent = "testAgent"
                    };
                    await _userAccessRepository.AddAsync(userAccess);
                    await _auditLogHandler.CreateAuditLog(new AuditLog { UserId = user.Id, Action = nameof(AuditLogAction.LoginWithFirebase) });
                    await _metaDataHandler.CreateMetaData(new MetaData { UserAccessId = userAccess.Id });

                    var claims = await _userManager.GetClaimsAsync(user);
                    string newToken = _jwtProvider.CreateToken(user, claims);
                    var refreshToken = new TokenDto() { Token = _jwtProvider.CreateRefreshToken() };

                    await _cache.SetAsync(user.Id.ToString(), System.Text.Encoding.UTF8.GetBytes(refreshToken.Token));

                    return (IdentityResult.Success, newToken, refreshToken.Token);
                }

                return (IdentityResult.Failed(new IdentityError { Description = "Failed to authenticate user." }), null, null);
            }
            catch (Exception ex)
            {
                return (IdentityResult.Failed(new IdentityError { Description = "An error occurred during Firebase login." }), null, null);
            }
        }

        public async Task<(IdentityResult Result, string? Token, string? RefreshToken)> RefreshTokenAsync(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                    return (IdentityResult.Failed(new IdentityError { Description = "User  not found." }), null, null);

                var refreshTokenBytes = await _cache.GetAsync(user.Id.ToString());
                if (refreshTokenBytes == null)
                    return (IdentityResult.Failed(new IdentityError { Description = "Refresh token not found." }), null, null);

                var refreshTokenString = System.Text.Encoding.UTF8.GetString(refreshTokenBytes);
                var refreshToken = JsonSerializer.Deserialize<TokenDto>(refreshTokenString);

                if (refreshToken == null || refreshToken.ExpirationDate < DateTime.UtcNow)
                    return (IdentityResult.Failed(new IdentityError { Description = "Invalid or expired refresh token." }), null, null);

                var claims = await _userManager.GetClaimsAsync(user);
                string newToken = _jwtProvider.CreateToken(user, claims);
                var newRefreshToken = new TokenDto() { Token = _jwtProvider.CreateRefreshToken() };

                await _cache.SetAsync(user.Id.ToString(), System.Text.Encoding.UTF8.GetBytes(newRefreshToken.Token));

                return (IdentityResult.Success, newToken, newRefreshToken.Token);
            }
            catch (Exception ex)
            {
                return (IdentityResult.Failed(new IdentityError { Description = "An error occurred during token refresh." }), null, null);
            }
        }
    }
}
