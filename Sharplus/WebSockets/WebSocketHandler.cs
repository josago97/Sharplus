using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;

namespace Sharplus.WebSockets
{
    public class WebSocketHandler : WebSocketPlus
    {
        private CancellationTokenSource _listenTokenSource;

        public event Action<WebSocketMessage> MessageReceived;

        public WebSocketHandler(WebSocket webSocket) : base(webSocket)
        {
        }

        protected void StartListen()
        {
            _listenTokenSource = new CancellationTokenSource();
            new Thread(Listen).Start();
        }

        protected void StopListen()
        {
            _listenTokenSource?.Cancel();
        }

        private async void Listen()
        {
            _listenTokenSource = new CancellationTokenSource();

            while (!_listenTokenSource.IsCancellationRequested)
            {
                WebSocketMessage message = await ReceiveAsync(_listenTokenSource.Token);

                MessageReceived?.Invoke(message);
            }
        }

        public override void Dispose()
        {
            StopListen();

            base.Dispose();
        }
    }
}
