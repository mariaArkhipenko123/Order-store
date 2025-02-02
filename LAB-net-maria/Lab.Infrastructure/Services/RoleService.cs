
using Lab.Application.Interfaces.Auth;
using Lab.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;

namespace Lab.Infrastructure.Services
{
    public class RoleService : IRoleService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IJwtProvider _jwtProvider;
        private readonly IDistributedCache _cache;
        public RoleService(UserManager<User> userManager, RoleManager<Role> roleManager, IJwtProvider jwtProvider, IDistributedCache cache)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtProvider = jwtProvider;
            _cache = cache;
        }

        public async Task<IdentityResult> AddRoleToUserAsync(string email, string role)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });

            if (!await _roleManager.RoleExistsAsync(role))
                return IdentityResult.Failed(new IdentityError { Description = "Role not found." });

            return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<(IdentityResult Result, IList<string>? roles)> GetUserRolesAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return (IdentityResult.Failed(new IdentityError { Description = "User not found." }), null);

            var roles = await _userManager.GetRolesAsync(user);
            return (IdentityResult.Success, roles);
        }

        public async Task<IdentityResult> RemoveRoleFromUserAsync(string email, string role)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });

            if (!await _roleManager.RoleExistsAsync(role))
                return IdentityResult.Failed(new IdentityError { Description = "Role not found." });

            return await _userManager.RemoveFromRoleAsync(user, role);
        }
    }
}
