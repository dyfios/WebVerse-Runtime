// Copyright (c) 2019-2024 Five Squared Interactive. All rights reserved.

using FiveSQD.WebVerse.Handlers.Javascript.APIs.Utilities;
using Newtonsoft.Json;
using System;

namespace FiveSQD.WebVerse.Daemon
{
    public class WebVerseDaemonMessages
    {
        /// <summary>
        /// Class for a JSON-serializable identification request.
        /// </summary>
        public class IdentificationRequest
        {
            /// <summary>
            /// Topic.
            /// </summary>
            [JsonProperty(PropertyName = "topic")]
            public string topic;

            /// <summary>
            /// Connection ID.
            /// </summary>
            [JsonProperty(PropertyName = "connectionID")]
            public string connectionID;

            /// <summary>
            /// Constructor for a JSON-serializable identification request.
            /// </summary>
            /// <param name="topic">Topic.</param>
            /// <param name="connectionID">Connection ID.</param>
            public IdentificationRequest(string topic, Guid? connectionID)
            {
                this.topic = topic;
                this.connectionID = connectionID.HasValue ? connectionID.Value.ToString() : "";
            }
        }

        /// <summary>
        /// Class for a JSON-serializable identification response.
        /// </summary>
        public class IdentificationResponse
        {
            /// <summary>
            /// Topic.
            /// </summary>
            [JsonProperty(PropertyName = "topic")]
            public string topic;

            /// <summary>
            /// Client Type.
            /// </summary>
            [JsonProperty(PropertyName = "clientType")]
            public string clientType;

            /// <summary>
            /// Connection ID.
            /// </summary>
            [JsonProperty(PropertyName = "connectionID")]
            public string connectionID;

            /// <summary>
            /// Window ID.
            /// </summary>
            [JsonProperty(PropertyName = "windowID")]
            public string windowID;

            /// <summary>
            /// Tab ID.
            /// </summary>
            [JsonProperty(PropertyName = "tabID")]
            public int tabID;

            /// <summary>
            /// Constructor for a JSON-serializable identification request.
            /// </summary>
            /// <param name="request">Request Message.</param>
            /// <param name="clientType">Client Type.</param>
            public IdentificationResponse(IdentificationRequest request, Guid? windowID, int? tabID, string clientType)
            {
                if (request == null)
                {
                    Logging.LogError("[IdentificationResponse] Invalid request.");
                }

                topic = "IDENTIFICATION-RESP";
                connectionID = request.connectionID.ToString();
                this.windowID = windowID.HasValue ? windowID.Value.ToString() : "";
                this.tabID = tabID.HasValue ? tabID.Value : 0;
                this.clientType = clientType;
            }
        }

        /// <summary>
        /// Class for a JSON-serializable load world command.
        /// </summary>
        public class LoadWorldCommand
        {
            /// <summary>
            /// Topic.
            /// </summary>
            [JsonProperty(PropertyName = "topic")]
            public string topic;

            /// <summary>
            /// World URL.
            /// </summary>
            [JsonProperty(PropertyName = "url")]
            public string url;

            /// <summary>
            /// Connection ID.
            /// </summary>
            [JsonProperty(PropertyName = "connectionID")]
            public string connectionID;

            /// <summary>
            /// Constructor for a JSON-serializable load world command.
            /// </summary>
            /// <param name="url">World URL.</param>
            /// <param name="connectionID">Connection ID.</param>
            public LoadWorldCommand(string url, Guid? connectionID)
            {
                topic = "LOAD-WORLD-CMD";
                this.url = url;
                this.connectionID = connectionID.HasValue ? connectionID.Value.ToString() : "";
            }
        }

        /// <summary>
        /// Class for a JSON-serializable close request.
        /// </summary>
        public class CloseRequest
        {
            /// <summary>
            /// Topic.
            /// </summary>
            [JsonProperty(PropertyName = "topic")]
            public string topic;

            /// <summary>
            /// Connection ID.
            /// </summary>
            [JsonProperty(PropertyName = "connectionID")]
            public string connectionID;

            /// <summary>
            /// Constructor for a JSON-serializable close request.
            /// </summary>
            /// <param name="connectionID">Connection ID.</param>
            public CloseRequest(Guid? connectionID)
            {
                topic = "CLOSE-REQ";
                this.connectionID = connectionID.HasValue ? connectionID.Value.ToString() : "";
            }
        }

        /// <summary>
        /// Class for a JSON-serializable focused tab request.
        /// </summary>
        public class FocusedTabRequest
        {
            /// <summary>
            /// Topic.
            /// </summary>
            [JsonProperty(PropertyName = "topic")]
            public string topic;

            /// <summary>
            /// World URL.
            /// </summary>
            [JsonProperty(PropertyName = "url")]
            public string url;

            /// <summary>
            /// Runtime Type.
            /// </summary>
            [JsonProperty(PropertyName = "runtimeType")]
            public string runtimeType;

            /// <summary>
            /// Connection ID.
            /// </summary>
            [JsonProperty(PropertyName = "connectionID")]
            public string connectionID;

            /// <summary>
            /// Constructor for a JSON-serializable focused tab request.
            /// </summary>
            /// <param name="url">World URL.</param>
            /// <param name="connectionID">Connection ID.</param>
            public FocusedTabRequest(string url, string runtimeType, Guid? connectionID)
            {
                topic = "FOCUSED-TAB-REQ";
                this.url = url;
                this.runtimeType = runtimeType;
                this.connectionID = connectionID.HasValue ? connectionID.Value.ToString() : "";
            }
        }

        /// <summary>
        /// Class for a JSON-serializable update history command.
        /// </summary>
        public class UpdateHistoryCommand
        {
            /// <summary>
            /// Topic.
            /// </summary>
            [JsonProperty(PropertyName = "topic")]
            public string topic;

            /// <summary>
            /// History.
            /// </summary>
            [JsonProperty(PropertyName = "history")]
            public string history;

            /// <summary>
            /// Connection ID.
            /// </summary>
            [JsonProperty(PropertyName = "connectionID")]
            public string connectionID;

            /// <summary>
            /// Constructor for a JSON-serializable update history command.
            /// </summary>
            /// <param name="history">History.</param>
            /// <param name="connectionID">Connection ID.</param>
            public UpdateHistoryCommand(string history, Guid? connectionID)
            {
                topic = "UPDATE_HIST-CMD";
                this.history = history;
                this.connectionID = connectionID.HasValue ? connectionID.Value.ToString() : "";
            }
        }

        /// <summary>
        /// Class for a JSON-serializable update settings request.
        /// </summary>
        public class UpdateSettingsRequest
        {
            /// <summary>
            /// Topic.
            /// </summary>
            [JsonProperty(PropertyName = "topic")]
            public string topic;

            /// <summary>
            /// Storage Entries.
            /// </summary>
            [JsonProperty(PropertyName = "storageEntries")]
            public int storageEntries;

            /// <summary>
            /// Storage Key Length.
            /// </summary>
            [JsonProperty(PropertyName = "storageKeyLength")]
            public int storageKeyLength;

            /// <summary>
            /// Storage Entry Length.
            /// </summary>
            [JsonProperty(PropertyName = "storageEntryLength")]
            public int storageEntryLength;

            /// <summary>
            /// Connection ID.
            /// </summary>
            [JsonProperty(PropertyName = "connectionID")]
            public string connectionID;

            /// <summary>
            /// Constructor for a JSON-serializable update history command.
            /// </summary>
            /// <param name="storageEntries">Storage Entries.</param>
            /// <param name="storageKeyLength">Storage Key Length.</param>
            /// <param name="storageEntryLength">Storage Entry Length.</param>
            /// <param name="connectionID">Connection ID.</param>
            public UpdateSettingsRequest(int storageEntries, int storageKeyLength, int storageEntryLength, Guid? connectionID)
            {
                topic = "UPDATE_HIST-CMD";
                this.storageEntries = storageEntries;
                this.storageKeyLength = storageKeyLength;
                this.storageEntryLength = storageEntryLength;
                this.connectionID = connectionID.HasValue ? connectionID.Value.ToString() : "";
            }
        }

        /// <summary>
        /// Class for a JSON-serializable heartbeat message.
        /// </summary>
        public class HeartbeatMessage
        {
            /// <summary>
            /// Topic.
            /// </summary>
            [JsonProperty(PropertyName = "topic")]
            public string topic;

            /// <summary>
            /// Connection ID.
            /// </summary>
            [JsonProperty(PropertyName = "connectionID")]
            public string connectionID;

            /// <summary>
            /// Constructor for a JSON-serializable heartbeat message.
            /// </summary>
            /// <param name="topic">Topic.</param>
            /// <param name="connectionID">Connection ID.</param>
            public HeartbeatMessage(string topic, Guid? connectionID)
            {
                this.topic = topic;
                this.connectionID = connectionID.HasValue ? connectionID.Value.ToString() : "";
            }
        }
    }
}