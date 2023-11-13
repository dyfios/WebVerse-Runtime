// Copyright (c) 2019-2023 Five Squared Interactive. All rights reserved.

using UnityEngine;
#if USE_WEBINTERFACE
using FiveSQD.WebVerse.WebInterface.WebSocket;
#endif
using FiveSQD.WebVerse.Utilities;
using Newtonsoft.Json;
using System.IO;
using System;
using FiveSQD.WebVerse.Runtime;

namespace FiveSQD.WebVerse.Daemon
{
    public class WebVerseDaemonManager : BaseManager
    {
        public enum RuntimeType { Focused = 0, WebGL = 1 }

        public RuntimeType runtimeType;

        /// <summary>
        /// Interval in seconds between heartbeats.
        /// </summary>
        public float heartbeatInterval = 5;

#if USE_WEBINTERFACE
        /// <summary>
        /// WebSocket being used.
        /// </summary>
        private WebSocket webSocket = null;
#endif

        /// <summary>
        /// ID for the connection.
        /// </summary>
        private Guid? connectionID;

        /// <summary>
        /// ID for the main app.
        /// </summary>
        private Guid? mainAppID;

        /// <summary>
        /// Initialize the Daemon Manager.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Terminate the Daemon Manager.
        /// </summary>
        public override void Terminate()
        {
            base.Terminate();

            CloseDaemonConnection();
        }

        /// <summary>
        /// Time since the last heartbeat.
        /// </summary>
        private float timeSinceLastHeartbeat = 0;
        private void Update()
        {
#if USE_WEBINTERFACE
            if (webSocket == null)
            {
                return;
            }

            timeSinceLastHeartbeat += Time.deltaTime;
            if (timeSinceLastHeartbeat > heartbeatInterval)
            {
                timeSinceLastHeartbeat = 0;
                SendHeartbeatMessage();
            }
#endif
        }

        /// <summary>
        /// Connect to the Daemon.
        /// </summary>
        /// <param name="port">Port to connect on.</param>
        /// <param name="mainAppID">ID for the main app.</param>
        public void ConnectToDaemon(uint port, Guid mainAppID)
        {
#if USE_WEBINTERFACE
            if (webSocket != null)
            {
                Logging.LogError("[WebVerseDaemonManager->ConnectToDaemon] Connection already exists.");
                return;
            }

            Action<WebSocket> onOpenedAction = (webSocket) =>
            {
                OnOpened();
            };

            Action<WebSocket, ushort, string> onClosedAction = (webSocket, code, msg) =>
            {
                OnClosed(code, msg);
            };

            Action<WebSocket, byte[]> onBinaryAction = (webSocket, data) =>
            {
                OnBinary(data);
            };

            Action<WebSocket, string> onStringAction = (webSocket, data) =>
            {
                OnString(data);
            };

            Action<WebSocket, string> onErrorAction = (webSocket, data) =>
            {
                OnError(data);
            };

            this.mainAppID = mainAppID;
            webSocket = new WebSocket("wss://localhost:" + port,
                onOpenedAction, onClosedAction, onBinaryAction, onStringAction, onErrorAction);
            webSocket.Open();
#endif
        }

        public void CloseDaemonConnection()
        {
#if USE_WEBINTERFACE
            if (webSocket == null || !webSocket.isOpen)
            {
                Logging.LogWarning("[WebVerseDaemonManager->CloseDaemonConnection] Connection already closed.");
                return;
            }

            webSocket.Close();
#endif
        }

        /// <summary>
        /// Invoked when daemon connection is opened.
        /// </summary>
        private void OnOpened()
        {
            Logging.Log("[WebVerseDaemonManager->OnOpened] Connection opened.");
        }

        /// <summary>
        /// Invoked when daemon connection is closed.
        /// </summary>
        /// <param name="code">Code for closure.</param>
        /// <param name="message">Message for closure.</param>
        private void OnClosed(ushort code, string message)
        {
            Logging.Log("[WebVerseDaemonManager->OnOpened] Connection closed. Code: " + code + ", Message: " + message);
        }

        /// <summary>
        /// Invoked when binary data is received.
        /// </summary>
        /// <param name="data">Data that has been received.</param>
        private void OnBinary(byte[] data)
        {
            Logging.LogWarning("[WebVerseDaemonManager->OnBinary] Received binary message. Not supported.");
        }

        /// <summary>
        /// Invoked when string data is received.
        /// </summary>
        /// <param name="data">Data that has been received.</param>
        private void OnString(string data)
        {
            string topic = GetTopicFromJSONMessage(data);
            switch (topic)
            {
                case "IDENTIFICATION-REQ":
                    Logging.Log("[WebVerseDaemonManager->OnString] Received identification request.");
                    HandleIndentificationRequest(data);
                    break;

                case "LOAD-WORLD-REQ":
                    Logging.Log("[WebVerseDaemonManager->OnString] Received load world request.");
                    HandleLoadWorldCommand(data);
                    break;

                default:
                    Logging.LogWarning("[WebVerseDaemonManager->OnString] Unhandled message received.");
                    break;
            }
        }

        /// <summary>
        /// Invoked when an error occurs.
        /// </summary>
        /// <param name="data">Data about the error.</param>
        private void OnError(string data)
        {
            Logging.Log("[WebVerseDaemonManager->OnError] Error: " + data);
        }

        /// <summary>
        /// Get the topic from a JSON message.
        /// </summary>
        /// <param name="json">JSON message.</param>
        /// <returns>The topic, or null.</returns>
        private string GetTopicFromJSONMessage(string json)
        {
            string topic = null;

            JsonTextReader reader = new JsonTextReader(new StringReader(json));
            while (reader.Read())
            {
                string key = null;
                object value = null;

                if (reader.TokenType == JsonToken.PropertyName)
                {
                    if (reader.Value == null)
                    {
                        Logging.LogError("[WebVerseDaemonManager->GetJSONItems] Invalid parameter name.");
                        continue;
                    }
                    key = (string) reader.Value;

                    if (reader.Read())
                    {
                        if (reader.Value == null)
                        {
                            Logging.LogError("[WebVerseDaemonManager->GetJSONItems] Value is null.");
                            continue;
                        }
                        value = reader.Value;
                    }
                    else
                    {
                        Logging.LogError("[WebVerseDaemonManager->GetJSONItems] Unable to find parameter value.");
                        return null;
                    }

                    if (key.ToLower() == "topic")
                    {
                        topic = value == null ? "" : (string) value;
                        break;
                    }
                }
            }

            return topic;
        }

        /// <summary>
        /// Handle an identification request.
        /// </summary>
        /// <param name="request">Request message.</param>
        private void HandleIndentificationRequest(string request)
        {
            WebVerseDaemonMessages.IdentificationRequest reqMessage =
                JsonConvert.DeserializeObject<WebVerseDaemonMessages.IdentificationRequest>(request);

            if (reqMessage == null)
            {
                Logging.LogError("[WebVerseDaemonManager->HandleIdentificationRequest] Error deserializing message.");
                return;
            }

            if (mainAppID.HasValue == false)
            {
                Logging.LogError("[WebVerseDaemonManager->HandleIdentificationRequest] No Main App ID.");
                return;
            }

            WebVerseDaemonMessages.IdentificationResponse respMessage =
                new WebVerseDaemonMessages.IdentificationResponse(reqMessage, mainAppID, "WV-FOCUSED-RUNTIME");
#if USE_WEBINTERFACE
            if (webSocket == null)
            {
                Logging.LogError("[WebVerseDaemonManager->HandleIdentificationRequest] WebSocket not set up.");
                return;
            }

            connectionID = Guid.Parse(respMessage.connectionID);
            Logging.Log("[WebVerseDaemonManager->HandleIdentificationRequest] Sending identification response.");
            webSocket.Send(JsonConvert.SerializeObject(respMessage));
#endif
        }

        private void HandleLoadWorldCommand(string request)
        {
            WebVerseDaemonMessages.LoadWorldCommand loadWorldCommandMessage =
                JsonConvert.DeserializeObject<WebVerseDaemonMessages.LoadWorldCommand>(request);

            if (loadWorldCommandMessage == null)
            {
                Logging.LogError("[WebVerseDaemonManager->HandleLoadWorldCommand] Error deserializing message.");
                return;
            }

            if (mainAppID.HasValue == false)
            {
                Logging.LogError("[WebVerseDaemonManager->HandleLoadWorldCommand] No Main App ID.");
                return;
            }

#if USE_WEBINTERFACE
            if (webSocket == null)
            {
                Logging.LogError("[WebVerseDaemonManager->HandleLoadWorldCommand] WebSocket not set up.");
                return;
            }

            WebVerseRuntime.Instance.LoadWorld(loadWorldCommandMessage.url);
#endif
        }

        /// <summary>
        /// Send a heartbeat message.
        /// </summary>
        private void SendHeartbeatMessage()
        {
#if USE_WEBINTERFACE
            WebVerseDaemonMessages.HeartbeatMessage hbMessage =
                new WebVerseDaemonMessages.HeartbeatMessage("HEARTBEAT", connectionID);

            if (webSocket == null)
            {
                Logging.LogError("[WebVerseDaemonManager->SendHeartbeat] WebSocket not set up.");
                return;
            }

            Logging.Log("[WebVerseDaemonManager->SendHeartbeat] Sending heartbeat.");
            webSocket.Send(JsonConvert.SerializeObject(hbMessage));
#endif
        }
    }
}