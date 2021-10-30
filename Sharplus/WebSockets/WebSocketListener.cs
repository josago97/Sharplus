using System;
using System.Net;
using System.Net.WebSockets;
using System.Threading;

namespace Sharplus.WebSockets
{
    public class WebSocketListener : IDisposable
    {
        private HttpListener _httpListener;

        public bool IsListening => _httpListener.IsListening;
        public string SubProtocol { get; private set; }

        public event Action<WebSocketContext> ClientConnected;

        public WebSocketListener(int port, bool isSecure = false, string subProtocol = null)
        {
            _httpListener = new HttpListener();
            SubProtocol = subProtocol;

            string protocol = isSecure ? "https" : "http";
            _httpListener.Prefixes.Add($"{protocol}://*:{port}/");
        }

        public void Start()
        {
            _httpListener.Start();

            Thread listenThread = new Thread(new ThreadStart(Listen));
            listenThread.Start();
        }

        public void Stop()
        {
            _httpListener.Close();
        }

        private async void Listen()
        {
            while (IsListening)
            {
                HttpListenerContext context = await _httpListener.GetContextAsync();

                if (context.Request.IsWebSocketRequest)
                {
                    HttpListenerWebSocketContext socketContext = await context.AcceptWebSocketAsync(SubProtocol);
                    ClientConnected?.Invoke(socketContext);
                }
            }
        }

        public void Dispose()
        {
            _httpListener.Close();
        }
    }
}
