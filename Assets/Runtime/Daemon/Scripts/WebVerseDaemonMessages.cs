// Copyright (c) 2019-2023 Five Squared Interactive. All rights reserved.

using FiveSQD.WebVerse.Handlers.Javascript.APIs.Utilities;
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace FiveSQD.WebVerse.Daemon
{
    public class WebVerseDaemonMessages : MonoBehaviour
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
            /// Constructor for a JSON-serializable identification request.
            /// </summary>
            /// <param name="topic">Topic.</param>
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