using Lab.Application.Models.DTOs;
using Lab.Domain.Entities;
using System.Security.Claims;

namespace Lab.Application.Interfaces.Auth
{
    public interface IJwtProvider
    {
        string CreateToken(User user);
        string CreateToken(User user, IEnumerable<Claim> claims);
        string CreateRefreshToken();
    }
}