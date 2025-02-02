using Lab.Application.Interfaces.Logical;
using Lab.Application.Models.DTOs.WebSocket;
using Lab.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Lab.Application.Services
{
    public class UserAttributesService : IUserAttributesService
    {
        private readonly UserManager<User> _userManager;
        public event EventHandler<UserStatusUpdatedEventArgs>? UserStatusUpdated;
        public UserAttributesService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task UpdateStatusAsync(UserStatusDTO userStatus)
        {
            var user = await _userManager.FindByEmailAsync(userStatus.Email);
            if (user is null)
                throw new ArgumentNullException(nameof(user), "User not found!");
            user.Status = userStatus.Status;
            await _userManager.UpdateAsync(user);
            OnUserStatusUpdated(new UserStatusUpdatedEventArgs(userStatus.Email, userStatus.Status));
        }
        protected virtual void OnUserStatusUpdated(UserStatusUpdatedEventArgs e)
        {
            UserStatusUpdated?.Invoke(this, e);
        }
    }
    public class UserStatusUpdatedEventArgs : EventArgs
    {
        public string UserEmail { get; }
        public UserStatus NewStatus { get; }
        public UserStatusUpdatedEventArgs(string userEmail, UserStatus newStatus)
        {
            UserEmail = userEmail;
            NewStatus = newStatus;

        }
    }
}
