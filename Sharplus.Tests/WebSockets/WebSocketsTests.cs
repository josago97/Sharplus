using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Xunit;
using Sharplus.WebSockets;

namespace Sharplus.Tests.WebSockets
{
    public class WebSocketsTests : IDisposable
    {
        private const int TIMEOUT = 5000; //milliseconds

        private WebSocketListener _server;
        private WebSocketClient _client;

        public WebSocketsTests()
        {
            int port = GetFreePort();
            _server = new WebSocketListener(port);
            _client = new WebSocketClient($"ws://localhost:{port}/");
        }

        private int GetFreePort()
        {
            TcpListener l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            int port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return port;
        }

        public void Dispose()
        {
            _client.Dispose();
            _server.Dispose();
        }

        [Fact]
        public async void Connect()
        {
            _server.Start();
            Thread.Sleep(1000);
            await _client.ConnectAsync(default);

            Assert.True(_client.IsConnected);
        }

        [Fact]
        public async void SendAndReceiveMessage()
        {
            string text = "Hola";
            WebSocketPlus socketListenerClient = null;

            _server.ClientConnected += c =>
            {
                socketListenerClient = new WebSocketPlus(c.WebSocket);
            };

            Connect();

            SpinWait.SpinUntil(() => socketListenerClient != null, TIMEOUT);

            await _client.SendAsync(text);
            WebSocketMessage message = await socketListenerClient.ReceiveAsync();

            Assert.Equal(text, message.ContentString);
        }
    }
}
