using System.Net.Sockets;
using System.Net.WebSockets;
using Sharplus.WebSockets;
using Xunit;

namespace Sharplus.Tests.WebSockets
{
    public class WebSocketClientTests : BaseWebSocketClientTests<WebSocketClient>
    {
        protected override WebSocketClient CreateWebSocketClient(string url)
        {
            return new WebSocketClient(url);
        }

        [Fact]
        public async void ConnectAndDisconnect()
        {
            await ConnectAsync();

            Assert.True(listener.IsListening);
            Assert.True(client.IsConnected);

            await DisconnectAsync();

            Assert.False(listener.IsListening);
            Assert.False(client.IsConnected);
        }

        [Fact]
        public async void SendMessagesToClient()
        {
            WebSocket listenerWebSocket = await ConnectAsync();
            await SendAndReceiveMessageAsync(listenerWebSocket, client);
            await DisconnectAsync();
        }

        [Fact]
        public async void SendMessagesToListener()
        {
            WebSocket listenerWebSocket = await ConnectAsync();
            await SendAndReceiveMessageAsync(client, listenerWebSocket);
            await DisconnectAsync();
        }

        [Fact]
        public async void Reconnect()
        {
            client.ReconnectionEnabled = true;
            WebSocket webSocket = await ConnectAsync();

            Assert.True(client.IsConnected);
            Assert.False(client.IsConnecting);

            listener.Stop();
            await Assert.ThrowsAsync<WebSocketException>(() => client.ReceiveAsync());
            Assert.False(client.IsConnected);
            Assert.True(client.IsConnecting);

            listener.Start();
            webSocket = (await listener.GetWebSocketContextAsync()).WebSocket;
            Assert.True(client.IsConnected);
            Assert.False(client.IsConnecting);

            await DisconnectAsync();

            Assert.False(client.IsConnected);
            Assert.False(client.IsConnecting);
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

            await client.SendAsync(string.Empty);

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

            await client.SendAsync(string.Empty);

            SpinWait.SpinUntil(() => messageReceived);
            Assert.True(messageReceived);
            Assert.False(disconnected);

            webSocketServer.Stop();
            messageReceived = false;
            await Assert.ThrowsAsync<IOException>(() => client.SendAsync(string.Empty));
            await Task.Delay(1000);
            Assert.False(messageReceived);
            SpinWait.SpinUntil(() => disconnected);
            Assert.True(disconnected);

            listener.Stop();
        }
    }
}
