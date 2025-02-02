using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Lab.Application.Extensions
{
    public static class WebSocketExtensions
    {
        public static async Task SendJsonAsync<T>(this WebSocket webSocket, T value)
        {
            if (webSocket.State == WebSocketState.Open)
            {
                var messageBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize<T>(value));
                var buffer = new ArraySegment<byte>(messageBytes);
                await webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
        public static async Task KeepAliveAndDiscardRecieve(this WebSocket webSocket)
        {
            while (!webSocket.CloseStatus.HasValue)
                await webSocket.ReceiveAsync(new ArraySegment<byte>(new byte[1024 * 4]), CancellationToken.None);
            await webSocket.CloseAsync(
                webSocket.CloseStatus.Value,
                webSocket.CloseStatusDescription,
                CancellationToken.None);
        }
    }
}
