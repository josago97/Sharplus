using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using Sharplus.WebSockets;
using Xunit;

namespace Sharplus.Tests.WebSockets
{
    public abstract class BaseWebSocketClientTests<T> : IDisposable where T : BaseWebSocketClient
    {
        protected T client;
        protected WebSocketListener listener;
        
        public BaseWebSocketClientTests()
        {
            int port = GetFreePort();
            listener = new WebSocketListener(port, "localhost");
            client = CreateWebSocketClient($"ws://localhost:{port}");
        }

        protected abstract T CreateWebSocketClient(string url);

        private int GetFreePort()
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Loopback, 0);
            tcpListener.Start();
            int port = ((IPEndPoint)tcpListener.LocalEndpoint).Port;
            tcpListener.Stop();

            return port;
        }

        public void Dispose()
        {
            listener.Dispose();
        }

        protected async Task<WebSocket> ConnectAsync()
        {
            WebSocket result;

            listener.Start();
            Task clientConnectTask = client.ConnectAsync();
            result = (await listener.GetWebSocketContextAsync()).WebSocket;
            await clientConnectTask;

            return result;
        }

        protected async Task DisconnectAsync()
        {
            await client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, string.Empty);
            listener.Stop();
        }

        protected async Task SendAndReceiveMessageAsync(WebSocket sender, WebSocket receiver)
        {
            using Stream stream = new MemoryStream();

            object objectMessage = new { Variable = 123, Variable2 = "Hello" };
            await sender.SendAsync(objectMessage);
            WebSocketReceiveMessage receiveMessage = await receiver.ReceiveAsync();
            Assert.Equal(objectMessage, receiveMessage.Parse(objectMessage.GetType()));

            string stringMessage = "Hello world";
            await sender.SendAsync(stringMessage);
            receiveMessage = await receiver.ReceiveAsync();
            Assert.Equal(stringMessage, receiveMessage.GetContentString());

            byte[] bytesMessage = new byte[] { 0, 1, 2 };
            await sender.SendAsync(bytesMessage);
            receiveMessage = await receiver.ReceiveAsync();
            Assert.Equal(bytesMessage, receiveMessage.Content);
        }


        /*


        [Fact]
        public async void ConnectAndDisconnect()
        {
            await ConnectAsync();

            Assert.True(_listener.IsListening);
            Assert.True(_client.IsConnected);

            await DisconnectAsync();

            Assert.False(_listener.IsListening);
            Assert.False(_client.IsConnected);
        }

        [Fact]
        public async void SendAndReceiveMessage()
        {
            WebSocket listenerWebSocket = await ConnectAsync();
            using Stream stream = new MemoryStream();

            object objectMessage = new { Variable = 123, Variable2 = "Hello" };
            await _client.SendAsync(objectMessage);
            WebSocketReceiveMessage receiveMessage = await listenerWebSocket.ReceiveAsync();
            Assert.Equal(objectMessage, receiveMessage.Parse(objectMessage.GetType()));

            string stringMessage = "Hello world";
            await _client.SendAsync(stringMessage);
            receiveMessage = await listenerWebSocket.ReceiveAsync();
            Assert.Equal(stringMessage, receiveMessage.GetContentString());

            byte[] bytesMessage = new byte[] { 0, 1, 2 };
            await _client.SendAsync(bytesMessage);
            receiveMessage = await listenerWebSocket.ReceiveAsync();
            Assert.Equal(bytesMessage, receiveMessage.Content);

            await DisconnectAsync();
        }

        [Fact]
        public async void Reconnect()
        {
            _client.ReconnectionEnabled = true;
            WebSocket webSocket = await ConnectAsync();

            Assert.True(_client.IsConnected);
            Assert.False(_client.IsConnecting);

            _listener.Stop();
            await Assert.ThrowsAsync<WebSocketException>(() => _client.ReceiveAsync());
            Assert.False(_client.IsConnected);
            Assert.True(_client.IsConnecting);

            _listener.Start();
            webSocket = (await _listener.GetWebSocketContextAsync()).WebSocket;
            Assert.True(_client.IsConnected);
            Assert.False(_client.IsConnecting);

            await DisconnectAsync();

            Assert.False(_client.IsConnected);
            Assert.False(_client.IsConnecting);
        }

        [Fact]
        public async void WebSocketServerDisconnection()
        {
            bool disconnected = false;
            bool messageReceived = false;
            WebSocketServer webSocketServer = new WebSocketServer(await ConnectAsync());

            webSocketServer.Disconnected += (_, __, ___, ____) => disconnected = true;
            webSocketServer.MessageReceived += (_, __) => messageReceived = true;
            webSocketServer.Start();

            await _client.SendAsync(string.Empty);

            SpinWait.SpinUntil(() => messageReceived);
            Assert.False(disconnected);

            messageReceived = false;
            await DisconnectAsync();
            SpinWait.SpinUntil(() => messageReceived);
            SpinWait.SpinUntil(() => disconnected);
            Assert.True(disconnected);
        }

        [Fact]
        public async void WebSocketServerStop()
        {
            bool disconnected = false;
            bool messageReceived = false;
            WebSocketServer webSocketServer = new WebSocketServer(await ConnectAsync());

            webSocketServer.Disconnected += (_, __, ___, ____) => disconnected = true;
            webSocketServer.MessageReceived += (_, __) => messageReceived = true;
            webSocketServer.Start();

            await _client.SendAsync(string.Empty);

            SpinWait.SpinUntil(() => messageReceived);
            Assert.True(messageReceived);
            Assert.False(disconnected);

            webSocketServer.Stop();
            messageReceived = false;
            await Assert.ThrowsAsync<IOException>(() => _client.SendAsync(string.Empty));
            await Task.Delay(1000);
            Assert.False(messageReceived);
            SpinWait.SpinUntil(() => disconnected);
            Assert.True(disconnected);

            _listener.Stop();
        }

        public void Dispose()
        {
            _listener.Dispose();
        }

        private async Task<WebSocket> ConnectAsync()
        {
            WebSocket result;

            _listener.Start();
            Task clientConnectTask = _client.ConnectAsync();
            result = (await _listener.GetWebSocketContextAsync()).WebSocket;
            await clientConnectTask;

            return result;
        }

        private async Task DisconnectAsync()
        {
            await _client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, string.Empty);
            _listener.Stop();
        }*/
    }
}
