using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Sharplus.WebSockets;
using Xunit;

namespace Sharplus.Tests.WebSockets
{
    public class WebSocketClientReactiveTests : BaseWebSocketClientTests<WebSocketClientReactive>
    {
        protected override WebSocketClientReactive CreateWebSocketClient(string url)
        {
            return new WebSocketClientReactive(url);
        }

        [Fact]
        public async void ConnectAndDisconnect()
        {
            bool isConnected = false;

            client.Connected += () => isConnected = true;
            client.Disconnected += (_, __, ___) => isConnected = false;

            await ConnectAsync();
            await Task.Delay(1000);

            Assert.True(listener.IsListening);
            Assert.True(client.IsConnected);
            Assert.True(isConnected);

            await DisconnectAsync();
            await Task.Delay(1000);

            Assert.False(listener.IsListening);
            Assert.False(client.IsConnected);
            Assert.False(isConnected);
        }

        [Fact]
        public async void SendMessagesToClient()
        {
            WebSocketReceiveMessage receiveMessage = null;
            bool isMessageReceived = false;
            WebSocket listenerWebSocket = await ConnectAsync();
            using Stream stream = new MemoryStream();

            client.MessageReceived += m => { receiveMessage = m; isMessageReceived = true; };

            object objectMessage = new { Variable = 123, Variable2 = "Hello" };
            await listenerWebSocket.SendAsync(objectMessage);
            SpinWait.SpinUntil(() => isMessageReceived, 2000);
            isMessageReceived = false;
            Assert.Equal(objectMessage, receiveMessage.Parse(objectMessage.GetType()));

            string stringMessage = "Hello world";
            await listenerWebSocket.SendAsync(stringMessage);
            SpinWait.SpinUntil(() => isMessageReceived, 2000);
            isMessageReceived = false;
            Assert.Equal(stringMessage, receiveMessage.GetContentString());

            byte[] bytesMessage = new byte[] { 0, 1, 2 };
            await listenerWebSocket.SendAsync(bytesMessage);
            SpinWait.SpinUntil(() => isMessageReceived, 2000);
            isMessageReceived = false;
            Assert.Equal(bytesMessage, receiveMessage.Content);

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
