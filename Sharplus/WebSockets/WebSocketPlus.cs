using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Sharplus.WebSockets
{
    public class WebSocketPlus : WebSocket
    {
        private static int BUFFER_SIZE = (int)Math.Pow(2, 10);

        public override WebSocketCloseStatus? CloseStatus => Socket.CloseStatus;
        public override string CloseStatusDescription => Socket.CloseStatusDescription;
        public override WebSocketState State => Socket.State;
        public override string SubProtocol => Socket.SubProtocol;
        public bool IsConnected => Socket.State == WebSocketState.Open;

        protected WebSocket Socket { get; set; }

        public event Action Disconnected;
        public event Action<Exception> Error;

        public WebSocketPlus(WebSocket webSocket)
        {
            Socket = webSocket;
        }

        public override void Abort()
        {
            Socket.Abort();
        }

        public override async Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
        {
            await Socket.CloseAsync(closeStatus, statusDescription, cancellationToken);
        }

        public override async Task CloseOutputAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
        {
            await Socket.CloseOutputAsync(closeStatus, statusDescription, cancellationToken);
        }

        public override void Dispose()
        {
            Socket.Dispose();
        }

        public override async Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
        {
            return await Socket.ReceiveAsync(buffer, cancellationToken);
        }

        public async Task<WebSocketMessage> ReceiveAsync(CancellationToken cancellation = default)
        {
            WebSocketMessage result = null;

            try
            {
                if (IsConnected)
                {
                    List<byte> bytes = new List<byte>();
                    byte[] buffer = new byte[BUFFER_SIZE];
                    ArraySegment<byte> bytesReceived = new ArraySegment<byte>(buffer);
                    WebSocketReceiveResult socketResult;

                    do
                    {
                        socketResult = await ReceiveAsync(bytesReceived, cancellation);
                        bytes.AddRange(bytesReceived.Slice(0, socketResult.Count));
                    }
                    while (!socketResult.EndOfMessage);

                    result = new WebSocketMessage(bytes.ToArray());
                }
            }
            catch (Exception e)
            {
                HandleException(e);
            }

            return result;
        }

        public override async Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
        {
            await Socket.SendAsync(buffer, messageType, endOfMessage, cancellationToken);
        }

        public async Task<bool> SendAsync(object message, CancellationToken cancellation = default)
        {
            string json = JsonSerializer.Serialize(message);
            return await SendAsync(json, cancellation);
        }

        public async Task<bool> SendAsync(string message, CancellationToken cancellation = default)
        {
            return await SendAsync(Encoding.UTF8.GetBytes(message), WebSocketMessageType.Text, cancellation);
        }

        public async Task<bool> SendAsync(byte[] message, CancellationToken cancellation = default)
        {
            return await SendAsync(message, WebSocketMessageType.Binary, cancellation);
        }

        private async Task<bool> SendAsync(byte[] message, WebSocketMessageType messageType, CancellationToken cancellation = default)
        {
            bool success = false;

            try
            {
                if (IsConnected)
                {
                    ArraySegment<byte> bytesToSend = new ArraySegment<byte>(message);
                    await SendAsync(bytesToSend, messageType, true, cancellation);
                    success = true;
                }
            }
            catch (Exception e)
            {
                HandleException(e);
            }

            return success;
        }

        private void HandleException(Exception exception)
        {
            if (State == WebSocketState.Aborted)
            {
                Disconnected?.Invoke();
            }
            else
            {
                Error?.Invoke(exception);
            }
        }
    }
}
