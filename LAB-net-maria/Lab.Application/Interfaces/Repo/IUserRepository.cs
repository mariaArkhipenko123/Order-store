using Lab.Application.Models.DTOs;
using Lab.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Lab.Application.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task DeleteAsync(Guid userId);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByIdAsync(Guid userId);
        Task UpdateAsync(User user);
    }
}