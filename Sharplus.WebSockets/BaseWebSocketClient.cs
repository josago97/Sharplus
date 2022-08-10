using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Sharplus.WebSockets
{
    public abstract class BaseWebSocketClient : WebSocketWrapper
    {
        public static readonly TimeSpan DEFAULT_RECONNECTION_INTERVAL = TimeSpan.FromSeconds(10);

        private Action<ClientWebSocket> _factory;

        public ClientWebSocketOptions Options => Socket.Options;
        public TimeSpan ReconnectionInterval { get; set; }
        public bool ReconnectionEnabled { get; set; }
        public bool IsConnecting { get; protected set; }
        public Uri Uri { get; private set; }
        protected new ClientWebSocket Socket
        {
            get => base.Socket as ClientWebSocket;
            set => base.Socket = value;
        }

        public BaseWebSocketClient(ClientWebSocket clientWebSocket, Action<ClientWebSocket> factory = null) : base(clientWebSocket)
        {
            _factory = factory;
            _factory?.Invoke(clientWebSocket);

            ReconnectionInterval = DEFAULT_RECONNECTION_INTERVAL;
            ReconnectionEnabled = true;
            IsConnecting = false;
        }

        public BaseWebSocketClient(Uri uri, Action<ClientWebSocket> factory = null) : this(new ClientWebSocket(), factory)
        {
            Uri = uri;
        }

        public BaseWebSocketClient(string url, Action<ClientWebSocket> factory = null) : this(new Uri(url), factory) { }

        public abstract Task ConnectAsync(CancellationToken cancellation = default);

        protected void CreateNewSocket()
        {
            ClientWebSocket newSocket = new ClientWebSocket();
            newSocket.Options.ClientCertificates = Options.ClientCertificates;
            newSocket.Options.Cookies = Options.Cookies;
            newSocket.Options.Credentials = Options.Credentials;
            newSocket.Options.KeepAliveInterval = Options.KeepAliveInterval;
            newSocket.Options.Proxy = Options.Proxy;
            newSocket.Options.RemoteCertificateValidationCallback = Options.RemoteCertificateValidationCallback;
            newSocket.Options.UseDefaultCredentials = Options.UseDefaultCredentials;
            _factory?.Invoke(newSocket);
            Socket = newSocket;
        }


        /*
        public static readonly TimeSpan DEFAULT_RECONNECTION_INTERVAL = TimeSpan.FromSeconds(5);

        private Action<ClientWebSocket> _factory;
        private CancellationTokenSource _processCancellationTokenSource;

        public ClientWebSocketOptions Options => Socket.Options;
        public TimeSpan ReconnectionInterval { get; set; }
        public bool ReconnectionEnabled { get; set; }
        public bool IsConnecting { get; private set; }
        public Uri Uri { get; private set; }
        protected new ClientWebSocket Socket
        {
            get => base.Socket as ClientWebSocket;
            set => base.Socket = value;
        }

        public event Action Connected;
        public event Action<WebSocketReceiveMessage> MessageReceived;
        public event Action<WebSocketCloseStatus, string, Exception> Disconnected;
        public event Action<Exception> Error;

        public WebSocketClient2(ClientWebSocket clientWebSocket, Action<ClientWebSocket> factory = null) : base(clientWebSocket)
        {
            _factory = factory;
            _factory?.Invoke(clientWebSocket);

            ReconnectionInterval = DEFAULT_RECONNECTION_INTERVAL;
            ReconnectionEnabled = true;
            IsConnecting = false;
        }

        public WebSocketClient2(Uri uri, Action<ClientWebSocket> factory = null) : this(new ClientWebSocket(), factory)
        {
            Uri = uri;
        }

        public WebSocketClient2(string url, Action<ClientWebSocket> factory = null) : this(new Uri(url), factory) { }

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
        }

        public override void Abort()
        {
            _processCancellationTokenSource?.Cancel();
            base.Abort();
        }

        public override async Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken = default)
        {
            _processCancellationTokenSource?.Cancel();
            await base.CloseAsync(closeStatus, statusDescription, cancellationToken);
        }

        public override async Task CloseOutputAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken = default)
        {
            _processCancellationTokenSource?.Cancel();
            await base.CloseOutputAsync(closeStatus, statusDescription, cancellationToken);
        }

        public override void Dispose()
        {
            _processCancellationTokenSource?.Cancel();
            base.Dispose();
        }

        private async Task ConnectAsync(CancellationToken cancellation)
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
        */

    }
}
