using System;
using System.Net.WebSockets;
using System.Threading;

namespace Sharplus.WebSockets
{
    public class WebSocketServer : WebSocketWrapper
    {
        private CancellationTokenSource _listenCancellationSource;

        public event Action<WebSocket, WebSocketReceiveMessage> MessageReceived;
        public event Action<WebSocket, WebSocketCloseStatus, string, Exception> Disconnected;

        public WebSocketServer(WebSocket webSocket) : base(webSocket)
        {
        }

        public void Start()
        {
            if (_listenCancellationSource != null) return;

            _listenCancellationSource = new CancellationTokenSource();
            Listen();
        }

        public void Stop()
        {
            _listenCancellationSource?.Cancel();
        }

        private async void Listen()
        {
            while (!IsStateTerminal(State) && !_listenCancellationSource.IsCancellationRequested)
            {
                WebSocketReceiveMessage messageReceived = null;
                Exception exception = null;

                try
                {
                    messageReceived = await Socket.ReceiveAsync(cancellation: _listenCancellationSource.Token);
                }
                catch (Exception ex)
                {
                    exception = ex;
                    Disconnected?.Invoke(this, WebSocketCloseStatus.Empty, string.Empty, ex);
                }

                if (messageReceived != null)
                {
                    MessageReceived?.Invoke(this, messageReceived);

                    if (messageReceived.CloseStatus.HasValue)
                    {
                        Disconnected?.Invoke(this, messageReceived.CloseStatus.Value, messageReceived.CloseStatusDescription, exception);
                    }
                }
            }
        }
    }
}
