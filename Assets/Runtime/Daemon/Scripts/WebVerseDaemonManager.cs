// Copyright (c) 2019-2024 Five Squared Interactive. All rights reserved.

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
    /// <summary>
    /// Class for the WebVerse Daemon Manager.
    /// </summary>
    public class WebVerseDaemonManager : BaseManager
    {
        /// <summary>
        /// Runtime type.
        /// </summary>
        public enum RuntimeType { Focused = 0, WebGL = 1 }

        /// <summary>
        /// Runtime type.
        /// </summary>
        public RuntimeType runtimeType;

        /// <summary>
        /// Interval in seconds between heartbeats.
        /// </summary>
        public float heartbeatInterval = 5;

        /// <summary>
        /// The port being used.
        /// </summary>
        public uint port { get; private set; }

        /// <summary>
        /// ID for the main app.
        /// </summary>
        public Guid? mainAppID { get; private set; }

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
        /// ID for the tab.
        /// </summary>
        private int? tabID;

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
        /// <param name="tabID">ID for the tab.</param>
        public void ConnectToDaemon(uint port, Guid mainAppID, int tabID)
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
            this.tabID = tabID;
            this.port = port;
            webSocket = new WebSocket("wss://localhost:" + port,
                onOpenedAction, onClosedAction, onBinaryAction, onStringAction, onErrorAction);
            webSocket.Open();
#endif
        }

        /// <summary>
        /// Close the Daemon socket connection.
        /// </summary>
        public void CloseDaemonConnection()
        {
#if USE_WEBINTERFACE
            if (webSocket == null || !webSocket.isOpen)
            {
                Logging.LogWarning("[WebVerseDaemonManager->CloseDaemonConnection] Connection already closed.");
                return;
            }

            webSocket.Close();
            port = 0;
#endif
        }

        /// <summary>
        /// Send a close request.
        /// </summary>
        public void SendCloseRequest()
        {
            WebVerseDaemonMessages.CloseRequest closeRequest =
                new WebVerseDaemonMessages.CloseRequest(connectionID);

            if (webSocket == null)
            {
                Logging.LogError("[WebVerseDaemonManager->SendCloseRequest] WebSocket not set up.");
                return;
            }

            //Logging.Log("[WebVerseDaemonManager->SendCloseRequest] Sending close request.");
            webSocket.Send(JsonConvert.SerializeObject(closeRequest));
        }

        /// <summary>
        /// Send a focused tab request.
        /// </summary>
        /// <param name="url">URL to use.</param>
        /// <param name="runtimeType">Runtime type to use.</param>
        public void SendFocusedTabRequest(string url, string runtimeType)
        {
            WebVerseDaemonMessages.FocusedTabRequest focusedTabRequest =
                new WebVerseDaemonMessages.FocusedTabRequest(url, runtimeType, connectionID);

            if (webSocket == null)
            {
                Logging.LogError("[WebVerseDaemonManager->SendFocusedTabRequest] WebSocket not set up.");
                return;
            }

            //Logging.Log("[WebVerseDaemonManager->SendFocusedTabRequest] Sending focused tab request.");
            webSocket.Send(JsonConvert.SerializeObject(focusedTabRequest));
        }

        /// <summary>
        /// Send a settings update request.
        /// </summary>
        /// <param name="storageEntries">Storage entries to use.</param>
        /// <param name="storageKeyLength">Storage key length to use.</param>
        /// <param name="storageEntriesLength">Storage entries length to use.</param>
        public void SendSettingsUpdateRequest(int storageEntries, int storageKeyLength,
            int storageEntriesLength)
        {
            WebVerseDaemonMessages.UpdateSettingsRequest updateSettingsRequest =
                new WebVerseDaemonMessages.UpdateSettingsRequest(
                    storageEntries, storageKeyLength, storageEntriesLength, connectionID);

            if (webSocket == null)
            {
                Logging.LogError("[WebVerseDaemonManager->SendSettingsUpdateRequest] WebSocket not set up.");
                return;
            }

            //Logging.Log("[WebVerseDaemonManager->SendSettingsUpdareRequest] Sending update settings request.");
            webSocket.Send(JsonConvert.SerializeObject(updateSettingsRequest));
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

                case "LOAD-WORLD-CMD":
                    Logging.Log("[WebVerseDaemonManager->OnString] Received load world command.");
                    HandleLoadWorldCommand(data);
                    break;

                case "UPDATE-HIST-CMD":
                    Logging.Log("[WebVerseDaemonManager->OnString] Received update history command.");
                    HandleUpdateHistoryCommand(data);
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

            if (tabID.HasValue == false)
            {
                Logging.LogError("[WebVerseDaemonManager->HandleIdentificationRequest] No Tab ID.");
                return;
            }

            WebVerseDaemonMessages.IdentificationResponse respMessage =
                new WebVerseDaemonMessages.IdentificationResponse(reqMessage, mainAppID, tabID, "WV-FOCUSED-RUNTIME");
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

        /// <summary>
        /// Handle a Load World command.
        /// </summary>
        /// <param name="request">The request.</param>
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

            WebVerseRuntime.Instance.LoadURL(loadWorldCommandMessage.url);
#endif
        }

        /// <summary>
        /// Handle an Update History command.
        /// </summary>
        /// <param name="request">The request.</param>
        private void HandleUpdateHistoryCommand(string request)
        {
            WebVerseDaemonMessages.UpdateHistoryCommand updateHistoryCommandMessage =
                JsonConvert.DeserializeObject<WebVerseDaemonMessages.UpdateHistoryCommand>(request);

            if (updateHistoryCommandMessage == null)
            {
                Logging.LogError("[WebVerseDaemonManager->HandleUpdateHistoryCommand] Error deserializing message.");
                return;
            }

            if (mainAppID.HasValue == false)
            {
                Logging.LogError("[WebVerseDaemonManager->HandleUpdateHistoryCommand] No Main App ID.");
                return;
            }

#if USE_WEBINTERFACE
            if (webSocket == null)
            {
                Logging.LogError("[WebVerseDaemonManager->HandleUpdateHistoryCommand] WebSocket not set up.");
                return;
            }

            WebVerseRuntime.Instance.handMenuController.UpdateHistory(updateHistoryCommandMessage.history);
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

            //Logging.Log("[WebVerseDaemonManager->SendHeartbeat] Sending heartbeat.");
            webSocket.Send(JsonConvert.SerializeObject(hbMessage));
#endif
        }
    }
}