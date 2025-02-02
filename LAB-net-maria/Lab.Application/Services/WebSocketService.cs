using Lab.Application.Extensions;
using Lab.Application.Interfaces.Auth;
using Lab.Domain.Entities;
using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace Lab.Application.Services
{
    public class WebSocketService : IWebSocketService
    {
        private ConcurrentDictionary<AppNoificationEvents, List<WebSocket>> subscribers = new();
        public void AddSubscriber(AppNoificationEvents e, WebSocket webSocket)
        {
            if (subscribers.TryGetValue(e, out List<WebSocket>? subs))
                subs.Add(webSocket);
            else
                subscribers.TryAdd(e, new() { webSocket });
        }
        public void RemoveSubscriber(AppNoificationEvents e, WebSocket webSocket)
        {
            if (!subscribers.TryGetValue(e, out List<WebSocket>? subs))
                return;
            if (subs.Count > 0)
                subs.Remove(webSocket);
            else
                subscribers.TryRemove(e, out _);
        }
        public async Task NotifySubscribers<T>(AppNoificationEvents e, T args)
        {
            foreach (var subSocket in subscribers[e])
            {
                if (subSocket.State == WebSocketState.Closed)
                    RemoveSubscriber(e, subSocket);
                else
                    await subSocket.SendJsonAsync<T>(args);
            }
        }
    }
}
