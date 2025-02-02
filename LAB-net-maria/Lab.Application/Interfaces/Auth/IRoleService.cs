using Microsoft.AspNetCore.Identity;

namespace Lab.Application.Interfaces.Auth
{
    public interface IRoleService
    {
        Task<IdentityResult> AddRoleToUserAsync(string email, string role);
        Task<(IdentityResult Result, IList<string>? roles)> GetUserRolesAsync(string email);
        Task<IdentityResult> RemoveRoleFromUserAsync(string email, string role);
    }
}