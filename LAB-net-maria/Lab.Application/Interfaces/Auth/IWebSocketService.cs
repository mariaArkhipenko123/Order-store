using Lab.Domain.Entities;
using System.Net.WebSockets;

namespace Lab.Application.Interfaces.Auth
{
    public interface IWebSocketService
    {
        void AddSubscriber(AppNoificationEvents e, WebSocket webSocket);
        Task NotifySubscribers<T>(AppNoificationEvents e, T args);
        void RemoveSubscriber(AppNoificationEvents e, WebSocket webSocket);
    }
}