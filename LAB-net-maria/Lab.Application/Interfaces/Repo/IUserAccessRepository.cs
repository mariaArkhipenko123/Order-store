using Lab.Domain.Entities;

namespace Lab.Application.Interfaces
{
    public interface IUserAccessRepository
    {
        Task AddAsync(UserAccess userAccess);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<UserAccess>> GetAllAsync();
        Task<UserAccess> GetByIdAsync(Guid id);
        Task UpdateAsync(UserAccess userAccess);
    }
}