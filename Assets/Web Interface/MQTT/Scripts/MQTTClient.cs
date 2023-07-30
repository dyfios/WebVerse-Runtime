using BestMQTT;
using BestMQTT.Packets.Builders;
using FiveSQD.WebVerse.Utilities;
using System;
using System.Collections.Generic;

namespace FiveSQD.WebVerse.WebInterface.MQTT
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

        public static BufferSegment FromBestMQTT(BestHTTP.PlatformSupport.Memory.BufferSegment bufferSegment)
        {
            return new BufferSegment(bufferSegment.Count, bufferSegment.Data, bufferSegment.Offset);
        }

        public static BestHTTP.PlatformSupport.Memory.BufferSegment ToBestMQTT(BufferSegment bufferSegment)
        {
            return new BestHTTP.PlatformSupport.Memory.BufferSegment(
                bufferSegment.data, bufferSegment.offset, bufferSegment.count);
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

        public static MQTTMessage FromBestMQTT(ApplicationMessage applicationMessage)
        {
            return new MQTTMessage(applicationMessage.ContentType,
                BufferSegment.FromBestMQTT(applicationMessage.CorrelationData),
                applicationMessage.ExpiryInterval, applicationMessage.IsDuplicate,
                BufferSegment.FromBestMQTT(applicationMessage.Payload),
                (PayloadTypes) applicationMessage.PayloadFormat,
                (QOSLevel) applicationMessage.QoS,
                applicationMessage.ResponseTopic, applicationMessage.Retain,
                applicationMessage.Topic, applicationMessage.UserProperties);
        }
    }

    public class MQTTClient
    {
        public enum Transports { TCP, WebSockets }

        public enum ClientState { Initial = 0, TransportConnecting = 1,
            TransportConnected = 2, Connected = 3, Disconnecting = 4, Disconnected = 5 }

        public string host
        {
            get
            {
                if (mqttClient == null || mqttClient.Options == null)
                {
                    Logging.LogWarning("[MQTTClient->host] No client.");
                    return null;
                }

                return mqttClient.Options.Host;
            }
        }

        public int port
        {
            get
            {
                if (mqttClient == null || mqttClient.Options == null)
                {
                    Logging.LogWarning("[MQTTClient->port] No client.");
                    return -1;
                }

                return mqttClient.Options.Port;
            }
        }

        public ClientState clientState
        {
            get
            {
                if (mqttClient == null)
                {
                    Logging.LogWarning("[MQTTClient->clientState] No MQTT client.");
                    return ClientState.Initial;
                }

                switch (mqttClient.State)
                {
                    case ClientStates.Initial:
                        return ClientState.Initial;

                    case ClientStates.TransportConnecting:
                        return ClientState.TransportConnecting;

                    case ClientStates.TransportConnected:
                        return ClientState.TransportConnected;

                    case ClientStates.Connected:
                        return ClientState.Connected;

                    case ClientStates.Disconnecting:
                        return ClientState.Disconnecting;

                    case ClientStates.Disconnected:
                        return ClientState.Disconnected;

                    default:
                        return ClientState.Initial;
                }
            }
        }

        private BestMQTT.MQTTClient mqttClient;

        public MQTTClient(string host, int port, bool useTLS, Transports supportedTransports,
            Action<MQTTClient> onConnected, Action<MQTTClient, byte, string> onDisconnected,
            Action<MQTTClient, ClientState, ClientState> onStateChanged,
            Action<MQTTClient, string> onError,
            string path = "/mqtt")
        {
            ConnectionOptions connectionOptions = new ConnectionOptions();
            connectionOptions.Host = host;
            connectionOptions.Port = port;
            connectionOptions.UseTLS = useTLS;
            connectionOptions.Transport = supportedTransports ==
                Transports.WebSockets ? SupportedTransports.WebSocket : SupportedTransports.TCP;
            connectionOptions.Path = path;

            mqttClient = new BestMQTT.MQTTClient(connectionOptions);

            mqttClient.OnConnected += new BestMQTT.OnConnectedDelegate((client) =>
            {
                onConnected.Invoke(this);
            });

            mqttClient.OnDisconnect += new OnDisconnectDelegate((client, code, msg) =>
            {
                onDisconnected.Invoke(this, (byte) code, msg);
            });

            mqttClient.OnStateChanged += new OnStateChangedDelegate((client, oldState, newState) =>
            {
                onStateChanged.Invoke(this, (ClientState) oldState, (ClientState) newState);
            });

            mqttClient.OnError += new BestMQTT.OnErrorDelegate((client, msg) =>
            {
                onError.Invoke(this, msg);
            });
        }

        public void Connect()
        {
            if (mqttClient == null)
            {
                Logging.LogWarning("[MQTTClient->Connect] Not initialized.");
                return;
            }

            mqttClient.BeginConnect(ConnectPacketBuilderCallback);
        }

        public void Disconnect(string reason = "disconnecting")
        {
            if (mqttClient == null)
            {
                Logging.LogWarning("[MQTTClient->Disconnect] Not initialized.");
                return;
            }
            
            mqttClient.CreateDisconnectPacketBuilder()
                .WithReasonCode(DisconnectReasonCodes.NormalDisconnection)
                .BeginDisconnect();
        }

        public void Subscribe(string topic, Action<string> onAcknowledged,
            Action<MQTTClient, string, string, MQTTMessage> onMessage)
        {
            if (mqttClient == null)
            {
                Logging.LogWarning("[MQTTClient->Subscribe] Not initialized.");
                return;
            }

            mqttClient.CreateSubscriptionBuilder(topic)
                .WithAcknowledgementCallback((mqttClient, topicFilter, reasonCode) =>
                {
                    if (onAcknowledged != null)
                    {
                        onAcknowledged.Invoke(reasonCode.ToString());
                    }
                })
                .WithMessageCallback(new SubscriptionMessageDelegate((client, topic, topicName, msg) =>
                {
                    onMessage.Invoke(this, topic.ToString(), topicName, MQTTMessage.FromBestMQTT(msg));
                }))
                .WithMaximumQoS(BestMQTT.Packets.QoSLevels.ExactlyOnceDelivery)
                .BeginSubscribe();
        }

        public void UnSubscribe(string topic, Action<string> onAcknowledged)
        {
            if (mqttClient == null)
            {
                Logging.LogWarning("[MQTTClient->UnSubscribe] Not initialized.");
                return;
            }

            mqttClient.CreateUnsubscribePacketBuilder(topic)
                .WithAcknowledgementCallback((mqttClient, topicFilter, reasonCode) =>
                {
                    if (onAcknowledged != null)
                    {
                        onAcknowledged.Invoke(reasonCode.ToString());
                    }
                })
                .BeginUnsubscribe();
        }

        public void Publish(string topic, string message)
        {
            if (mqttClient == null)
            {
                Logging.LogWarning("[MQTTClient->Publish] Not initialized.");
                return;
            }

            mqttClient.CreateApplicationMessageBuilder(topic)
                .WithPayload(message)
                .WithQoS(BestMQTT.Packets.QoSLevels.ExactlyOnceDelivery)
                .BeginPublish();
        }

        private ConnectPacketBuilder ConnectPacketBuilderCallback(BestMQTT.MQTTClient mqttClient, ConnectPacketBuilder builder)
        {
            return builder;
        }
    }
}