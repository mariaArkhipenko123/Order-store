using Lab.Application.Models.DTOs;
using Lab.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Lab.Application.Interfaces.Auth;

namespace Lab.Infrastructure.Providers
{
    internal class JwtProvider : IJwtProvider
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        private readonly double _expiresInMinutes;

        public JwtProvider(IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key is null")));
            _expiresInMinutes = _config.GetValue<double>("Jwt:ExpiresInMinutes");
        }

        public string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email)
            };

            return CreateToken(user, claims);
        }

        public string CreateToken(User user, IEnumerable<Claim> claims)
        {
            ArgumentNullException.ThrowIfNull(user.Email, nameof(user.Email));

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_expiresInMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string CreateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
