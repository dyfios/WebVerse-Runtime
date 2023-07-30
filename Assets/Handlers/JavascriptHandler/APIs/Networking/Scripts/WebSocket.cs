using FiveSQD.WebVerse.Runtime;
using FiveSQD.WebVerse.Utilities;

namespace FiveSQD.WebVerse.Handlers.Javascript.APIs.Networking
{
    public class WebSocketEvent
    {
        public struct Data
        {
            public byte[] binary;

            public string message;
        }

        public WebSocket webSocket;

        public int code;

        public string details;

        public Data data;

        public WebSocketEvent(WebSocket webSocket, string message, int code, string details)
        {
            this.webSocket = webSocket;
            data = new Data()
            {
                message = message
            };
            this.code = code;
            this.details = details;
        }

        public WebSocketEvent(WebSocket webSocket, byte[] binary, int code, string details)
        {
            this.webSocket = webSocket;
            data = new Data()
            {
                binary = binary
            };
            this.code = code;
            this.details = details;
        }
    }

    public class WebSocket
    {
        private WebInterface.WebSocket.WebSocket internalWebSocket;

        public WebSocket(string uri)
        {
            internalWebSocket = new WebInterface.WebSocket.WebSocket(uri, null, null, null, null, null);
        }

        public bool AddEventListener(string type, string functionToCall)
        {
            if (internalWebSocket == null)
            {
                Logging.LogWarning("[WebSocket:AddEventListener] WebSocket not set up.");
                return false;
            }

            WebSocketEvent evt = null;
            switch (type)
            {
                case "open":
                    System.Action<WebInterface.WebSocket.WebSocket> onOpenAction = string.IsNullOrEmpty(functionToCall) ?
                        null : (ws) =>
                    {
                        evt = new WebSocketEvent(this, (string) null, 0, null);
                        WebVerseRuntime.Instance.javascriptHandler.Run(functionToCall.Replace("?", "evt"));
                    };
                    return internalWebSocket.AddOnOpenAction(onOpenAction);

                case "close":
                    System.Action<WebInterface.WebSocket.WebSocket, ushort, string> onClosedAction = string.IsNullOrEmpty(functionToCall) ?
                        null : (ws, code, msg) =>
                        {
                            evt = new WebSocketEvent(this, (string) null, code, msg);
                            WebVerseRuntime.Instance.javascriptHandler.Run(functionToCall.Replace("?", "evt"));
                        };
                    return internalWebSocket.AddOnClosedAction(onClosedAction);

                case "binary":
                    System.Action<WebInterface.WebSocket.WebSocket, byte[]> onBinaryAction = string.IsNullOrEmpty(functionToCall) ?
                        null : (ws, data) =>
                        {
                            evt = new WebSocketEvent(this, data, 0, null);
                            WebVerseRuntime.Instance.javascriptHandler.Run(functionToCall.Replace("?", "evt"));
                        };
                    return internalWebSocket.AddOnBinaryAction(onBinaryAction);

                case "message":
                    System.Action<WebInterface.WebSocket.WebSocket, string> onMessageAction = string.IsNullOrEmpty(functionToCall) ?
                        null : (ws, msg) =>
                        {
                            evt = new WebSocketEvent(this, msg, 0, null);
                            WebVerseRuntime.Instance.javascriptHandler.Run(functionToCall.Replace("?", "evt"));
                        };
                    return internalWebSocket.AddOnMessageAction(onMessageAction);

                case "error":
                    System.Action<WebInterface.WebSocket.WebSocket, string> onErrorAction = string.IsNullOrEmpty(functionToCall) ?
                        null : (ws, msg) =>
                        {
                            evt = new WebSocketEvent(this, (string) null, -1, msg);
                            WebVerseRuntime.Instance.javascriptHandler.Run(functionToCall.Replace("?", "evt"));
                        };
                    return internalWebSocket.AddOnErrorAction(onErrorAction);

                default:
                    Logging.LogWarning("[WebSocket:AddEventListener] Unknown event type " + type);
                    return false;
            }
        }

        public bool Send(string dataToSend)
        {
            if (internalWebSocket == null)
            {
                Logging.LogWarning("[WebSocket:Send] WebSocket not set up.");
                return false;
            }

            if (string.IsNullOrEmpty(dataToSend))
            {
                Logging.LogWarning("[WebSocket:Send] No data to send.");
                return false;
            }

            internalWebSocket.Send(dataToSend);
            return true;
        }

        public bool Send(byte[] dataToSend)
        {
            if (internalWebSocket == null)
            {
                Logging.LogWarning("[WebSocket:Send] WebSocket not set up.");
                return false;
            }

            if (dataToSend == null || dataToSend.Length == 0)
            {
                Logging.LogWarning("[WebSocket:Send] No data to send.");
            }

            internalWebSocket.Send(dataToSend);
            return true;
        }
    }
}