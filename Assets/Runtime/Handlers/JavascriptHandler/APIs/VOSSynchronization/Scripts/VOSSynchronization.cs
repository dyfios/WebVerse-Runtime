using FiveSQD.WebVerse.WorldEngine.Utilities;
using System;
using FiveSQD.WebVerse.Runtime;

namespace FiveSQD.WebVerse.Handlers.Javascript.APIs.VOSSynchronization
{
    public class VOSSynchronization
    {
        public enum Transport { TCP, WebSocket }

        public static bool ConnectToService(string host, int port, bool tls,
            string onConnected = null, Transport transport = Transport.TCP)
        {
            WebInterface.MQTT.MQTTClient.Transports tp = WebInterface.MQTT.MQTTClient.Transports.TCP;
            switch (transport)
            {
                case Transport.TCP:
                    tp = WebInterface.MQTT.MQTTClient.Transports.TCP;
                    break;

                case Transport.WebSocket:
                    tp = WebInterface.MQTT.MQTTClient.Transports.WebSockets;
                    break;
            }

            WebVerse.VOSSynchronization.VOSSynchronizer newSynchronizer
                = WebVerseRuntime.Instance.vosSynchronizationManager.AddSynchronizer(host, port, tls, tp);
            if (newSynchronizer == null)
            {
                LogSystem.LogError("[VOSSynchronization:ConnectToService] Unable to set up synchronizer.");
                return false;
            }
            Action onConnectedAction = () => {
                InvokeCallback(onConnected);
            };
            newSynchronizer.Connect(onConnected == null ? null : onConnectedAction);
            return true;
        }

        public static bool DisconnectService(string host, int port)
        {
            WebVerse.VOSSynchronization.VOSSynchronizer synchronizerToRemove
                = WebVerseRuntime.Instance.vosSynchronizationManager.GetSynchronizer(host, port);
            if (synchronizerToRemove == null)
            {
                LogSystem.LogError("[VOSSynchronization:DisconnectService] Can't find service to disconnect.");
                return false;
            }
            synchronizerToRemove.Disconnect();
            return true;
        }

        public static bool CreateSession(string host, int port, string id, string tag)
        {
            WebVerse.VOSSynchronization.VOSSynchronizer synchronizerToCreateSessionOn
                = WebVerseRuntime.Instance.vosSynchronizationManager.GetSynchronizer(host, port);
            if (synchronizerToCreateSessionOn == null)
            {
                LogSystem.LogError("[VOSSynchronization:CreateSession] Can't find service to create session on.");
                return false;
            }

            Guid uuid = Guid.NewGuid();
            if (Guid.TryParse(id, out uuid) == false)
            {
                LogSystem.LogError("[VOSSynchronization:CreateSession] Invalid UUID.");
                return false;
            }

            synchronizerToCreateSessionOn.CreateSession(uuid, tag);
            return true;
        }

        public static bool DestroySession(string host, int port)
        {
            WebVerse.VOSSynchronization.VOSSynchronizer synchronizerToDestroySessionOn
                = WebVerseRuntime.Instance.vosSynchronizationManager.GetSynchronizer(host, port);
            if (synchronizerToDestroySessionOn == null)
            {
                LogSystem.LogError("[VOSSynchronization:DestroySession] Can't find service to destroy session on.");
                return false;
            }
            synchronizerToDestroySessionOn.DestroySession();
            return true;
        }

        public static string JoinSession(string host, int port, string id, string tag, string callback = null)
        {
            WebVerse.VOSSynchronization.VOSSynchronizer synchronizerToJoinSessionOn
                = WebVerseRuntime.Instance.vosSynchronizationManager.GetSynchronizer(host, port);
            if (synchronizerToJoinSessionOn == null)
            {
                LogSystem.LogError("[VOSSynchronization:JoinSession] Can't find service to join session on.");
                return null;
            }

            Guid uuid = Guid.NewGuid();
            if (Guid.TryParse(id, out uuid) == false)
            {
                LogSystem.LogError("[VOSSynchronization:JoinSession] Invalid UUID.");
                return null;
            }

            Guid? clientID = synchronizerToJoinSessionOn.JoinSession(uuid, tag);

            // Handle callback.
            Action onJoinAction = null;
            if (!string.IsNullOrEmpty(callback))
            {
                onJoinAction = () => {
                    WebVerseRuntime.Instance.javascriptHandler.Run(callback);
                };
            }

            synchronizerToJoinSessionOn.GetSessionState(onJoinAction);

            return clientID.HasValue ? clientID.Value.ToString() : null;
        }

        public static bool ExitSession(string host, int port)
        {
            WebVerse.VOSSynchronization.VOSSynchronizer synchronizerToExitSessionOn
                = WebVerseRuntime.Instance.vosSynchronizationManager.GetSynchronizer(host, port);
            if (synchronizerToExitSessionOn == null)
            {
                LogSystem.LogError("[VOSSynchronization:ExitSession] Can't find service to exit session on.");
                return false;
            }

            synchronizerToExitSessionOn.ExitSession();
            return true;
        }

        public static bool StartSynchronizingEntity(string host, int port,
            string entityID, bool deleteWithClient = false, string resources = null)
        {
            WebVerse.VOSSynchronization.VOSSynchronizer synchronizerToSynchronizeEntityOn
                = WebVerseRuntime.Instance.vosSynchronizationManager.GetSynchronizer(host, port);
            if (synchronizerToSynchronizeEntityOn == null)
            {
                LogSystem.LogError("[VOSSynchronization:StartSynchronizingEntity] Can't find service to synchronize entity on.");
                return false;
            }

            Guid uuid = Guid.NewGuid();
            if (Guid.TryParse(entityID, out uuid) == false)
            {
                LogSystem.LogError("[VOSSynchronizationAPI->StartSynchronizingEntity] Invalid entity UUID.");
                return false;
            }

            WorldEngine.Entity.BaseEntity entityToSynchronize = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(uuid);
            if (entityToSynchronize == null)
            {
                LogSystem.LogError("[VOSSynchronizationAPI->StartSynchronizingEntity] Unable to find entity to synchronize.");
                return false;
            }
            synchronizerToSynchronizeEntityOn.AddSynchronizedEntity(entityToSynchronize, deleteWithClient, resources);
            return true;
        }

        public static bool StopSynchronizingEntity(string host, int port, string entityID)
        {
            WebVerse.VOSSynchronization.VOSSynchronizer synchronizerToStopSynchronizingEntityOn
                = WebVerseRuntime.Instance.vosSynchronizationManager.GetSynchronizer(host, port);
            if (synchronizerToStopSynchronizingEntityOn == null)
            {
                LogSystem.LogError("[VOSSynchronization:StopSynchronizingEntity] Can't find service to stop synchronizing entity on.");
                return false;
            }

            Guid uuid = Guid.NewGuid();
            if (Guid.TryParse(entityID, out uuid) == false)
            {
                LogSystem.LogError("[VOSSynchronization:StopSynchronizingEntity] Invalid entity UUID.");
                return false;
            }

            WorldEngine.Entity.BaseEntity entityToStopSynchronizing = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(uuid);
            if (entityToStopSynchronizing == null)
            {
                LogSystem.LogError("[VOSSynchronization:StopSynchronizingEntity] Unable to find entity to stop synchronizing.");
                return false;
            }

            synchronizerToStopSynchronizingEntityOn.RemoveSynchronizedEntity(entityToStopSynchronizing);
            return true;
        }

        public static bool SendMessage(string host, int port, string topic, string message)
        {
            WebVerse.VOSSynchronization.VOSSynchronizer synchronizerToSendMessageOn
                = WebVerseRuntime.Instance.vosSynchronizationManager.GetSynchronizer(host, port);
            if (synchronizerToSendMessageOn == null)
            {
                LogSystem.LogError("[VOSSynchronization:SendMessage] Can't find service to send message on.");
                return false;
            }

            synchronizerToSendMessageOn.SendMessage(topic, message);
            return true;
        }

        public static bool RegisterMessageCallback(string host, int port, string callback)
        {
            WebVerse.VOSSynchronization.VOSSynchronizer synchronizerToRegisterMessageCallbackOn
                = WebVerseRuntime.Instance.vosSynchronizationManager.GetSynchronizer(host, port);
            if (synchronizerToRegisterMessageCallbackOn == null)
            {
                LogSystem.LogError("[VOSSynchronization:RegisterMessageCallback] Can't find service to register message callback on.");
                return false;
            }

            // Handle callback.
            Action<string, string, string> onMessageAction = null;
            if (!string.IsNullOrEmpty(callback))
            {
                onMessageAction = (string topic, string senderID, string message) => {
                    WebVerseRuntime.Instance.javascriptHandler.Run(callback.Replace("?",
                        "\"" + topic + "\", \"" + senderID + "\", \"" + message + "\""));
                };
            }
            synchronizerToRegisterMessageCallbackOn.AddMessageListener(onMessageAction);

            return true;
        }

        public static string GetUserTag(string host, int port, string userID)
        {
            WebVerse.VOSSynchronization.VOSSynchronizer synchronizerToGetUserTagOn
                = WebVerseRuntime.Instance.vosSynchronizationManager.GetSynchronizer(host, port);
            if (synchronizerToGetUserTagOn == null)
            {
                LogSystem.LogError("[VOSSynchronization:GetUserTag] Can't find service to get user tag on.");
                return null;
            }

            return synchronizerToGetUserTagOn.GetUserTag(Guid.Parse(userID));
        }

        private static void InvokeCallback(string function)
        {
            WebVerseRuntime.Instance.javascriptHandler.Run(function);
        }
    }
}