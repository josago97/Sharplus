using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Sharplus.WebSockets
{
    public class WebSocketClient : WebSocketWrapper
    {
        public static readonly TimeSpan DEFAULT_RECONNECTION_INTERVAL = TimeSpan.FromSeconds(10);

        private Task _connectTask;
        private CancellationTokenSource _connectCancellationSource;

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

        public WebSocketClient(ClientWebSocket clientWebSocket) : base(clientWebSocket)
        {
            ReconnectionInterval = DEFAULT_RECONNECTION_INTERVAL;
            ReconnectionEnabled = true;
            IsConnecting = false;
        }

        public WebSocketClient(string url) : this(new ClientWebSocket())
        {
            Uri = new Uri(url);
        }

        public async Task ConnectAsync(CancellationToken cancellation = default)
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
                            if (ReconnectionEnabled) CreateNewSocket();
                            else throw;
                        }
                    }
                    while (!IsConnected && ReconnectionEnabled && !_connectCancellationSource.IsCancellationRequested);

                    IsConnecting = false;
                });

                await _connectTask;
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

        private void CreateNewSocket()
        {
            ClientWebSocket newSocket = new ClientWebSocket();
            newSocket.Options.ClientCertificates = Options.ClientCertificates;
            newSocket.Options.Cookies = Options.Cookies;
            newSocket.Options.Credentials = Options.Credentials;
            newSocket.Options.KeepAliveInterval = Options.KeepAliveInterval;
            newSocket.Options.Proxy = Options.Proxy;
            newSocket.Options.RemoteCertificateValidationCallback = Options.RemoteCertificateValidationCallback;
            newSocket.Options.UseDefaultCredentials = Options.UseDefaultCredentials;

            Socket = newSocket;
        }
    }
}
