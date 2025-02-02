using Lab.Application.Interfaces;
using Lab.Application.Interfaces.Logical;
using Lab.Domain.Entities;


namespace Lab.Application.Services
{
    public class UserAccessService : IUserAccessService
    {
        private readonly IUserAccessRepository _userAccessRepository;

        public UserAccessService(IUserAccessRepository userAccessRepository)
        {
            _userAccessRepository = userAccessRepository;
        }

        public async Task<IEnumerable<UserAccess>> GetAllAsync()
        {
            return await _userAccessRepository.GetAllAsync();
        }

        public async Task<UserAccess> GetByIdAsync(Guid id)
        {
            return await _userAccessRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(UserAccess userAccess)
        {
            await _userAccessRepository.AddAsync(userAccess);
        }

        public async Task UpdateAsync(UserAccess userAccess)
        {
            await _userAccessRepository.UpdateAsync(userAccess);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _userAccessRepository.DeleteAsync(id);
        }
    }
}
