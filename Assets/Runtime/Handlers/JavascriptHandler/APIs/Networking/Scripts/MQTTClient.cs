using FiveSQD.WebVerse.Runtime;
using FiveSQD.WebVerse.Utilities;
using System;
using System.Collections.Generic;

namespace FiveSQD.WebVerse.Handlers.Javascript.APIs.Networking
{
    public enum QOSLevel
    {
        AtMostOnceDelivery = 0b00, AtLeastOnceDelivery = 0b01,
        ExactlyOnceDelivery = 0b10, Reserved = 0b11
    }

    public enum PayloadTypes
    {
        Bytes = 0b00,
        UTF8 = 0b01
    }

    public class BufferSegment
    {
        public readonly int count;

        public readonly byte[] data;

        public readonly int offset;

        public BufferSegment(int count, byte[] data, int offset)
        {
            this.count = count;
            this.data = data;
            this.offset = offset;
        }
    }

    public class MQTTMessage
    {
        public readonly string contentType;

        public readonly BufferSegment correlationData;

        public readonly TimeSpan expiryInterval;

        public readonly bool isDuplicate;

        public readonly BufferSegment payload;

        public readonly PayloadTypes payloadFormat;

        public readonly QOSLevel qosLevel;

        public readonly string responseTopic;

        public readonly bool retain;

        public readonly string topic;

        public readonly List<KeyValuePair<string, string>> userProperties;

        public MQTTMessage(string contentType, BufferSegment correlationData, TimeSpan expiryInterval,
            bool isDuplicate, BufferSegment payload, PayloadTypes payloadFormat, QOSLevel qosLevel,
            string responseTopic, bool retain, string topic, List<KeyValuePair<string, string>> userProperties)
        {
            this.contentType = contentType;
            this.correlationData = correlationData;
            this.expiryInterval = expiryInterval;
            this.isDuplicate = isDuplicate;
            this.payload = payload;
            this.payloadFormat = payloadFormat;
            this.qosLevel = qosLevel;
            this.responseTopic = responseTopic;
            this.retain = retain;
            this.topic = topic;
            this.userProperties = userProperties;
        }
    }

    public class MQTTClient
    {
        private WebInterface.MQTT.MQTTClient internalClient;

        public MQTTClient(string host, int port, bool useTLS, string transport,
            string onConnected, string onDisconnected, string onStateChanged, string onError,
            string path = "/mqtt")
        {
            WebInterface.MQTT.MQTTClient.Transports supportedTransports = WebInterface.MQTT.MQTTClient.Transports.TCP;
            switch (transport.ToLower())
            {
                case "tcp":
                    supportedTransports = WebInterface.MQTT.MQTTClient.Transports.TCP;
                    break;

                case "websockets":
                    supportedTransports = WebInterface.MQTT.MQTTClient.Transports.WebSockets;
                    break;

                default:
                    Logging.LogWarning("[MQTTClient] Invalid transport " + transport);
                    return;
            }

            Action<WebInterface.MQTT.MQTTClient> onConnectedAction = new Action<WebInterface.MQTT.MQTTClient>((client) =>
            {
                if (!string.IsNullOrEmpty(onConnected))
                {
                    WebVerseRuntime.Instance.javascriptHandler.Run(onConnected.Replace("?", "this"));
                }
            });

            Action<WebInterface.MQTT.MQTTClient, byte, string> onDisconnectedAction
                = new Action<WebInterface.MQTT.MQTTClient, byte, string>((client, code, msg) =>
            {
                if (!string.IsNullOrEmpty(onDisconnected))
                {
                    WebVerseRuntime.Instance.javascriptHandler.Run(onDisconnected.Replace("?", "this, code, msg"));
                }
            });

            Action<WebInterface.MQTT.MQTTClient, WebInterface.MQTT.MQTTClient.ClientState,
                WebInterface.MQTT.MQTTClient.ClientState> onStateChangedAction
                = new Action<WebInterface.MQTT.MQTTClient, WebInterface.MQTT.MQTTClient.ClientState,
                WebInterface.MQTT.MQTTClient.ClientState>((client, from, to) =>
                {
                    if (!string.IsNullOrEmpty(onStateChanged))
                    {
                        WebVerseRuntime.Instance.javascriptHandler.Run(onStateChanged.Replace("?", "this, from, to"));
                    }
                });

            Action<WebInterface.MQTT.MQTTClient, string> onErrorAction
                = new Action<WebInterface.MQTT.MQTTClient, string>((client, msg) =>
                {
                    if (!string.IsNullOrEmpty(onError))
                    {
                        WebVerseRuntime.Instance.javascriptHandler.Run(onError.Replace("?", "this, msg"));
                    }
                });

            internalClient = new WebInterface.MQTT.MQTTClient(host, port, useTLS, supportedTransports,
                onConnectedAction, onDisconnectedAction, onStateChangedAction, onErrorAction, path);
        }

        public bool Connect()
        {
            if (internalClient == null)
            {
                Logging.LogWarning("[MQTTClient:Connect] Not initialized.");
                return false;
            }

            internalClient.Connect();
            return true;
        }

        public bool Disconnect()
        {
            if (internalClient == null)
            {
                Logging.LogWarning("[MQTTClient:Disconnect] Not initialized.");
                return false;
            }

            internalClient.Disconnect();
            return true;
        }

        public bool Subscribe(string topic, string onAcknowledged, string onMessage)
        {
            if (internalClient == null)
            {
                Logging.LogWarning("[MQTTClient:Subscribe] Not initialized.");
                return false;
            }

            Action<string> onAcknowledgedAction = new Action<string>((msg) =>
            {
                if (!string.IsNullOrEmpty(onAcknowledged))
                {
                    WebVerseRuntime.Instance.javascriptHandler.Run(onAcknowledged.Replace("?", "msg"));
                }
            });

            Action<WebInterface.MQTT.MQTTClient, string, string, WebInterface.MQTT.MQTTMessage> onMessageAction
                = new Action<WebInterface.MQTT.MQTTClient, string, string, WebInterface.MQTT.MQTTMessage>((client, topic, topicName, msg) =>
            {
                if (!string.IsNullOrEmpty(onAcknowledged))
                {
                    WebVerseRuntime.Instance.javascriptHandler.Run(onMessage.Replace("?", "client, topic, topicName, msg"));
                }
            });

            internalClient.Subscribe(topic, onAcknowledgedAction, onMessageAction);
            return true;
        }

        public bool UnSubscribe(string topic, string onAcknowledged, string onMessage)
        {
            if (internalClient == null)
            {
                Logging.LogWarning("[MQTTClient:UnSubscribe] Not initialized.");
                return false;
            }

            Action<string> onAcknowledgedAction = new Action<string>((msg) =>
            {
                if (!string.IsNullOrEmpty(onAcknowledged))
                {
                    WebVerseRuntime.Instance.javascriptHandler.Run(onAcknowledged.Replace("?", "msg"));
                }
            });

            internalClient.UnSubscribe(topic, onAcknowledgedAction);
            return true;
        }

        public bool Publish(string topic, string message)
        {
            if (internalClient == null)
            {
                Logging.LogWarning("[MQTTClient:Publish] Not initialized.");
                return false;
            }

            internalClient.Publish(topic, message);
            return true;
        }
    }
}