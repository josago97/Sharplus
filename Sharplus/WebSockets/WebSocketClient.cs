using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sharplus.WebSockets
{
    public class WebSocketClient : WebSocketHandler
    {
        public static readonly TimeSpan DEFAULT_RECONNECTION_INTERVAL = TimeSpan.FromSeconds(10);

        private CancellationToken _connectCancellationToken;

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

        public WebSocketClient(Uri uri) : base(new ClientWebSocket())
        {
            Uri = uri;

            Disconnected += OnDisconnected;
        }

        public WebSocketClient(string url) : this(new Uri(url))
        {
        }

        public WebSocketClient(WebSocket webSocket) : base(webSocket)
        {
        }

        public async Task ConnectAsync(CancellationToken cancellationToken)
        {
            IsConnecting = true;

            do
            {
                try
                {
                    await Socket.ConnectAsync(Uri, cancellationToken);
                    //SpinWait.SpinUntil(() => IsConnected || !IsActive, Options.KeepAliveInterval);
                }
                catch (Exception ex)
                {
                    CreateNewSocket();
                }
            }
            while (ReconnectionEnabled && !IsConnected);

            if (IsConnected)
            {
                Connected?.Invoke();
                StartListen();
            }

            IsConnecting = false;
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

        private void OnDisconnected()
        {
            StopListen();
            if (ReconnectionEnabled) Task.Run(() => ConnectAsync(default));
        }







        /*private CancellationTokenSource _connectTokenSource;

        public event Action Connected;
        public event Action<WebSocketMessage> MessageReceived;

        public ClientWebSocket ClientSocket
        {
            get => (ClientWebSocket)Socket;
            set => Socket = value;
        }
        public Uri Uri { get; private set; }
        public TimeSpan ReconnectionInterval { get; set; }
        public TimeSpan KeepAliveInterval
        {
            get => ClientSocket.Options.KeepAliveInterval;
            set => ClientSocket.Options.KeepAliveInterval = value;
        }
        public bool IsActive { get; private set; }

        public WebSocketClient(string completeUrl) : base(new ClientWebSocket())
        {
            Uri = new Uri(completeUrl);

            ReconnectionInterval = KeepAliveInterval;

            Disconnected += OnDisconnected;
        }

        public async Task StartAsync()
        {
            if (!IsActive)
            {
                IsActive = true;
                _connectTokenSource = new CancellationTokenSource();
                await ConnectAsync();
            }
        }

        public async Task StopAsync()
        {
            IsActive = false;
            _connectTokenSource?.Cancel();

            if (ClientSocket.State == WebSocketState.Open)
            {
                await ClientSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
            }
        }

        private async Task ConnectAsync()
        {
            while (IsActive && !IsConnected)
            {
                try
                {
                    await ClientSocket.ConnectAsync(Uri, _connectTokenSource.Token);
                    SpinWait.SpinUntil(() => IsConnected || !IsActive, KeepAliveInterval);
                }
                catch (Exception ex)
                {
                    CreateNewSocket();
                }
            }

            if (IsConnected)
            {
                Connected?.Invoke();
                Listen();
            }
        }

        private void CreateNewSocket()
        {
            ClientWebSocket newSocket = new ClientWebSocket();
            newSocket.Options.KeepAliveInterval = KeepAliveInterval;

            ClientSocket = newSocket;
        }

        private void OnDisconnected()
        {
            IsActive = false;
        }

        private async void Listen()
        {
            while (IsConnected)
            {
                WebSocketMessage message = await ReceiveAsync();

                MessageReceived?.Invoke(message);
            }
        }*/
        /*public override WebSocketCloseStatus? CloseStatus => throw new NotImplementedException();

        public override string CloseStatusDescription => throw new NotImplementedException();

        public override WebSocketState State => throw new NotImplementedException();

        public override string SubProtocol => throw new NotImplementedException();

        public override void Abort()
        {
            throw new NotImplementedException();
        }

        public override Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task CloseOutputAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }

        public override Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }*/
    }
}
