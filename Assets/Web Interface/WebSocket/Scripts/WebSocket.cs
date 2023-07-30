using FiveSQD.WebVerse.Utilities;
using System;

namespace FiveSQD.WebVerse.WebInterface.WebSocket
{
    public class WebSocket
    {
        public bool isOpen
        {
            get
            {
                if (webSocket == null)
                {
                    return false;
                }
                return webSocket.IsOpen;
            }
        }

        public Action<WebSocket> onOpen
        {
            set
            {
                if (webSocket == null)
                {
                    Logging.LogWarning("[WebSocket->onOpen] WebSocket not set up.");
                    return;
                }

                webSocket.OnOpen = new BestHTTP.WebSocket.OnWebSocketOpenDelegate((ws) =>
                {
                    value.Invoke(this);
                });
            }
        }

        public Action<WebSocket, ushort, string> onClosed
        {
            set
            {
                if (webSocket == null)
                {
                    Logging.LogWarning("[WebSocket->onClosed] WebSocket not set up.");
                    return;
                }

                webSocket.OnClosed = new BestHTTP.WebSocket.OnWebSocketClosedDelegate((ws, code, msg) =>
                {
                    value.Invoke(this, code, msg);
                });
            }
        }

        public Action<WebSocket, byte[]> onBinary
        {
            set
            {
                if (webSocket == null)
                {
                    Logging.LogWarning("[WebSocket->onBinary] WebSocket not set up.");
                    return;
                }

                webSocket.OnBinary = new BestHTTP.WebSocket.OnWebSocketBinaryDelegate((ws, data) =>
                {
                    value.Invoke(this, data);
                });
            }
        }

        public Action<WebSocket, string> onMessage
        {
            set
            {
                if (webSocket == null)
                {
                    Logging.LogWarning("[WebSocket->onMessage] WebSocket not set up.");
                    return;
                }

                webSocket.OnMessage = new BestHTTP.WebSocket.OnWebSocketMessageDelegate((ws, msg) =>
                {
                    value.Invoke(this, msg);
                });
            }
        }

        public Action<WebSocket, string> onError
        {
            set
            {
                if (webSocket == null)
                {
                    Logging.LogWarning("[WebSocket->onError] WebSocket not set up.");
                    return;
                }

                webSocket.OnError = new BestHTTP.WebSocket.OnWebSocketErrorDelegate((ws, msg) =>
                {
                    value.Invoke(this, msg);
                });
            }
        }

        private BestHTTP.WebSocket.WebSocket webSocket;

        public WebSocket(string uri, Action<WebSocket> onOpen, Action<WebSocket, ushort, string> onClosed,
            Action<WebSocket, byte[]> onBinary, Action<WebSocket, string> onMessage, Action<WebSocket, string> onError)
        {
            webSocket = new BestHTTP.WebSocket.WebSocket(new Uri(uri));

            webSocket.OnOpen = new BestHTTP.WebSocket.OnWebSocketOpenDelegate((ws) =>
            {
                onOpen.Invoke(this);
            });

            webSocket.OnClosed = new BestHTTP.WebSocket.OnWebSocketClosedDelegate((ws, code, msg) =>
            {
                onClosed.Invoke(this, code, msg);
            });

            webSocket.OnBinary = new BestHTTP.WebSocket.OnWebSocketBinaryDelegate((ws, data) =>
            {
                onBinary.Invoke(this, data);
            });

            webSocket.OnMessage = new BestHTTP.WebSocket.OnWebSocketMessageDelegate((ws, msg) =>
            {
                onMessage.Invoke(this, msg);
            });

            webSocket.OnError = new BestHTTP.WebSocket.OnWebSocketErrorDelegate((ws, msg) =>
            {
                onError.Invoke(this, msg);
            });
        }

        public bool AddOnOpenAction(Action<WebSocket> action)
        {
            if (webSocket == null)
            {
                Logging.LogWarning("[WebSocket->AddOnOpenAction] WebSocket not set up.");
                return false;
            }

            webSocket.OnOpen += new BestHTTP.WebSocket.OnWebSocketOpenDelegate((ws) =>
            {
                action.Invoke(this);
            });

            return true;
        }

        public bool AddOnClosedAction(Action<WebSocket, ushort, string> action)
        {
            if (webSocket == null)
            {
                Logging.LogWarning("[WebSocket->AddOnClosedAction] WebSocket not set up.");
                return false;
            }

            webSocket.OnClosed += new BestHTTP.WebSocket.OnWebSocketClosedDelegate((ws, code, msg) =>
            {
                action.Invoke(this, code, msg);
            });

            return true;
        }

        public bool AddOnBinaryAction(Action<WebSocket, byte[]> action)
        {
            if (webSocket == null)
            {
                Logging.LogWarning("[WebSocket->AddOnBinaryAction] WebSocket not set up.");
                return false;
            }

            webSocket.OnBinary += new BestHTTP.WebSocket.OnWebSocketBinaryDelegate((ws, data) =>
            {
                action.Invoke(this, data);
            });

            return true;
        }

        public bool AddOnMessageAction(Action<WebSocket, string> action)
        {
            if (webSocket == null)
            {
                Logging.LogWarning("[WebSocket->AddOnMessageAction] WebSocket not set up.");
                return false;
            }

            webSocket.OnMessage += new BestHTTP.WebSocket.OnWebSocketMessageDelegate((ws, msg) =>
            {
                action.Invoke(this, msg);
            });

            return true;
        }

        public bool AddOnErrorAction(Action<WebSocket, string> action)
        {
            if (webSocket == null)
            {
                Logging.LogWarning("[WebSocket->AddOnErrorAction] WebSocket not set up.");
                return false;
            }

            webSocket.OnError += new BestHTTP.WebSocket.OnWebSocketErrorDelegate((ws, msg) =>
            {
                action.Invoke(this, msg);
            });

            return true;
        }

        public void Open()
        {
            if (webSocket == null)
            {
                Logging.LogWarning("[WebSocket->Open] WebSocket not initialized.");
                return;
            }

            if (webSocket.IsOpen)
            {
                Logging.LogWarning("[WebSocket->Open] WebSocket already open.");
                return;
            }

            webSocket.Open();
        }

        public void Close()
        {
            if (webSocket == null)
            {
                Logging.LogWarning("[WebSocket->Close] WebSocket not initialized.");
                return;
            }

            if (!webSocket.IsOpen)
            {
                Logging.LogWarning("[WebSocket->Close] WebSocket not open.");
            }

            webSocket.Close();
        }

        public void Send(string dataToSend)
        {
            if (webSocket == null)
            {
                Logging.LogWarning("[WebSocket->Send] WebSocket not initialized.");
                return;
            }

            webSocket.Send(dataToSend);
        }

        public void Send(byte[] dataToSend)
        {
            if (webSocket == null)
            {
                Logging.LogWarning("[WebSocket->Send] WebSocket not initialized.");
                return;
            }

            webSocket.Send(dataToSend);
        }
    }
}