using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Sharplus.WebSockets
{
    public class WebSocketClient : BaseWebSocketClient
    {
        private Task _connectTask;
        private CancellationTokenSource _connectCancellationSource;

        public WebSocketClient(ClientWebSocket clientWebSocket, Action<ClientWebSocket> factory = null) : base(clientWebSocket, factory)
        {
        }

        public WebSocketClient(Uri uri, Action<ClientWebSocket> factory = null) : base(uri, factory)
        {
        }

        public WebSocketClient(string url, Action<ClientWebSocket> factory = null) : base(url, factory)
        {
        }

        public override async Task ConnectAsync(CancellationToken cancellation = default)
        {
            if (IsConnecting) await _connectTask;
            else
            {
                _connectCancellationSource = CancellationTokenSource.CreateLinkedTokenSource(cancellation);
                _connectTask = Task.Run(async () =>
                {
                    IsConnecting = true;

                    do
                    {
                        try
                        {
                            await Socket.ConnectAsync(Uri, _connectCancellationSource.Token);
                        }
                        catch
                        {
                            if (ReconnectionEnabled && !cancellation.IsCancellationRequested)
                            {
                                CreateNewSocket();
                                await Task.Delay(ReconnectionInterval, cancellation);
                            }
                            else throw;
                        }
                    }
                    while (!IsConnected && ReconnectionEnabled && !_connectCancellationSource.IsCancellationRequested);

                    IsConnecting = false;
                });

                await _connectTask;
            }
        }

        public override async Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken = default)
        {
            WebSocketReceiveResult receiveResult = null;

            try
            {
                receiveResult = await Socket.ReceiveAsync(buffer, cancellationToken);
            }
            catch (Exception ex)
            {
                if (ReconnectionEnabled) _ = ConnectAsync();
                throw;
            }

            return receiveResult;
        }

        public override async Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken = default)
        {
            try
            {
                await Socket.SendAsync(buffer, messageType, endOfMessage, cancellationToken);
            }
            catch (Exception ex)
            {
                _ = ConnectAsync();
                throw;
            }
        }

        public override void Abort()
        {
            ReconnectionEnabled = false;
            _connectCancellationSource?.Cancel();
            base.Abort();
        }

        public override async Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken = default)
        {
            ReconnectionEnabled = false;
            _connectCancellationSource?.Cancel();
            await base.CloseAsync(closeStatus, statusDescription, cancellationToken);
        }

        public override async Task CloseOutputAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken = default)
        {
            ReconnectionEnabled = false;
            _connectCancellationSource?.Cancel();
            await base.CloseOutputAsync(closeStatus, statusDescription, cancellationToken);
        }

        public override void Dispose()
        {
            ReconnectionEnabled = false;
            _connectCancellationSource?.Cancel();
            base.Dispose();
        }
    }
}
