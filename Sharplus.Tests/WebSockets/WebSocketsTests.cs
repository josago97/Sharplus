using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Sharplus.WebSockets;
using Xunit;

namespace Sharplus.Tests.WebSockets
{
    public class WebSocketsTests : IDisposable
    {
        private static int Port = 12345;

        private WebSocketListener _server;
        private WebSocketClient _client;


        public WebSocketsTests()
        {
            _server = new WebSocketListener(12345);
            _client = new WebSocketClient($"ws://localhost:{Port}/");

            Port++;
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

            SpinWait.SpinUntil(() => socketListenerClient != null);

            await _client.SendAsync(text);
            var message = await socketListenerClient.ReceiveAsync();

            Assert.Equal(text, message.ContentString);
        }
    }
}
