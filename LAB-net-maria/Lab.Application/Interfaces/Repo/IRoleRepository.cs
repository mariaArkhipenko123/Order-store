using Lab.Domain.Entities;

namespace Lab.Application.Interfaces
{
    public interface IRoleRepository
    {
        Task AddAsync(Role role);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<Role>> GetAllAsync();
        Task<Role> GetByIdAsync(Guid id);
        Task UpdateAsync(Role role);
    }
}