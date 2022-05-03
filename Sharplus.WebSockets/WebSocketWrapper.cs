using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Sharplus.WebSockets
{
    public abstract class WebSocketWrapper : WebSocket
    {
        public override WebSocketCloseStatus? CloseStatus => Socket.CloseStatus;
        public override string CloseStatusDescription => Socket.CloseStatusDescription;
        public override WebSocketState State => Socket.State;
        public override string SubProtocol => Socket.SubProtocol;
        public bool IsConnected => Socket.State == WebSocketState.Open;

        protected WebSocket Socket { get; set; }

        protected WebSocketWrapper(WebSocket webSocket)
        {
            Socket = webSocket;
        }

        public override void Abort()
        {
            Socket.Abort();
        }

        public override async Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken = default)
        {
            await Socket.CloseAsync(closeStatus, statusDescription, cancellationToken);
        }

        public override async Task CloseOutputAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken = default)
        {
            await Socket.CloseOutputAsync(closeStatus, statusDescription, cancellationToken);
        }

        public override void Dispose()
        {
            Socket.Dispose();
        }

        public override async Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken = default)
        {
            WebSocketReceiveResult receiveResult = await Socket.ReceiveAsync(buffer, cancellationToken);
            
            if (receiveResult.CloseStatus.HasValue)
                await CloseOutputAsync(receiveResult.CloseStatus.Value, receiveResult.CloseStatusDescription, cancellationToken);

            return receiveResult;
        }

        public override async Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken = default)
        {
            await Socket.SendAsync(buffer, messageType, endOfMessage, cancellationToken);
        }
    }
}
