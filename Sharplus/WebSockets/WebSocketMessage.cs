using System;
using System.Text;
using System.Text.Json;

namespace Sharplus.WebSockets
{
    public class WebSocketMessage
    {
        private byte[] _content;
        private string _contentString;

        public byte[] ContentBytes => _content;
        public string ContentString => _contentString ??= GetContentString();

        public WebSocketMessage(byte[] content)
        {
            _content = content;
        }

        private string GetContentString()
        {
            return _content != null ? Encoding.UTF8.GetString(_content) : string.Empty;
        }

        public bool TryParse<T>(out T result)
        {
            result = default;
            bool success = false;
            string json = ContentString;

            try
            {
                result = JsonSerializer.Deserialize<T>(json);
                success = true;
            }
            catch (Exception e)
            {
            };

            return success;
        }
    }
}
