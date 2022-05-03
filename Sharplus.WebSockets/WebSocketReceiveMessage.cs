using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Sharplus.WebSockets
{
    public class WebSocketReceiveMessage
    {
        public WebSocketCloseStatus? CloseStatus { get; private set; }
        public string CloseStatusDescription { get; private set; }
        public byte[] Content { get; private set; }
        public WebSocketMessageType MessageType { get; private set; }

        public WebSocketReceiveMessage(WebSocketReceiveResult receiveResult, byte[] content)
        {
            CloseStatus = receiveResult.CloseStatus;
            CloseStatusDescription = receiveResult.CloseStatusDescription;
            MessageType = receiveResult.MessageType;
            Content = content;
        }

        public string GetContentString(Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            return encoding.GetString(Content, 0, Content.Length);
        }

        public object Parse(Type type, Encoding encoding = null, JsonSerializerOptions options = null)
        {
            string json = GetContentString(encoding);
            object result = JsonSerializer.Deserialize(json, type, options);

            return result;
        }

        public T Parse<T>(Encoding encoding = null, JsonSerializerOptions options = null)
        {
            string json = GetContentString(encoding);
            T result = JsonSerializer.Deserialize<T>(json, options);

            return result;
        }

        public bool TryParse(Type type, out object result, Encoding encoding = null, JsonSerializerOptions options = null)
        {
            result = default;
            bool success = false;

            try
            {
                result = Parse(type, encoding, options);
                success = true;
            }
            catch { }

            return success;
        }

        public bool TryParse<T>(out T result, Encoding encoding = null, JsonSerializerOptions options = null)
        {
            result = default;
            bool success = false;

            try
            {
                result = Parse<T>(encoding, options);
                success = true;
            }
            catch { }

            return success;
        }
    }
}
