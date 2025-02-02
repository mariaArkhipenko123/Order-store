using Lab.Application.Models.DTOs.WebSocket;
using Lab.Application.Services;

namespace Lab.Application.Interfaces.Logical
{
    public interface IUserAttributesService
    {
        event EventHandler<UserStatusUpdatedEventArgs>? UserStatusUpdated;

        Task UpdateStatusAsync(UserStatusDTO userStatus);
    }
}