using System;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Sharplus.WebSockets
{
    public class WebSocketListener : IDisposable
    {
        private HttpListener _httpListener;

        public bool IsListening => _httpListener.IsListening;

        public WebSocketListener(string url)
        {
            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add(url);
        }

        public WebSocketListener(int port, string networkInterface = "*", bool isSecure = false)
            : this($"{(isSecure ? "https" : "http")}://{networkInterface}:{port}/") { }

        public void Start()
        {
            _httpListener.Start();
        }

        public void Stop()
        {
            _httpListener.Stop();
        }

        public async Task<HttpListenerContext> GetHttpListenerContextAsync()
        {
            return await _httpListener.GetContextAsync();
        }

        public async Task<WebSocketContext> GetWebSocketContextAsync(string subProtocol = null, CancellationToken cancellation = default)
        {
            WebSocketContext webSocketContext = null;

            while (webSocketContext == null && !cancellation.IsCancellationRequested)
            {
                HttpListenerContext context = await GetHttpListenerContextAsync();

                if (context.Request.IsWebSocketRequest)
                {
                    webSocketContext = await context.AcceptWebSocketAsync(subProtocol);
                }
            }

            return webSocketContext;
        }

        public void Dispose()
        {
            _httpListener.Close();
        }
    }
}
