using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Sharplus.WebSockets
{
    public static class Extensions
    {
        private const int RECEIVE_BUFFER_SIZE = 2048; // 2^10 bytes

        public static async Task<WebSocketReceiveMessage> ReceiveAsync(this WebSocket socket, int bufferSeize = RECEIVE_BUFFER_SIZE, CancellationToken cancellation = default)
        {
            WebSocketReceiveMessage result = null;
            WebSocketReceiveResult socketReceiveResult;
            byte[] buffer = new byte[bufferSeize];
            ArraySegment<byte> bytesReceived = new ArraySegment<byte>(buffer);
            List<byte> content = new List<byte>();

            do
            {
                socketReceiveResult = await socket.ReceiveAsync(bytesReceived, cancellation);

                if (socketReceiveResult != null)
                    content.AddRange(bytesReceived.Slice(0, socketReceiveResult.Count));
            }
            while (socketReceiveResult != null && !socketReceiveResult.EndOfMessage && !cancellation.IsCancellationRequested);

            if (socketReceiveResult != null)
                result = new WebSocketReceiveMessage(socketReceiveResult, content.ToArray());

            return result;
        }

        public static async Task SendAsync(this WebSocket socket, byte[] message, CancellationToken cancellation = default)
        {
            await socket.SendAsync(message, WebSocketMessageType.Binary, cancellation);
        }

        public static async Task SendAsync(this WebSocket socket, string message, Encoding encoding = null, CancellationToken cancellation = default)
        {
            if (encoding == null) encoding = Encoding.UTF8;

            await socket.SendAsync(encoding.GetBytes(message), WebSocketMessageType.Text, cancellation);
        }

        public static async Task SendAsync(this WebSocket socket, object message, JsonSerializerOptions options = null, CancellationToken cancellation = default)
        {
            string json = JsonSerializer.Serialize(message, options);
            await socket.SendAsync(json, null, cancellation);
        }

        private static async Task SendAsync(this WebSocket socket, byte[] message, WebSocketMessageType messageType, CancellationToken cancellation = default)
        {
            ArraySegment<byte> bytesToSend = new ArraySegment<byte>(message);
            await socket.SendAsync(bytesToSend, messageType, true, cancellation);
        }
    }
}
