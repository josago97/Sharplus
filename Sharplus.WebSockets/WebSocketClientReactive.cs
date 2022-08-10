using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Sharplus.WebSockets
{
    public class WebSocketClientReactive : BaseWebSocketClient
    {
        private CancellationTokenSource _processCancellationTokenSource;

        public event Action Connected;
        public event Action<WebSocketReceiveMessage> MessageReceived;
        public event Action<WebSocketCloseStatus, string, Exception> Disconnected;
        public event Action<Exception> Error;

        public WebSocketClientReactive(ClientWebSocket clientWebSocket, Action<ClientWebSocket> factory = null) : base(clientWebSocket, factory)
        {
        }

        public WebSocketClientReactive(Uri uri, Action<ClientWebSocket> factory = null) : base(uri, factory)
        {
        }

        public WebSocketClientReactive(string url, Action<ClientWebSocket> factory = null) : base(url, factory)
        {
        }

        public override async Task ConnectAsync(CancellationToken cancellation)
        {
            IsConnecting = true;

            while (!IsConnected && ReconnectionEnabled && !cancellation.IsCancellationRequested)
            {
                try
                {
                    await Socket.ConnectAsync(Uri, cancellation);
                    Connected?.Invoke();
                }
                catch (Exception e)
                {
                    Error?.Invoke(e);

                    if (ReconnectionEnabled && !cancellation.IsCancellationRequested)
                    {
                        CreateNewSocket();
                        await Task.Delay(ReconnectionInterval, cancellation);
                    }
                }
            }

            IsConnecting = false;

            if (IsConnected && !cancellation.IsCancellationRequested) Run();
        }

        private async void Run()
        {
            _processCancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellation = _processCancellationTokenSource.Token;

            while (IsConnected && !cancellation.IsCancellationRequested)
            {
                WebSocketReceiveMessage message = await ReceiveMessageAsync(cancellation);
                if (message != null && !cancellation.IsCancellationRequested)
                    MessageReceived?.Invoke(message);
            }

            if (ReconnectionEnabled && !cancellation.IsCancellationRequested)
                await ConnectAsync(cancellation);
        }
        /*
        public async Task RunAsync()
        {
            if (_processCancellationTokenSource != null)
                throw new Exception("There is already a process running");

            _processCancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellation = _processCancellationTokenSource.Token;

            do
            {
                await ConnectAsync(cancellation);

                while (IsConnected && !cancellation.IsCancellationRequested)
                {
                    WebSocketReceiveMessage message = await ReceiveMessageAsync(cancellation);
                    if (message != null && !cancellation.IsCancellationRequested)
                        MessageReceived?.Invoke(message);
                }
            }
            while (ReconnectionEnabled && !cancellation.IsCancellationRequested);

            _processCancellationTokenSource = null;
        }*/

        public override void Abort()
        {
            _processCancellationTokenSource?.Cancel();
            base.Abort();
        }

        public override async Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken = default)
        {
            await base.CloseAsync(closeStatus, statusDescription, cancellationToken);
            _processCancellationTokenSource?.Cancel();
        }

        public override async Task CloseOutputAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken = default)
        {
            await base.CloseOutputAsync(closeStatus, statusDescription, cancellationToken);
            _processCancellationTokenSource?.Cancel();
        }

        public override void Dispose()
        {
            _processCancellationTokenSource?.Cancel();
            base.Dispose();
        }

        private async Task<WebSocketReceiveMessage> ReceiveMessageAsync(CancellationToken cancellation)
        {
            WebSocketReceiveMessage message = null;

            try
            {
                message = await this.ReceiveAsync(cancellation: cancellation);
            }
            catch (Exception e)
            {
                Error?.Invoke(e);
                Disconnected?.Invoke(WebSocketCloseStatus.Empty, string.Empty, e);
            }

            return message;
        }
    }
}
