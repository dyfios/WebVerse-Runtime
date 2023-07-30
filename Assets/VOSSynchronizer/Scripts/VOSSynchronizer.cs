using FiveSQD.WebVerse.WebInterface.MQTT;
using FiveSQD.WebVerse.WorldEngine.Entity;
using FiveSQD.WebVerse.WorldEngine.Synchronization;
using FiveSQD.WebVerse.WorldEngine.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using FiveSQD.WebVerse.Runtime;
using FiveSQD.WebVerse.Utilities;

namespace FiveSQD.WebVerse.VOSSynchronization
{
    /// <summary>
    /// Class for a VOS Synchronizer.
    /// </summary>
    public class VOSSynchronizer : BaseSynchronizer
    {
        /// <summary>
        /// Host address for the synchronizer.
        /// </summary>
        public string host
        {
            get
            {
                if (mqttClient == null)
                {
                    LogSystem.LogError("[VOSSynchronizer->host] Not initialized.");
                    return null;
                }
                return mqttClient.host;
            }
        }

        /// <summary>
        /// Port for the synchronizer.
        /// </summary>
        public int port
        {
            get
            {
                if (mqttClient == null)
                {
                    LogSystem.LogError("[VOSSynchronizer->port] Not initialized.");
                    return -1;
                }
                return mqttClient.port;
            }
        }

        /// <summary>
        /// Whether or not the synchronizer is connected.
        /// </summary>
        public bool isConnected { get; private set; }

        /// <summary>
        /// Interval in seconds between heartbeats.
        /// </summary>
        public float heartbeatInterval = 5;

        /// <summary>
        /// MQTT client.
        /// </summary>
        private MQTTClient mqttClient;

        /// <summary>
        /// Currently available sessions.
        /// </summary>
        private Dictionary<Guid, string> availableSessions;

        /// <summary>
        /// Currently synchronized users.
        /// </summary>
        private Dictionary<Guid, string> synchronizedUsers;

        /// <summary>
        /// Currently synchronized entities.
        /// </summary>
        private List<BaseEntity> synchronizedEntities;

        /// <summary>
        /// ID of the current client.
        /// </summary>
        private Guid? currentClientID;

        /// <summary>
        /// Tag of the current client.
        /// </summary>
        private string currentClientTag;

        /// <summary>
        /// ID of the current session.
        /// </summary>
        private Guid? currentSessionID;

        /// <summary>
        /// Action to perform upon connection.
        /// </summary>
        private Action onConnected;

        /// <summary>
        /// Action to perform upon getting state.
        /// </summary>
        private Action onGotState;

        /// <summary>
        /// Action to perform upon getting message.
        /// </summary>
        private List<Action<string, string, string>> onMessage;

        /// <summary>
        /// The current session state message.
        /// </summary>
        private VOSSynchronizationMessages.SessionMessages.SessionStateMessage currentStateMessage;

        /// <summary>
        /// Whether or not state has been requested.
        /// </summary>
        private bool requestedState = false;

        /// <summary>
        /// Initialize the synchronizer.
        /// </summary>
        /// <param name="host">Host to use.</param>
        /// <param name="port">Port to use.</param>
        /// <param name="tls">Whether or not to use TLS.</param>
        /// <param name="transport">Transport to use (tcp or ws).</param>
        public void Initialize(string host, int port, bool tls, MQTTClient.Transports transport)
        {
            if (mqttClient != null)
            {
                LogSystem.LogError("[VOSSynchronizer->Initialize] Already initialized.");
                return;
            }

            Action<MQTTClient> onConnectedAction = new Action<MQTTClient>((cl) => { OnConnected();  });
            Action<MQTTClient, byte, string> onDisconnectedAction =
                new Action<MQTTClient, byte, string>((cl, code, msg) => { OnDisconnected(code, msg); });
            Action<MQTTClient, MQTTClient.ClientState, MQTTClient.ClientState> onStateChangedAction =
                new Action<MQTTClient, MQTTClient.ClientState, MQTTClient.ClientState>((cl, oldS, newS)
                => { OnStateChanged(oldS + " => " + newS); });
            Action<MQTTClient, string> onErrorAction = new Action<MQTTClient, string>((cl, msg) => { OnError(msg); });
            mqttClient = new MQTTClient(host, port, tls, transport,
                onConnectedAction, onDisconnectedAction, onStateChangedAction, onErrorAction, "/mqtt");
            isConnected = false;
            synchronizedUsers = new Dictionary<Guid, string>();
            synchronizedEntities = new List<BaseEntity>();
            availableSessions = new Dictionary<Guid, string>();
            currentClientID = Guid.NewGuid();
            currentSessionID = null;
            currentStateMessage = null;
            onMessage = new List<Action<string, string, string>>();
            onGotState = null;
        }

        /// <summary>
        /// Terminate the synchronizer.
        /// </summary>
        public void Terminate()
        {
            ExitSession();
            Disconnect();
            mqttClient = null;
        }

        /// <summary>
        /// Time since the last heartbeat.
        /// </summary>
        private float timeSinceLastHeartbeat = 0;
        private void Update()
        {
            if (mqttClient == null || currentSessionID == null || currentClientID == null)
            {
                return;
            }

            timeSinceLastHeartbeat += Time.deltaTime;
            if (timeSinceLastHeartbeat > heartbeatInterval)
            {
                timeSinceLastHeartbeat = 0;
                SendHeartbeatMessage();
            }
        }

        /// <summary>
        /// Add a message listener.
        /// </summary>
        /// <param name="action">Action to add to the message listener list.</param>
        public void AddMessageListener(Action<string, string, string> action)
        {
            onMessage.Add(action);
        }

        /// <summary>
        /// Get the tag of a given user.
        /// </summary>
        /// <param name="userID">ID of the user to get the tag for.</param>
        /// <returns>Tag of the requested user, or null if not found.</returns>
        public string GetUserTag(Guid userID)
        {
            if (synchronizedUsers == null)
            {
                return null;
            }

            return synchronizedUsers[userID];
        }

        /// <summary>
        /// Connect the synchronizer to the bus.
        /// </summary>
        /// <param name="onConnected">Action to perform upon connection.</param>
        public void Connect(Action onConnected = null)
        {
            if (mqttClient == null)
            {
                LogSystem.LogError("[VOSSynchronizer->Connect] Not initialized.");
                return;
            }

            this.onConnected = onConnected;
            mqttClient.Connect();
        }

        /// <summary>
        /// Disconnect the synchronizer from the bus.
        /// </summary>
        public void Disconnect()
        {
            if (mqttClient == null)
            {
                LogSystem.LogError("[VOSSynchronizer->Disconnect] Not initialized.");
                return;
            }

            mqttClient.Disconnect("Ending connection.");
        }

        /// <summary>
        /// Create a session.
        /// </summary>
        /// <param name="id">ID of the session.</param>
        /// <param name="tag">Tag for the session.</param>
        public void CreateSession(Guid id, string tag)
        {
            if (mqttClient == null)
            {
                LogSystem.LogError("[VOSSynchronizer->CreateSession] Not initialized.");
                return;
            }

            Guid messageID = Guid.NewGuid();
            VOSSynchronizationMessages.SessionMessages.CreateSessionMessage
                createSessionMessage = new VOSSynchronizationMessages.SessionMessages.CreateSessionMessage(
                    messageID, currentClientID.Value, id, tag);
            mqttClient.Publish("vos/session/create", JsonConvert.SerializeObject(createSessionMessage));
        }

        /// <summary>
        /// Destroy the session that the synchronizer is in.
        /// </summary>
        public void DestroySession()
        {
            if (mqttClient == null)
            {
                LogSystem.LogError("[VOSSynchronizer->DestroySession] Not initialized.");
                return;
            }

            if (currentSessionID == null)
            {
                LogSystem.LogError("[VOSSynchronizer->DestroySession] Not in session.");
                return;
            }

            if (currentClientID == null)
            {
                LogSystem.LogError("[VOSSynchronizer->DestroySession] No client ID.");
                return;
            }

            Guid messageID = Guid.NewGuid();
            VOSSynchronizationMessages.SessionMessages.DestroySessionMessage
                destroySessionMessage = new VOSSynchronizationMessages.SessionMessages.DestroySessionMessage(
                    messageID, currentClientID.Value, currentSessionID.Value);
            mqttClient.Publish("vos/session/destroy", JsonConvert.SerializeObject(destroySessionMessage));
        }

        /// <summary>
        /// Join a session.
        /// </summary>
        /// <param name="sessionID">ID of the session to join.</param>
        /// <param name="clientTag">Tag for the client.</param>
        /// <returns>ID of the client in the session, or null if unsuccessful.</returns>
        public Guid? JoinSession(Guid sessionID, string clientTag)
        {
            if (mqttClient == null)
            {
                LogSystem.LogError("[VOSSynchronizer->JoinSession] Not initialized.");
                return null;
            }

            if (currentSessionID != null)
            {
                LogSystem.LogError("[VOSSynchronizer->JoinSession] Already in session.");
                return null;
            }

            currentSessionID = sessionID;
            Guid messageID = Guid.NewGuid();
            VOSSynchronizationMessages.SessionMessages.JoinSessionMessage
                createSessionMessage = new VOSSynchronizationMessages.SessionMessages
                .JoinSessionMessage(messageID, sessionID, currentClientID.Value, clientTag);
            mqttClient.Publish("vos/session/join", JsonConvert.SerializeObject(createSessionMessage));
            mqttClient.Subscribe("vos/status/" + sessionID.ToString() + "/#", (info) =>
                LogSystem.Log("Joined session " + currentSessionID.ToString() + "."),
                (cl, topic, topicName, msg) =>
                {
                    if (msg != null)
                    {
                        string message = System.Text.Encoding.UTF8.GetString(
                            msg.payload.data, msg.payload.offset, msg.payload.count);
                        OnMessage(msg.topic, message);
                    }
                }
            );
            return currentClientID;
        }

        /// <summary>
        /// Exit the current session.
        /// </summary>
        public void ExitSession()
        {
            if (mqttClient == null)
            {
                LogSystem.LogError("[VOSSynchronizer->ExitSession] Not initialized.");
                return;
            }

            if (currentSessionID == null)
            {
                LogSystem.LogError("[VOSSynchronizer->ExitSession] Not in session.");
                return;
            }

            if (currentClientID == null)
            {
                LogSystem.LogError("[VOSSynchronizer->ExitSession] No client ID.");
                return;
            }

            Guid messageID = Guid.NewGuid();
            VOSSynchronizationMessages.SessionMessages.LeaveSessionMessage
                exitSessionMessage = new VOSSynchronizationMessages.SessionMessages
                .LeaveSessionMessage(messageID, currentSessionID.Value, currentClientID.Value);
            mqttClient.Publish("vos/session/exit", JsonConvert.SerializeObject(exitSessionMessage));
            mqttClient.UnSubscribe("vos/status/" + currentSessionID.Value.ToString() + "/#", (info) =>
                LogSystem.Log("Exited session " + currentSessionID.Value.ToString() + ".")
            );
        }

        /// <summary>
        /// Send a heartbeat message.
        /// </summary>
        public void SendHeartbeatMessage()
        {
            if (mqttClient == null)
            {
                LogSystem.LogError("[VOSSynchronizer->SendHeartbeatMessage] Not initialized.");
                return;
            }

            if (currentSessionID == null)
            {
                LogSystem.LogError("[VOSSynchronizer->SendHeartbeatMessage] Not in session.");
                return;
            }

            if (currentClientID == null)
            {
                LogSystem.LogError("[VOSSynchronizer->SendHeartbeatMessage] No client ID.");
                return;
            }

            Guid messageID = Guid.NewGuid();
            VOSSynchronizationMessages.SessionMessages.ClientHeartbeatMessage
                clientHeartbeatMessage = new VOSSynchronizationMessages.SessionMessages
                .ClientHeartbeatMessage(messageID, currentSessionID.Value, currentClientID.Value);
            mqttClient.Publish("vos/session/heartbeat", JsonConvert.SerializeObject(clientHeartbeatMessage));
        }

        /// <summary>
        /// Send a session state request message and invoke an action when a response is received..
        /// </summary>
        /// <param name="onStateReceived">Action to invoke upon receiving a response.</param>
        public void GetSessionState(Action onStateReceived = null)
        {
            if (mqttClient == null)
            {
                LogSystem.LogError("[VOSSynchronizer->GetSessionState] Not initialized.");
                return;
            }

            if (currentSessionID == null)
            {
                LogSystem.LogError("[VOSSynchronizer->GetSessionState] Not in session.");
                return;
            }

            if (currentClientID == null)
            {
                LogSystem.LogError("[VOSSynchronizer->GetSessionState] No client ID.");
                return;
            }

            Guid messageID = Guid.NewGuid();
            VOSSynchronizationMessages.SessionMessages.GetSessionStateMessage
                getSessionStateMessage = new VOSSynchronizationMessages.SessionMessages
                .GetSessionStateMessage(messageID, currentSessionID.Value, currentClientID.Value);
            requestedState = true;
            onGotState = onStateReceived;
            mqttClient.Publish("vos/session/getstate", JsonConvert.SerializeObject(getSessionStateMessage));
        }

        /// <summary>
        /// Start synchronizing an entity in the current session.
        /// </summary>
        /// <param name="entityToSynchronize">Entity to start synchronizing.</param>
        /// <param name="deleteWithClient">Whether or not to delete the entity when the client disconnects.</param>
        /// <param name="filePath">Path to a resource for the entity.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        public override StatusCode AddSynchronizedEntity(BaseEntity entityToSynchronize, bool deleteWithClient, string filePath = null)
        {
            if (mqttClient == null)
            {
                LogSystem.LogError("[VOSSynchronizer->AddSynchronizedEntity] Not initialized.");
                return StatusCode.FAILED;
            }

            if (currentSessionID == null)
            {
                LogSystem.LogError("[VOSSynchronizer->AddSynchronizedEntity] Not in session.");
                return StatusCode.FAILED;
            }

            if (currentClientID == null)
            {
                LogSystem.LogError("[VOSSynchronizer->AddSynchronizedEntity] No client ID.");
                return StatusCode.FAILED;
            }

            if (entityToSynchronize == null)
            {
                LogSystem.LogError("[VOSSynchronizer->AddSynchronizedEntity] Invalid entity.");
                return StatusCode.FAILED;
            }

            Guid messageID = Guid.NewGuid();

            Guid? parentID = null;
            BaseEntity parentEntity = entityToSynchronize.GetParent();
            if (parentEntity != null)
            {
                parentID = parentEntity.id;
            }

            if (entityToSynchronize is MeshEntity)
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    LogSystem.LogError("[VOSSynchronizer->AddSynchronizedEntity] Invalid file path.");
                    return StatusCode.FAILED;
                }

                VOSSynchronizationMessages.RequestMessages.AddMeshEntityMessage
                    addMeshEntityMessage = new VOSSynchronizationMessages.RequestMessages
                    .AddMeshEntityMessage(messageID, currentClientID.Value, currentSessionID.Value,
                    entityToSynchronize.id, entityToSynchronize.tag, filePath, parentID,
                    entityToSynchronize.GetPosition(true), entityToSynchronize.GetRotation(true),
                    entityToSynchronize.GetScale(), false, deleteWithClient);
                mqttClient.Publish("vos/request/" + currentSessionID.Value.ToString() + "/createmeshentity",
                    JsonConvert.SerializeObject(addMeshEntityMessage));
            }
            else if (entityToSynchronize is CharacterEntity)
            {
                VOSSynchronizationMessages.RequestMessages.AddCharacterEntityMessage
                    addCharacterEntityMessage = new VOSSynchronizationMessages.RequestMessages
                    .AddCharacterEntityMessage(messageID, currentClientID.Value, currentSessionID.Value,
                    entityToSynchronize.id, entityToSynchronize.tag, parentID, filePath, entityToSynchronize.GetPosition(true),
                    entityToSynchronize.GetRotation(true), entityToSynchronize.GetScale(), false, deleteWithClient);
                mqttClient.Publish("vos/request/" + currentSessionID.Value.ToString() + "/createcharacterentity",
                    JsonConvert.SerializeObject(addCharacterEntityMessage));
            }
            else if (entityToSynchronize is ButtonEntity)
            {
                if (parentEntity is not CanvasEntity)
                {
                    Logging.LogWarning("[VOSSynchronizer->AddSynchronizedEntity] Button entity parent must be canvas entity.");
                    return StatusCode.FAILED;
                }

                VOSSynchronizationMessages.RequestMessages.AddButtonEntityMessage
                    addButtonEntityMessage = new VOSSynchronizationMessages.RequestMessages
                    .AddButtonEntityMessage(messageID, currentClientID.Value, currentSessionID.Value,
                    entityToSynchronize.id, entityToSynchronize.tag, parentID,
                    ((ButtonEntity) entityToSynchronize).GetPositionPercent(),
                    ((ButtonEntity) entityToSynchronize).GetSizePercent(), null, deleteWithClient); // TODO event.
                mqttClient.Publish("vos/request/" + currentSessionID.Value.ToString() + "/createbuttonentity",
                    JsonConvert.SerializeObject(addButtonEntityMessage));
            }
            else if (entityToSynchronize is CanvasEntity)
            {
                VOSSynchronizationMessages.RequestMessages.AddCanvasEntityMessage
                    addCanvasEntityMessage = new VOSSynchronizationMessages.RequestMessages
                    .AddCanvasEntityMessage(messageID, currentClientID.Value, currentSessionID.Value,
                    entityToSynchronize.id, entityToSynchronize.tag, parentID,
                    entityToSynchronize.GetPosition(true), entityToSynchronize.GetRotation(true),
                    entityToSynchronize.GetScale(), false, deleteWithClient);
                mqttClient.Publish("vos/request/" + currentSessionID.Value.ToString() + "/createcanvasentity",
                    JsonConvert.SerializeObject(addCanvasEntityMessage));
            }
            else if (entityToSynchronize is InputEntity)
            {
                if (parentEntity is not CanvasEntity)
                {
                    Logging.LogWarning("[VOSSynchronizer->AddSynchronizedEntity] Input entity parent must be canvas entity.");
                    return StatusCode.FAILED;
                }

                VOSSynchronizationMessages.RequestMessages.AddInputEntityMessage
                    addInputEntityMessage = new VOSSynchronizationMessages.RequestMessages
                    .AddInputEntityMessage(messageID, currentClientID.Value, currentSessionID.Value,
                    entityToSynchronize.id, entityToSynchronize.tag, parentID,
                    ((InputEntity) entityToSynchronize).GetPositionPercent(),
                    ((InputEntity) entityToSynchronize).GetSizePercent(), deleteWithClient);
                mqttClient.Publish("vos/request/" + currentSessionID.Value.ToString() + "/createinputentity",
                    JsonConvert.SerializeObject(addInputEntityMessage));
            }
            else if (entityToSynchronize is LightEntity)
            {
                VOSSynchronizationMessages.RequestMessages.AddLightEntityMessage
                    addLightEntityMessage = new VOSSynchronizationMessages.RequestMessages
                    .AddLightEntityMessage(messageID, currentClientID.Value, currentSessionID.Value,
                    entityToSynchronize.id, entityToSynchronize.tag, parentID,
                    entityToSynchronize.GetPosition(true), entityToSynchronize.GetRotation(true),
                    deleteWithClient);
                mqttClient.Publish("vos/request/" + currentSessionID.Value.ToString() + "/createlightentity",
                    JsonConvert.SerializeObject(addLightEntityMessage));
            }
            else if (entityToSynchronize is TerrainEntity)
            {
                VOSSynchronizationMessages.RequestMessages.AddTerrainEntityMessage
                    addTerrainEntityMessage = new VOSSynchronizationMessages.RequestMessages
                    .AddTerrainEntityMessage(messageID, currentClientID.Value, currentSessionID.Value,
                    entityToSynchronize.id, entityToSynchronize.tag, parentID,
                    entityToSynchronize.GetPosition(true), entityToSynchronize.GetRotation(true),
                    entityToSynchronize.GetScale(), false, 0, 0, 0,
                    ((TerrainEntity) entityToSynchronize).GetHeights(), deleteWithClient); // TODO dimensions.
                mqttClient.Publish("vos/request/" + currentSessionID.Value.ToString() + "/createterrainentity",
                    JsonConvert.SerializeObject(addTerrainEntityMessage));
            }
            else if (entityToSynchronize is TextEntity)
            {
                if (parentEntity is not CanvasEntity)
                {
                    Logging.LogWarning("[VOSSynchronizer->AddSynchronizedEntity] Text entity parent must be canvas entity.");
                    return StatusCode.FAILED;
                }

                VOSSynchronizationMessages.RequestMessages.AddTextEntityMessage
                    addTextEntityMessage = new VOSSynchronizationMessages.RequestMessages
                    .AddTextEntityMessage(messageID, currentClientID.Value, currentSessionID.Value,
                    entityToSynchronize.id, entityToSynchronize.tag, parentID,
                    ((TextEntity) entityToSynchronize).GetPositionPercent(),
                    ((TextEntity) entityToSynchronize).GetSizePercent(),
                    ((TextEntity) entityToSynchronize).GetText(),
                    ((TextEntity) entityToSynchronize).GetFontSize(), deleteWithClient);
                mqttClient.Publish("vos/request/" + currentSessionID.Value.ToString() + "/createtextentity",
                    JsonConvert.SerializeObject(addTextEntityMessage));
            }
            else if (entityToSynchronize is VoxelEntity)
            {
                VOSSynchronizationMessages.RequestMessages.AddVoxelEntityMessage
                    addVoxelEntityMessage = new VOSSynchronizationMessages.RequestMessages
                    .AddVoxelEntityMessage(messageID, currentClientID.Value, currentSessionID.Value,
                    entityToSynchronize.id, entityToSynchronize.tag, parentID,
                    entityToSynchronize.GetPosition(true), entityToSynchronize.GetRotation(true),
                    entityToSynchronize.GetScale(), deleteWithClient);
                mqttClient.Publish("vos/request/" + currentSessionID.Value.ToString() + "/createvoxelentity",
                    JsonConvert.SerializeObject(addVoxelEntityMessage));
            }
            else
            {
                Logging.LogWarning("[VOSSynchronizer->AddSynchronizedEntity] Unhandled entity type. Cannot synchronize.");
                return StatusCode.FAILED;
            }
            synchronizedEntities.Add(entityToSynchronize);
            entityToSynchronize.StartSynchronizing(this);
            return StatusCode.SUCCESS;
        }

        /// <summary>
        /// Remove an entity from the session.
        /// </summary>
        /// <param name="entityToUnSynchronize">Entity to stop synchronizing.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        public override StatusCode RemoveSynchronizedEntity(BaseEntity entityToUnSynchronize)
        {
            if (mqttClient == null)
            {
                LogSystem.LogError("[VOSSynchronizer->RemoveSynchronizedEntity] Not initialized.");
                return StatusCode.FAILED;
            }

            if (currentSessionID == null)
            {
                LogSystem.LogError("[VOSSynchronizer->RemoveSynchronizedEntity] Not in session.");
                return StatusCode.FAILED;
            }

            if (currentClientID == null)
            {
                LogSystem.LogError("[VOSSynchronizer->RemoveSynchronizedEntity] No client ID.");
                return StatusCode.FAILED;
            }

            if (entityToUnSynchronize == null)
            {
                LogSystem.LogError("[VOSSynchronizer->RemoveSynchronizedEntity] Invalid entity.");
                return StatusCode.FAILED;
            }
            Guid messageID = Guid.NewGuid();
            VOSSynchronizationMessages.RequestMessages.RemoveEntityMessage
                removeEntityMessage = new VOSSynchronizationMessages.RequestMessages
                .RemoveEntityMessage(messageID, currentClientID.Value, currentSessionID.Value,
                entityToUnSynchronize.id);
            mqttClient.Publish("vos/request/" + currentSessionID.Value.ToString()
                + "/entity/" + entityToUnSynchronize.id.ToString() + "/remove",
                JsonConvert.SerializeObject(removeEntityMessage));
            synchronizedEntities.Remove(entityToUnSynchronize);
            entityToUnSynchronize.StopSynchronizing();
            return StatusCode.SUCCESS;
        }

        /// <summary>
        /// Delete an entity that is being synchronized.
        /// </summary>
        /// <param name="entityToDelete">Entity to delete.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        public override StatusCode DeleteSynchronizedEntity(BaseEntity entityToDelete)
        {
            if (mqttClient == null)
            {
                LogSystem.LogError("[VOSSynchronizer->DeleteSynchronizedEntity] Not initialized.");
                return StatusCode.FAILED;
            }

            if (currentSessionID == null)
            {
                LogSystem.LogError("[VOSSynchronizer->DeleteSynchronizedEntity] Not in session.");
                return StatusCode.FAILED;
            }

            if (currentClientID == null)
            {
                LogSystem.LogError("[VOSSynchronizer->DeleteSynchronizedEntity] No client ID.");
                return StatusCode.FAILED;
            }

            if (entityToDelete == null)
            {
                LogSystem.LogError("[VOSSynchronizer->DeleteSynchronizedEntity] Invalid entity.");
                return StatusCode.FAILED;
            }
            Guid messageID = Guid.NewGuid();
            VOSSynchronizationMessages.RequestMessages.DeleteEntityMessage
                deleteEntityMessage = new VOSSynchronizationMessages.RequestMessages
                .DeleteEntityMessage(messageID, currentClientID.Value, currentSessionID.Value,
                entityToDelete.id);
            mqttClient.Publish("vos/request/" + currentSessionID.Value.ToString()
                + "/entity/" + entityToDelete.id.ToString() + "/delete",
                JsonConvert.SerializeObject(deleteEntityMessage));
            synchronizedEntities.Remove(entityToDelete);
            return StatusCode.SUCCESS;
        }

        /// <summary>
        /// Make a canvas entity a screen canvas.
        /// </summary>
        /// <param name="entityToSet">Canvas entity to apply this to.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        public override StatusCode MakeScreenCanvas(CanvasEntity entityToSet)
        {
            if (mqttClient == null)
            {
                LogSystem.LogError("[VOSSynchronizer->MakeScreenCanvas] Not initialized.");
                return StatusCode.FAILED;
            }

            if (currentSessionID == null)
            {
                LogSystem.LogError("[VOSSynchronizer->MakeScreenCanvas] Not in session.");
                return StatusCode.FAILED;
            }

            if (currentClientID == null)
            {
                LogSystem.LogError("[VOSSynchronizer->MakeScreenCanvas] No client ID.");
                return StatusCode.FAILED;
            }

            if (entityToSet == null)
            {
                LogSystem.LogError("[VOSSynchronizer->MakeScreenCanvas] Invalid entity.");
                return StatusCode.FAILED;
            }
            Guid messageID = Guid.NewGuid();
            VOSSynchronizationMessages.RequestMessages.SetCanvasTypeMessage
                setCanvasTypeMessage = new VOSSynchronizationMessages.RequestMessages
                .SetCanvasTypeMessage(messageID, currentClientID.Value, currentSessionID.Value,
                entityToSet.id, "screen");
            mqttClient.Publish("vos/request/" + currentSessionID.Value.ToString()
                + "/entity/" + entityToSet.id.ToString() + "/canvastype",
                JsonConvert.SerializeObject(setCanvasTypeMessage));
            return StatusCode.SUCCESS;
        }

        /// <summary>
        /// Make a canvas entity a world canvas.
        /// </summary>
        /// <param name="entityToSet">Canvas entity to apply this to.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        public override StatusCode MakeWorldCanvas(CanvasEntity entityToSet)
        {
            if (mqttClient == null)
            {
                LogSystem.LogError("[VOSSynchronizer->MakeWorldCanvas] Not initialized.");
                return StatusCode.FAILED;
            }

            if (currentSessionID == null)
            {
                LogSystem.LogError("[VOSSynchronizer->MakeWorldCanvas] Not in session.");
                return StatusCode.FAILED;
            }

            if (currentClientID == null)
            {
                LogSystem.LogError("[VOSSynchronizer->MakeWorldCanvas] No client ID.");
                return StatusCode.FAILED;
            }

            if (entityToSet == null)
            {
                LogSystem.LogError("[VOSSynchronizer->MakeWorldCanvas] Invalid entity.");
                return StatusCode.FAILED;
            }
            Guid messageID = Guid.NewGuid();
            VOSSynchronizationMessages.RequestMessages.SetCanvasTypeMessage
                setCanvasTypeMessage = new VOSSynchronizationMessages.RequestMessages
                .SetCanvasTypeMessage(messageID, currentClientID.Value, currentSessionID.Value,
                entityToSet.id, "world");
            mqttClient.Publish("vos/request/" + currentSessionID.Value.ToString()
                + "/entity/" + entityToSet.id.ToString() + "/canvastype",
                JsonConvert.SerializeObject(setCanvasTypeMessage));
            return StatusCode.SUCCESS;
        }

        /// <summary>
        /// Set the highlight state of an entity.
        /// </summary>
        /// <param name="entityToSet">Entity to set the highlight state of.</param>
        /// <param name="highlight">Whether or not to enable highlighting.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        public override StatusCode SetHighlight(BaseEntity entityToSet, bool highlight)
        {
            if (mqttClient == null)
            {
                LogSystem.LogError("[VOSSynchronizer->SetHighlight] Not initialized.");
                return StatusCode.FAILED;
            }

            if (currentSessionID == null)
            {
                LogSystem.LogError("[VOSSynchronizer->SetHighlight] Not in session.");
                return StatusCode.FAILED;
            }

            if (currentClientID == null)
            {
                LogSystem.LogError("[VOSSynchronizer->SetHighlight] No client ID.");
                return StatusCode.FAILED;
            }

            if (entityToSet == null)
            {
                LogSystem.LogError("[VOSSynchronizer->SetHighlight] Invalid entity.");
                return StatusCode.FAILED;
            }
            Guid messageID = Guid.NewGuid();
            VOSSynchronizationMessages.RequestMessages.SetHighlightStateMessage
                setHighlightStateMessage = new VOSSynchronizationMessages.RequestMessages
                .SetHighlightStateMessage(messageID, currentClientID.Value, currentSessionID.Value,
                entityToSet.id, highlight);
            mqttClient.Publish("vos/request/" + currentSessionID.Value.ToString()
                + "/entity/" + entityToSet.id.ToString() + "/highlight",
                JsonConvert.SerializeObject(setHighlightStateMessage));
            return StatusCode.SUCCESS;
        }

        /// <summary>
        /// Set the interaction state of an entity.
        /// </summary>
        /// <param name="entityToSet">Entity to set the interaction state of.</param>
        /// <param name="state">State to apply.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        public override StatusCode SetInteractionState(BaseEntity entityToSet, BaseEntity.InteractionState? state)
        {
            if (mqttClient == null)
            {
                LogSystem.LogError("[VOSSynchronizer->SetInteractionState] Not initialized.");
                return StatusCode.FAILED;
            }

            if (currentSessionID == null)
            {
                LogSystem.LogError("[VOSSynchronizer->SetInteractionState] Not in session.");
                return StatusCode.FAILED;
            }

            if (currentClientID == null)
            {
                LogSystem.LogError("[VOSSynchronizer->SetInteractionState] No client ID.");
                return StatusCode.FAILED;
            }

            if (entityToSet == null)
            {
                LogSystem.LogError("[VOSSynchronizer->SetInteractionState] Invalid entity.");
                return StatusCode.FAILED;
            }
            Guid messageID = Guid.NewGuid();
            VOSSynchronizationMessages.RequestMessages.SetInteractionStateMessage
                setInteractionStateMessage = new VOSSynchronizationMessages.RequestMessages
                .SetInteractionStateMessage(messageID, currentClientID.Value, currentSessionID.Value,
                entityToSet.id, state.HasValue ? state.Value.ToString().ToLower() : "static");
            mqttClient.Publish("vos/request/" + currentSessionID.Value.ToString()
                + "/entity/" + entityToSet.id.ToString() + "/interactionstate",
                JsonConvert.SerializeObject(setInteractionStateMessage));
            return StatusCode.SUCCESS;
        }

        /// <summary>
        /// Set the motion state of an entity.
        /// </summary>
        /// <param name="entityToSet">Entity to set the motion state of.</param>
        /// <param name="motion">Motion state to set.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        public override StatusCode SetMotion(BaseEntity entityToSet, BaseEntity.EntityMotion? motion)
        {
            if (mqttClient == null)
            {
                LogSystem.LogError("[VOSSynchronizer->SetMotion] Not initialized.");
                return StatusCode.FAILED;
            }

            if (currentSessionID == null)
            {
                LogSystem.LogError("[VOSSynchronizer->SetMotion] Not in session.");
                return StatusCode.FAILED;
            }

            if (currentClientID == null)
            {
                LogSystem.LogError("[VOSSynchronizer->SetMotion] No client ID.");
                return StatusCode.FAILED;
            }

            if (entityToSet == null)
            {
                LogSystem.LogError("[VOSSynchronizer->SetMotion] Invalid entity.");
                return StatusCode.FAILED;
            }

            Vector3 angularVelocity = Vector3.zero;
            Vector3 velocity = Vector3.zero;
            bool stationary = true;
            if (motion.HasValue)
            {
                angularVelocity = motion.Value.angularVelocity;
                velocity = motion.Value.velocity;
                stationary = motion.Value.stationary.HasValue ? motion.Value.stationary.Value : true;
            }

            Guid messageID = Guid.NewGuid();
            VOSSynchronizationMessages.RequestMessages.SetMotionMessage
                setMotionMessage = new VOSSynchronizationMessages.RequestMessages
                .SetMotionMessage(messageID, currentClientID.Value, currentSessionID.Value,
                entityToSet.id, angularVelocity, velocity, stationary);
            mqttClient.Publish("vos/request/" + currentSessionID.Value.ToString()
                + "/entity/" + entityToSet.id.ToString() + "/motion",
                JsonConvert.SerializeObject(setMotionMessage));
            return StatusCode.SUCCESS;
        }

        /// <summary>
        /// Set the parent of an entity.
        /// </summary>
        /// <param name="entityToSet">Entity to set the parent of.</param>
        /// <param name="parentToSet">Parent to set.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        public override StatusCode SetParent(BaseEntity entityToSet, BaseEntity parentToSet)
        {
            if (mqttClient == null)
            {
                LogSystem.LogError("[VOSSynchronizer->SetParent] Not initialized.");
                return StatusCode.FAILED;
            }

            if (currentSessionID == null)
            {
                LogSystem.LogError("[VOSSynchronizer->SetParent] Not in session.");
                return StatusCode.FAILED;
            }

            if (currentClientID == null)
            {
                LogSystem.LogError("[VOSSynchronizer->SetParent] No client ID.");
                return StatusCode.FAILED;
            }

            if (entityToSet == null)
            {
                LogSystem.LogError("[VOSSynchronizer->SetParent] Invalid entity.");
                return StatusCode.FAILED;
            }

            Guid messageID = Guid.NewGuid();
            VOSSynchronizationMessages.RequestMessages.SetParentMessage
                setParentMessage = new VOSSynchronizationMessages.RequestMessages
                .SetParentMessage(messageID, currentClientID.Value, currentSessionID.Value,
                entityToSet.id, parentToSet.id);
            mqttClient.Publish("vos/request/" + currentSessionID.Value.ToString()
                + "/entity/" + entityToSet.id.ToString() + "/parent",
                JsonConvert.SerializeObject(setParentMessage));
            return StatusCode.SUCCESS;
        }

        /// <summary>
        /// Set the physical properties of an entity.
        /// </summary>
        /// <param name="entityToSet">Entity to set the physical properties of.</param>
        /// <param name="properties">Properties to apply.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        public override StatusCode SetPhysicalProperties(BaseEntity entityToSet, BaseEntity.EntityPhysicalProperties? properties)
        {
            if (mqttClient == null)
            {
                LogSystem.LogError("[VOSSynchronizer->SetMotion] Not initialized.");
                return StatusCode.FAILED;
            }

            if (currentSessionID == null)
            {
                LogSystem.LogError("[VOSSynchronizer->SetMotion] Not in session.");
                return StatusCode.FAILED;
            }

            if (currentClientID == null)
            {
                LogSystem.LogError("[VOSSynchronizer->SetMotion] No client ID.");
                return StatusCode.FAILED;
            }

            if (entityToSet == null)
            {
                LogSystem.LogError("[VOSSynchronizer->SetMotion] Invalid entity.");
                return StatusCode.FAILED;
            }

            float angularDrag = 0;
            Vector3 centerOfMass = Vector3.zero;
            float drag = 0;
            bool gravitational = false;
            float mass = 0;
            if (properties.HasValue)
            {
                angularDrag = properties.Value.angularDrag.HasValue ? properties.Value.angularDrag.Value : 0;
                centerOfMass = properties.Value.centerOfMass;
                drag = properties.Value.drag.HasValue ? properties.Value.drag.Value : 0;
                gravitational = properties.Value.gravitational.HasValue ? properties.Value.gravitational.Value : false;
            }

            Guid messageID = Guid.NewGuid();
            VOSSynchronizationMessages.RequestMessages.SetPhysicalPropertiesMessage
                setPhysicalPropertiesMessage = new VOSSynchronizationMessages.RequestMessages
                .SetPhysicalPropertiesMessage(messageID, currentClientID.Value, currentSessionID.Value,
                entityToSet.id, angularDrag, centerOfMass, drag, gravitational, mass);
            mqttClient.Publish("vos/request/" + currentSessionID.Value.ToString()
                + "/entity/" + entityToSet.id.ToString() + "/physicalproperties",
                JsonConvert.SerializeObject(setPhysicalPropertiesMessage));
            return StatusCode.SUCCESS;
        }

        /// <summary>
        /// Set the position of an entity.
        /// </summary>
        /// <param name="entityToSet">Entity to set the position of.</param>
        /// <param name="position">Position to apply.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        public override StatusCode SetPosition(BaseEntity entityToSet, Vector3 position)
        {
            if (mqttClient == null)
            {
                LogSystem.LogError("[VOSSynchronizer->UpdateSynchronizedEntityPosition] Not initialized.");
                return StatusCode.FAILED;
            }

            if (currentSessionID == null)
            {
                LogSystem.LogError("[VOSSynchronizer->UpdateSynchronizedEntityPosition] Not in session.");
                return StatusCode.FAILED;
            }

            if (currentClientID == null)
            {
                LogSystem.LogError("[VOSSynchronizer->UpdateSynchronizedEntityPosition] No client ID.");
                return StatusCode.FAILED;
            }

            if (entityToSet == null)
            {
                LogSystem.LogError("[VOSSynchronizer->UpdateSynchronizedEntityPosition] Invalid entity.");
                return StatusCode.FAILED;
            }
            Guid messageID = Guid.NewGuid();
            VOSSynchronizationMessages.RequestMessages.UpdateEntityPositionMessage
                updateEntityPositionMessage = new VOSSynchronizationMessages.RequestMessages
                .UpdateEntityPositionMessage(messageID, currentClientID.Value, currentSessionID.Value,
                entityToSet.id, position);
            mqttClient.Publish("vos/request/" + currentSessionID.Value.ToString()
                + "/entity/" + entityToSet.id.ToString() + "/position",
                JsonConvert.SerializeObject(updateEntityPositionMessage));
            return StatusCode.SUCCESS;
        }

        /// <summary>
        /// Set the rotation of an entity.
        /// </summary>
        /// <param name="entityToSet">Entity to set the rotation of.</param>
        /// <param name="rotation">Rotation to apply.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        public override StatusCode SetRotation(BaseEntity entityToSet, Quaternion rotation)
        {
            if (mqttClient == null)
            {
                LogSystem.LogError("[VOSSynchronizer->UpdateSynchronizedEntityRotation] Not initialized.");
                return StatusCode.FAILED;
            }

            if (currentSessionID == null)
            {
                LogSystem.LogError("[VOSSynchronizer->UpdateSynchronizedEntityRotation] Not in session.");
                return StatusCode.FAILED;
            }

            if (currentClientID == null)
            {
                LogSystem.LogError("[VOSSynchronizer->UpdateSynchronizedEntityRotation] No client ID.");
                return StatusCode.FAILED;
            }

            if (entityToSet == null)
            {
                LogSystem.LogError("[VOSSynchronizer->UpdateSynchronizedEntityRotation] Invalid entity.");
                return StatusCode.FAILED;
            }
            Guid messageID = Guid.NewGuid();
            VOSSynchronizationMessages.RequestMessages.UpdateEntityRotationMessage
                updateEntityRotationMessage = new VOSSynchronizationMessages.RequestMessages
                .UpdateEntityRotationMessage(messageID, currentClientID.Value, currentSessionID.Value,
                entityToSet.id, rotation);
            mqttClient.Publish("vos/request/" + currentSessionID.Value.ToString()
                + "/entity/" + entityToSet.id.ToString() + "/rotation",
                JsonConvert.SerializeObject(updateEntityRotationMessage));
            return StatusCode.SUCCESS;
        }

        /// <summary>
        /// Set the scale of an entity.
        /// </summary>
        /// <param name="entityToSet">Entity to set the scale of.</param>
        /// <param name="scale">Scale to apply.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        public override StatusCode SetScale(BaseEntity entityToSet, Vector3 scale)
        {
            if (mqttClient == null)
            {
                LogSystem.LogError("[VOSSynchronizer->UpdateSynchronizedEntityScale] Not initialized.");
                return StatusCode.FAILED;
            }

            if (currentSessionID == null)
            {
                LogSystem.LogError("[VOSSynchronizer->UpdateSynchronizedEntityScale] Not in session.");
                return StatusCode.FAILED;
            }

            if (currentClientID == null)
            {
                LogSystem.LogError("[VOSSynchronizer->UpdateSynchronizedEntityScale] No client ID.");
                return StatusCode.FAILED;
            }

            if (entityToSet == null)
            {
                LogSystem.LogError("[VOSSynchronizer->UpdateSynchronizedEntityScale] Invalid entity.");
                return StatusCode.FAILED;
            }
            Guid messageID = Guid.NewGuid();
            VOSSynchronizationMessages.RequestMessages.UpdateEntityScaleMessage
                updateEntityScaleMessage = new VOSSynchronizationMessages.RequestMessages
                .UpdateEntityScaleMessage(messageID, currentClientID.Value, currentSessionID.Value,
                entityToSet.id, scale);
            mqttClient.Publish("vos/request/" + currentSessionID.Value.ToString()
                + "/entity/" + entityToSet.id.ToString() + "/scale",
                JsonConvert.SerializeObject(updateEntityScaleMessage));
            return StatusCode.SUCCESS;
        }

        /// <summary>
        /// Set the size of an entity.
        /// </summary>
        /// <param name="entityToSet">Entity to set the size of.</param>
        /// <param name="size">Size to apply.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        public override StatusCode SetSize(BaseEntity entityToSet, Vector3 size)
        {
            if (mqttClient == null)
            {
                LogSystem.LogError("[VOSSynchronizer->UpdateSynchronizedEntitySize] Not initialized.");
                return StatusCode.FAILED;
            }

            if (currentSessionID == null)
            {
                LogSystem.LogError("[VOSSynchronizer->UpdateSynchronizedEntitySize] Not in session.");
                return StatusCode.FAILED;
            }

            if (currentClientID == null)
            {
                LogSystem.LogError("[VOSSynchronizer->UpdateSynchronizedEntitySize] No client ID.");
                return StatusCode.FAILED;
            }

            if (entityToSet == null)
            {
                LogSystem.LogError("[VOSSynchronizer->UpdateSynchronizedEntitySize] Invalid entity.");
                return StatusCode.FAILED;
            }
            Guid messageID = Guid.NewGuid();
            VOSSynchronizationMessages.RequestMessages.UpdateEntitySizeMessage
                updateEntitySizeMessage = new VOSSynchronizationMessages.RequestMessages
                .UpdateEntitySizeMessage(messageID, currentClientID.Value, currentSessionID.Value,
                entityToSet.id, size);
            mqttClient.Publish("vos/request/" + currentSessionID.Value.ToString()
                + "/entity/" + entityToSet.id.ToString() + "/size",
                JsonConvert.SerializeObject(updateEntitySizeMessage));
            return StatusCode.SUCCESS;
        }

        /// <summary>
        /// Set the visibility of an entity.
        /// </summary>
        /// <param name="entityToSet">Entity to set the visibility of.</param>
        /// <param name="visible">Visibility to apply.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        public override StatusCode SetVisibility(BaseEntity entityToSet, bool visible)
        {
            if (mqttClient == null)
            {
                LogSystem.LogError("[VOSSynchronizer->SetVisibility] Not initialized.");
                return StatusCode.FAILED;
            }

            if (currentSessionID == null)
            {
                LogSystem.LogError("[VOSSynchronizer->SetVisibility] Not in session.");
                return StatusCode.FAILED;
            }

            if (currentClientID == null)
            {
                LogSystem.LogError("[VOSSynchronizer->SetVisibility] No client ID.");
                return StatusCode.FAILED;
            }

            if (entityToSet == null)
            {
                LogSystem.LogError("[VOSSynchronizer->SetVisibility] Invalid entity.");
                return StatusCode.FAILED;
            }
            Guid messageID = Guid.NewGuid();
            VOSSynchronizationMessages.RequestMessages.SetVisibilityMessage
                setVisibilityMessage = new VOSSynchronizationMessages.RequestMessages
                .SetVisibilityMessage(messageID, currentClientID.Value, currentSessionID.Value,
                entityToSet.id, visible);
            mqttClient.Publish("vos/request/" + currentSessionID.Value.ToString()
                + "/entity/" + entityToSet.id.ToString() + "/visibility",
                JsonConvert.SerializeObject(setVisibilityMessage));
            return StatusCode.SUCCESS;
        }

        /// <summary>
        /// Send a message on a synchronizer.
        /// </summary>
        /// <param name="topic">Topic of the message to send.</param>
        /// <param name="message">Message to send.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        public override StatusCode SendMessage(string topic, string message)
        {
            if (mqttClient == null)
            {
                LogSystem.LogError("[VOSSynchronizer->SendMessage] Not initialized.");
                return StatusCode.FAILED;
            }

            if (currentSessionID == null)
            {
                LogSystem.LogError("[VOSSynchronizer->SendMessage] Not in session.");
                return StatusCode.FAILED;
            }

            if (currentClientID == null)
            {
                LogSystem.LogError("[VOSSynchronizer->SendMessage] No client ID.");
                return StatusCode.FAILED;
            }
            Guid messageID = Guid.NewGuid();
            VOSSynchronizationMessages.RequestMessages.PublishMessageMessage
                publishMessageMessage = new VOSSynchronizationMessages.RequestMessages
                .PublishMessageMessage(messageID, currentClientID.Value, currentSessionID.Value,
                topic, message);
            mqttClient.Publish("vos/request/" + currentSessionID.Value.ToString()
                + "/message/create",
                JsonConvert.SerializeObject(publishMessageMessage));
            return StatusCode.SUCCESS;
        }

        /// <summary>
        /// Called when synchronizer is connected.
        /// </summary>
        private void OnConnected()
        {
            if (mqttClient == null)
            {
                LogSystem.LogError("[VOSSynchronizer->OnConnected] Not initialized.");
                return;
            }
            LogSystem.Log("[VOSSynchronizer] Connected on "
                + mqttClient.host + ":" + mqttClient.port + ".");
            mqttClient.Subscribe("vos/session/#", (info) =>
            {
                LogSystem.Log("[VOSSynchronizer] Subscribed to vos/session/#.");
                isConnected = true;
            },
            (cl, topic, topicName, msg) =>
            {
                if (msg != null)
                {
                    string message = System.Text.Encoding.UTF8.GetString(
                        msg.payload.data, msg.payload.offset, msg.payload.count);
                    OnMessage(msg.topic, message);
                }
            });
            if (onConnected != null)
            {
                onConnected.Invoke();
                onConnected = null;
            }
        }

        /// <summary>
        /// Called when synchronizer is disconnected.
        /// </summary>
        /// <param name="code">Code for disconnection.</param>
        /// <param name="reason">Reason for disconnection.</param>
        private void OnDisconnected(byte code, string reason)
        {
            if (mqttClient == null)
            {
                LogSystem.LogError("[VOSSynchronizer->OnDisconnected] Not initialized.");
                return;
            }
            LogSystem.Log("[VOSSynchronizer] Disconnected from "
                + mqttClient.host + ":" + mqttClient.port
                + " - " + reason);
            isConnected = false;
        }

        /// <summary>
        /// Called when the connection state of the synchronizer has changed.
        /// </summary>
        /// <param name="info"></param>
        private void OnStateChanged(string info)
        {
            if (mqttClient == null)
            {
                LogSystem.LogError("[VOSSynchronizer->OnStateChanged] Not initialized.");
                return;
            }
            LogSystem.Log("[VOSSynchronizer] " + mqttClient.host
                + ":" + mqttClient.port + " state changed - "
                + info);
        }

        /// <summary>
        /// Called when a connection error occurs with the synchronizer.
        /// </summary>
        /// <param name="info">Information about the error.</param>
        private void OnError(string info)
        {
            if (mqttClient == null)
            {
                LogSystem.LogError("[VOSSynchronizer->OnError] Not intialized.");
                return;
            }
            LogSystem.LogError("[VOSSynchronizer] Error: " + info);
        }

        /// <summary>
        /// Called to process a message.
        /// </summary>
        /// <param name="topic">Topic of the message.</param>
        /// <param name="message">Message.</param>
        private void OnMessage(string topic, string message)
        {
            if (topic == "vos/session/new")
            {
                VOSSynchronizationMessages.SessionMessages.NewSessionMessage
                    newSessionMessage = JsonConvert.DeserializeObject<
                        VOSSynchronizationMessages.SessionMessages.NewSessionMessage>(message);
                if (availableSessions.ContainsKey(Guid.Parse(newSessionMessage.sessionID)))
                {
                    LogSystem.LogWarning("[VOSSynchronizer->OnMessage] Skipping new session "
                        + newSessionMessage.sessionID + ", already saved.");
                }
                else
                {
                    availableSessions.Add(Guid.Parse(newSessionMessage.sessionID),
                        newSessionMessage.sessionTag);
                }
            }
            else if (topic == "vos/session/closed")
            {
                VOSSynchronizationMessages.SessionMessages.ClosedSessionMessage
                    closedSessionMessage = JsonConvert.DeserializeObject<
                        VOSSynchronizationMessages.SessionMessages.ClosedSessionMessage>(message);
                if (!availableSessions.ContainsKey(Guid.Parse(closedSessionMessage.sessionID)))
                {
                    LogSystem.LogWarning("[VOSSynchronizer->OnMessage] Skipping close session "
                        + closedSessionMessage.sessionID + ", already closed.");
                }
                else
                {
                    availableSessions.Remove(Guid.Parse(closedSessionMessage.sessionID));
                }
            }
            else if (topic == "vos/session/state")
            {
                VOSSynchronizationMessages.SessionMessages.SessionStateMessage
                    sessionStateMessage = JsonConvert.DeserializeObject<
                        VOSSynchronizationMessages.SessionMessages.SessionStateMessage>(message);
                if (sessionStateMessage.sessionID == currentSessionID.ToString())
                {
                    if (requestedState)
                    {
                        foreach (BaseEntity entity in synchronizedEntities)
                        {
                            DeleteSynchronizedEntity(entity);
                        }

                        synchronizedUsers = new Dictionary<Guid, string>();
                        foreach (VOSSynchronizationMessages.ClientInfo clientInfo in sessionStateMessage.clients)
                        {
                            synchronizedUsers.Add(clientInfo.uuid, clientInfo.tag);
                        }

                        synchronizedEntities = new List<BaseEntity>();
                        foreach (VOSSynchronizationMessages.EntityInfo entityInfo in sessionStateMessage.entities)
                        {
                            BaseEntity newEntity = AddEntity(entityInfo);
                            synchronizedEntities.Add(newEntity);
                        }
                    }
                }
            }
            else if (topic.StartsWith("vos/status/"))
            {
                if (topic == "vos/status/" + currentSessionID.ToString() + "/newclient")
                {
                    VOSSynchronizationMessages.SessionMessages.NewClientMessage
                        newClientMessage = JsonConvert.DeserializeObject<
                        VOSSynchronizationMessages.SessionMessages.NewClientMessage>(message);
                    synchronizedUsers.Add(Guid.Parse(newClientMessage.clientID), newClientMessage.clientTag);
                    Debug.Log(newClientMessage.clientID);
                }
                else if (topic == "vos/status/" + currentSessionID.ToString() + "/clientleft")
                {
                    VOSSynchronizationMessages.SessionMessages.ClientLeftMessage
                        clientLeftMessage = JsonConvert.DeserializeObject<
                        VOSSynchronizationMessages.SessionMessages.ClientLeftMessage>(message);
                    synchronizedUsers.Remove(Guid.Parse(clientLeftMessage.clientID));
                    Debug.Log(clientLeftMessage.clientID);
                }
                else if (topic == "vos/status/" + currentSessionID.ToString() + "/createcharacterentity")
                {
                    VOSSynchronizationMessages.StatusMessages.AddCharacterEntityMessage
                        addCharacterEntityMessage = JsonConvert.DeserializeObject<
                            VOSSynchronizationMessages.StatusMessages.AddCharacterEntityMessage>(message);

                    BaseEntity parent = null;
                    if (!string.IsNullOrEmpty(addCharacterEntityMessage.parentID))
                    {
                        parent = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(
                            Guid.Parse(addCharacterEntityMessage.parentID));
                    }

                    bool isSize = false;
                    Vector3 scaleSize = Vector3.zero;
                    if (addCharacterEntityMessage.scale != null)
                    {
                        isSize = false;
                        scaleSize = addCharacterEntityMessage.scale.ToVector3();
                    }
                    else if (addCharacterEntityMessage.size != null)
                    {
                        isSize = true;
                        scaleSize = addCharacterEntityMessage.size.ToVector3();
                    }
                    else
                    {
                        LogSystem.LogWarning("[VOSSynchronizer->OnMessage] Invalid " +
                            "createcharacterentity message.");
                    }
                    if (addCharacterEntityMessage.clientID == currentClientID.ToString())
                    {
                        return;
                    }
                    if (addCharacterEntityMessage.id != null &&
                        WorldEngine.WorldEngine.ActiveWorld.entityManager.Exists(Guid.Parse(addCharacterEntityMessage.id)))
                    {
                        LogSystem.LogWarning("[VOSSynchronizer->OnMessage] Entity " +
                            "already exists.");
                        return;
                    }
                    WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadCharacterEntity(parent, addCharacterEntityMessage.position.ToVector3(),
                        addCharacterEntityMessage.rotation.ToQuaternion(), scaleSize,
                        Guid.Parse(addCharacterEntityMessage.id), tag, isSize);
                    BaseEntity ce = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(Guid.Parse(addCharacterEntityMessage.id));
                    if (ce == null)
                    {
                        LogSystem.LogWarning("[VOSSynchronizer->OnMessage] Could not find entity.");
                    }
                    else
                    {
                        ce.SetVisibility(true);
                    }
                }
                else if (topic == "vos/status/" + currentSessionID.ToString() + "/createmeshentity")
                {
                    VOSSynchronizationMessages.StatusMessages.AddMeshEntityMessage
                        addMeshEntityMessage = JsonConvert.DeserializeObject<
                        VOSSynchronizationMessages.StatusMessages.AddMeshEntityMessage>(message);
                    bool isSize = false;
                    Vector3 scaleSize = Vector3.zero;
                    if (addMeshEntityMessage.scale != null)
                    {
                        isSize = false;
                        scaleSize = addMeshEntityMessage.scale.ToVector3();
                    }
                    else if (addMeshEntityMessage.size != null)
                    {
                        isSize = true;
                        scaleSize = addMeshEntityMessage.size.ToVector3();
                    }
                    else
                    {
                        LogSystem.LogWarning("[VOSSynchronizer->OnMessage] Invalid " +
                            "createmeshentity message.");
                    }
                    WebVerseRuntime.Instance.gltfHandler.LoadGLTFResourceAsMeshEntity(addMeshEntityMessage.path, Guid.Parse(addMeshEntityMessage.parentID));
                    BaseEntity me = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(Guid.Parse(addMeshEntityMessage.id));
                    if (me == null)
                    {
                        LogSystem.LogWarning("[VOSSynchronizer->OnMessage] Could not find entity.");
                    }
                    else
                    {
                        me.SetVisibility(true);
                        me.SetPosition(addMeshEntityMessage.position.ToVector3(), false, false);
                        me.SetRotation(addMeshEntityMessage.rotation.ToQuaternion(), false, false);
                        if (isSize)
                        {
                            me.SetSize(scaleSize, false);
                        }
                        else
                        {
                            me.SetScale(scaleSize, false);
                        }
                        me.tag = addMeshEntityMessage.tag;
                    }
                }
                else if (topic == "vos/status/" + currentSessionID.ToString() + "/createbuttonentity")
                {
                    VOSSynchronizationMessages.StatusMessages.AddButtonEntityMessage
                        addButtonEntityMessage = JsonConvert.DeserializeObject<
                        VOSSynchronizationMessages.StatusMessages.AddButtonEntityMessage>(message);

                    CanvasEntity parent = null;
                    if (!string.IsNullOrEmpty(addButtonEntityMessage.parentID))
                    {
                        BaseEntity foundEntity = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(
                            Guid.Parse(addButtonEntityMessage.parentID));
                        if (foundEntity is CanvasEntity)
                        {
                            parent = (CanvasEntity) foundEntity;
                        }
                        else
                        {
                            LogSystem.LogWarning("[VOSSynchronizer->OnMessage] Parent is not a canvas entity.");
                        }
                    }

                    Action onClickAction = null;
                    if (addButtonEntityMessage.onClick != null)
                    {
                        onClickAction = new Action(() =>
                        {
                            WebVerseRuntime.Instance.javascriptHandler.Run(addButtonEntityMessage.onClick);
                        });
                    }

                    WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadButtonEntity(parent,
                        addButtonEntityMessage.positionPercent.ToVector2(), addButtonEntityMessage.sizePercent.ToVector2(),
                        onClickAction, Guid.Parse(addButtonEntityMessage.id), null);
                    BaseEntity be = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(Guid.Parse(addButtonEntityMessage.id));
                    if (be == null)
                    {
                        LogSystem.LogWarning("[VOSSynchronizer->OnMessage] Could not find entity.");
                    }
                    else
                    {
                        be.SetVisibility(true);
                        be.tag = addButtonEntityMessage.tag;
                    }
                }
                else if (topic == "vos/status/" + currentSessionID.ToString() + "/createcanvasentity")
                {
                    VOSSynchronizationMessages.StatusMessages.AddCanvasEntityMessage
                        addCanvasEntityMessage = JsonConvert.DeserializeObject<
                        VOSSynchronizationMessages.StatusMessages.AddCanvasEntityMessage>(message);

                    BaseEntity parent = null;
                    if (!string.IsNullOrEmpty(addCanvasEntityMessage.parentID))
                    {
                        parent = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(
                            Guid.Parse(addCanvasEntityMessage.parentID));
                    }

                    bool isSize = false;
                    Vector3 scaleSize = Vector3.zero;
                    if (addCanvasEntityMessage.scale != null)
                    {
                        isSize = false;
                        scaleSize = addCanvasEntityMessage.scale.ToVector3();
                    }
                    else if (addCanvasEntityMessage.size != null)
                    {
                        isSize = true;
                        scaleSize = addCanvasEntityMessage.size.ToVector3();
                    }
                    else
                    {
                        LogSystem.LogWarning("[VOSSynchronizer->OnMessage] Invalid " +
                            "createcanvasentity message.");
                    }

                    WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadCanvasEntity(parent,
                        addCanvasEntityMessage.position.ToVector3(), addCanvasEntityMessage.rotation.ToQuaternion(),
                        scaleSize, Guid.Parse(addCanvasEntityMessage.id), isSize, null);
                    BaseEntity ce = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(Guid.Parse(addCanvasEntityMessage.id));
                    if (ce == null)
                    {
                        LogSystem.LogWarning("[VOSSynchronizer->OnMessage] Could not find entity.");
                    }
                    else
                    {
                        ce.SetVisibility(true);
                        ce.tag = addCanvasEntityMessage.tag;
                    }
                }
                else if (topic == "vos/status/" + currentSessionID.ToString() + "/createinputentity")
                {
                    VOSSynchronizationMessages.StatusMessages.AddInputEntityMessage
                        addInputEntityMessage = JsonConvert.DeserializeObject<
                        VOSSynchronizationMessages.StatusMessages.AddInputEntityMessage>(message);

                    CanvasEntity parent = null;
                    if (!string.IsNullOrEmpty(addInputEntityMessage.parentID))
                    {
                        BaseEntity foundEntity = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(
                            Guid.Parse(addInputEntityMessage.parentID));
                        if (foundEntity is CanvasEntity)
                        {
                            parent = (CanvasEntity) foundEntity;
                        }
                        else
                        {
                            LogSystem.LogWarning("[VOSSynchronizer->OnMessage] Parent is not a canvas entity.");
                        }
                    }

                    WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadInputEntity(parent,
                        addInputEntityMessage.positionPercent.ToVector2(), addInputEntityMessage.sizePercent.ToVector2(),
                        Guid.Parse(addInputEntityMessage.id), null);
                    BaseEntity be = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(Guid.Parse(addInputEntityMessage.id));
                    if (be == null)
                    {
                        LogSystem.LogWarning("[VOSSynchronizer->OnMessage] Could not find entity.");
                    }
                    else
                    {
                        be.SetVisibility(true);
                        be.tag = addInputEntityMessage.tag;
                    }
                }
                else if (topic == "vos/status/" + currentSessionID.ToString() + "/createlightentity")
                {
                    VOSSynchronizationMessages.StatusMessages.AddLightEntityMessage
                        addLightEntityMessage = JsonConvert.DeserializeObject<
                        VOSSynchronizationMessages.StatusMessages.AddLightEntityMessage>(message);

                    BaseEntity parentEntity = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(
                            Guid.Parse(addLightEntityMessage.parentID));

                    WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadLightEntity(parentEntity,
                        addLightEntityMessage.position.ToVector3(), addLightEntityMessage.rotation.ToQuaternion(), null);
                    BaseEntity me = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(Guid.Parse(addLightEntityMessage.id));
                    if (me == null)
                    {
                        LogSystem.LogWarning("[VOSSynchronizer->OnMessage] Could not find entity.");
                    }
                    else
                    {
                        me.SetVisibility(true);
                        me.tag = addLightEntityMessage.tag;
                    }
                }
                else if (topic == "vos/status/" + currentSessionID.ToString() + "/createterrainentity")
                {
                    VOSSynchronizationMessages.StatusMessages.AddTerrainEntityMessage
                        addTerrainEntityMessage = JsonConvert.DeserializeObject<
                        VOSSynchronizationMessages.StatusMessages.AddTerrainEntityMessage>(message);
                    bool isSize = false;
                    Vector3 scaleSize = Vector3.zero;
                    if (addTerrainEntityMessage.scale != null)
                    {
                        isSize = false;
                        scaleSize = addTerrainEntityMessage.scale.ToVector3();
                    }
                    else if (addTerrainEntityMessage.size != null)
                    {
                        isSize = true;
                        scaleSize = addTerrainEntityMessage.size.ToVector3();
                    }
                    else
                    {
                        LogSystem.LogWarning("[VOSSynchronizer->OnMessage] Invalid " +
                            "createterrainentity message.");
                    }

                    BaseEntity parentEntity = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(
                            Guid.Parse(addTerrainEntityMessage.parentID));

                    WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadTerrainEntity(addTerrainEntityMessage.length,
                        addTerrainEntityMessage.width, addTerrainEntityMessage.height, addTerrainEntityMessage.heights,
                        parentEntity, addTerrainEntityMessage.position.ToVector3(), addTerrainEntityMessage.rotation.ToQuaternion(),
                        addTerrainEntityMessage.scale.ToVector3(), Guid.Parse(addTerrainEntityMessage.id), isSize, null);
                    BaseEntity me = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(Guid.Parse(addTerrainEntityMessage.id));
                    if (me == null)
                    {
                        LogSystem.LogWarning("[VOSSynchronizer->OnMessage] Could not find entity.");
                    }
                    else
                    {
                        me.SetVisibility(true);
                        me.tag = addTerrainEntityMessage.tag;
                    }
                }
                else if (topic == "vos/status/" + currentSessionID.ToString() + "/createtextentity")
                {
                    VOSSynchronizationMessages.StatusMessages.AddTextEntityMessage
                        addTextEntityMessage = JsonConvert.DeserializeObject<
                        VOSSynchronizationMessages.StatusMessages.AddTextEntityMessage>(message);

                    CanvasEntity parent = null;
                    if (!string.IsNullOrEmpty(addTextEntityMessage.parentID))
                    {
                        BaseEntity foundEntity = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(
                            Guid.Parse(addTextEntityMessage.parentID));
                        if (foundEntity is CanvasEntity)
                        {
                            parent = (CanvasEntity) foundEntity;
                        }
                        else
                        {
                            LogSystem.LogWarning("[VOSSynchronizer->OnMessage] Parent is not a canvas entity.");
                        }
                    }

                    WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadTextEntity(addTextEntityMessage.text,
                        addTextEntityMessage.fontSize, parent,
                        addTextEntityMessage.positionPercent.ToVector2(), addTextEntityMessage.sizePercent.ToVector2(),
                        Guid.Parse(addTextEntityMessage.id), null);
                    BaseEntity be = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(Guid.Parse(addTextEntityMessage.id));
                    if (be == null)
                    {
                        LogSystem.LogWarning("[VOSSynchronizer->OnMessage] Could not find entity.");
                    }
                    else
                    {
                        be.SetVisibility(true);
                        be.tag = addTextEntityMessage.tag;
                    }
                }
                else if (topic == "vos/status/" + currentSessionID.ToString() + "/createvoxelentity")
                {
                    VOSSynchronizationMessages.StatusMessages.AddVoxelEntityMessage
                        addVoxelEntityMessage = JsonConvert.DeserializeObject<
                        VOSSynchronizationMessages.StatusMessages.AddVoxelEntityMessage>(message);

                    BaseEntity parentEntity = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(
                            Guid.Parse(addVoxelEntityMessage.parentID));

                    WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadVoxelEntity(parentEntity,
                        addVoxelEntityMessage.position.ToVector3(), addVoxelEntityMessage.rotation.ToQuaternion(),
                        addVoxelEntityMessage.scale.ToVector3(), Guid.Parse(addVoxelEntityMessage.id), null);
                    BaseEntity ve = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(Guid.Parse(addVoxelEntityMessage.id));
                    if (ve == null)
                    {
                        LogSystem.LogWarning("[VOSSynchronizer->OnMessage] Could not find entity.");
                    }
                    else
                    {
                        ve.SetVisibility(true);
                        ve.tag = addVoxelEntityMessage.tag;
                    }
                }
                else if (topic == "vos/status/" + currentSessionID.ToString() + "/state")
                {
                    VOSSynchronizationMessages.SessionMessages.SessionStateMessage
                        sessionStateMessage = JsonConvert.DeserializeObject<
                        VOSSynchronizationMessages.SessionMessages.SessionStateMessage>(message);
                    currentStateMessage = sessionStateMessage;
                    if (sessionStateMessage.sessionID == currentSessionID.ToString())
                    {
                        if (requestedState)
                        {
                            List<BaseEntity> entitiesToDelete = new List<BaseEntity>();
                            foreach (BaseEntity entity in synchronizedEntities)
                            {
                                entitiesToDelete.Add(entity);
                            }
                            foreach (BaseEntity entity in entitiesToDelete)
                            {
                                RemoveSynchronizedEntity(entity);
                            }

                            synchronizedUsers = new Dictionary<Guid, string>();
                            if (sessionStateMessage.clients != null)
                            {
                                foreach (VOSSynchronizationMessages.ClientInfo clientInfo in sessionStateMessage.clients)
                                {
                                    synchronizedUsers.Add(clientInfo.uuid, clientInfo.tag);
                                }
                            }

                            synchronizedEntities = new List<BaseEntity>();
                            if (sessionStateMessage.entities != null)
                            {
                                foreach (VOSSynchronizationMessages.EntityInfo entityInfo in sessionStateMessage.entities)
                                {
                                    BaseEntity newEntity = AddEntity(entityInfo);
                                    synchronizedEntities.Add(newEntity);
                                }
                            }
                            requestedState = false;
                            if (onGotState != null)
                            {
                                onGotState.Invoke();
                                onGotState = null;
                            }
                        }
                    }
                }
                else if (topic == "vos/status/" + currentSessionID.ToString() + "/message/new")
                {
                    VOSSynchronizationMessages.StatusMessages.NewMessageMessage
                        newMessageMessage = JsonConvert.DeserializeObject<
                        VOSSynchronizationMessages.StatusMessages.NewMessageMessage>(message);
                    foreach (Action<string, string, string> action in onMessage)
                    {
                        if (action != null)
                        {
                            action.Invoke(newMessageMessage.topic, newMessageMessage.clientID, newMessageMessage.message);
                        }
                    }
                }
                else if (topic.StartsWith("vos/status/" + currentSessionID.ToString() + "/entity/"))
                {
                    if (topic.EndsWith("/remove"))
                    {
                        VOSSynchronizationMessages.StatusMessages.RemoveEntityMessage
                            removeEntityMessage = JsonConvert.DeserializeObject<
                            VOSSynchronizationMessages.StatusMessages.RemoveEntityMessage>(message);
                        Debug.Log(removeEntityMessage.id);
                    }
                    else if (topic.EndsWith("/delete"))
                    {
                        VOSSynchronizationMessages.StatusMessages.DeleteEntityMessage
                            deleteEntityMessage = JsonConvert.DeserializeObject<
                            VOSSynchronizationMessages.StatusMessages.DeleteEntityMessage>(message);
                        BaseEntity de = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(Guid.Parse(deleteEntityMessage.id));
                        if (de == null)
                        {
                            LogSystem.LogWarning("[VOSSynchronizer->OnMessage] Could not find entity.");
                        }
                        else
                        {
                            de.Delete();
                        }
                    }
                    else if (topic.EndsWith("/position"))
                    {
                        VOSSynchronizationMessages.StatusMessages.UpdateEntityPositionMessage
                            updateEntityPositionMessage = JsonConvert.DeserializeObject<
                            VOSSynchronizationMessages.StatusMessages.UpdateEntityPositionMessage>(message);
                        if (updateEntityPositionMessage.clientID == currentClientID.ToString())
                        {
                            return;
                        }
                        BaseEntity pe = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(Guid.Parse(updateEntityPositionMessage.id));
                        if (pe == null)
                        {
                            LogSystem.LogWarning("[VOSSynchronizer->OnMessage] Could not find entity.");
                        }
                        else
                        {
                            pe.SetPosition(updateEntityPositionMessage.position.ToVector3(), false, false);
                        }
                    }
                    else if (topic.EndsWith("/rotation"))
                    {
                        VOSSynchronizationMessages.StatusMessages.UpdateEntityRotationMessage
                            updateEntityRotationMessage = JsonConvert.DeserializeObject<
                            VOSSynchronizationMessages.StatusMessages.UpdateEntityRotationMessage>(message);
                        if (updateEntityRotationMessage.clientID == currentClientID.ToString())
                        {
                            return;
                        }
                        BaseEntity re = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(Guid.Parse(updateEntityRotationMessage.id));
                        if (re == null)
                        {
                            LogSystem.LogWarning("[VOSSynchronizer->OnMessage] Could not find entity.");
                        }
                        else
                        {
                            re.SetRotation(updateEntityRotationMessage.rotation.ToQuaternion(), false, false);
                        }
                    }
                    else if (topic.EndsWith("/scale"))
                    {
                        VOSSynchronizationMessages.StatusMessages.UpdateEntityScaleMessage
                            updateEntityScaleMessage = JsonConvert.DeserializeObject<
                            VOSSynchronizationMessages.StatusMessages.UpdateEntityScaleMessage>(message);
                        if (updateEntityScaleMessage.clientID == currentClientID.ToString())
                        {
                            return;
                        }
                        BaseEntity se = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(Guid.Parse(updateEntityScaleMessage.id));
                        if (se == null)
                        {
                            LogSystem.LogWarning("[VOSSynchronizer->OnMessage] Could not find entity.");
                        }
                        else
                        {
                            se.SetScale(updateEntityScaleMessage.scale.ToVector3(), false);
                        }
                    }
                    else if (topic.EndsWith("/size"))
                    {
                        VOSSynchronizationMessages.StatusMessages.UpdateEntitySizeMessage
                            updateEntitySizeMessage = JsonConvert.DeserializeObject<
                            VOSSynchronizationMessages.StatusMessages.UpdateEntitySizeMessage>(message);
                        if (updateEntitySizeMessage.clientID == currentClientID.ToString())
                        {
                            return;
                        }
                        BaseEntity se = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(Guid.Parse(updateEntitySizeMessage.id));
                        if (se == null)
                        {
                            LogSystem.LogWarning("[VOSSynchronizer->OnMessage] Could not find entity.");
                        }
                        else
                        {
                            se.SetSize(updateEntitySizeMessage.size.ToVector3(), false);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Add an entity locally that is being synchronized.
        /// </summary>
        /// <param name="entityInfo">Information about the entity to add.</param>
        /// <returns>The entity that has been added.</returns>
        private BaseEntity AddEntity(VOSSynchronizationMessages.EntityInfo entityInfo)
        {
            bool isSize = false;
            Vector3 scaleSize = Vector3.one;
            if (entityInfo.scale != null)
            {
                scaleSize = new Vector3(entityInfo.scale.x, entityInfo.scale.y, entityInfo.scale.z);
            }
            else if (entityInfo.size != null)
            {
                scaleSize = new Vector3(entityInfo.size.x, entityInfo.size.y, entityInfo.size.z);
                isSize = true;
            }

            Guid newEntityID;
            BaseEntity ne;
            BaseEntity parentEntity = null;
            if (entityInfo.parentID != null)
            {
                WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(Guid.Parse(entityInfo.parentID));
            }

            CanvasEntity parentCE = null;
            if (parentEntity is CanvasEntity)
            {
                parentCE = (CanvasEntity) parentEntity;
            }

            switch (entityInfo.type)
            {
                case "mesh":
                    WebVerseRuntime.Instance.gltfHandler.LoadGLTFResourceAsMeshEntity(entityInfo.path, Guid.Parse(entityInfo.parentID));
                    ne = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(Guid.Parse(entityInfo.id));
                    if (ne == null)
                    {
                        LogSystem.LogWarning("[VOSSynchronizer->AddEntity] Could not find entity.");
                    }
                    else
                    {
                        ne.SetParent(parentEntity);
                        ne.SetVisibility(true);
                        ne.SetPosition(entityInfo.position.ToVector3(), false, false);
                        ne.SetRotation(entityInfo.rotation.ToQuaternion(), false, false);
                        if (isSize)
                        {
                            ne.SetSize(scaleSize, false);
                        }
                        else
                        {
                            ne.SetScale(scaleSize, false);
                        }
                        ne.tag = entityInfo.tag;
                    }
                    return ne;

                case "character":
                    newEntityID = WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadCharacterEntity(parentEntity,
                        new Vector3(entityInfo.position.x, entityInfo.position.y, entityInfo.position.z),
                        new Quaternion(entityInfo.rotation.x, entityInfo.rotation.y,
                        entityInfo.rotation.z, entityInfo.rotation.w), scaleSize, Guid.Parse(entityInfo.id),
                        entityInfo.tag, isSize, null);
                    ne = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(Guid.Parse(entityInfo.id));
                    if (ne == null)
                    {
                        LogSystem.LogWarning("[VOSSynchronizer->AddEntity] Could not find entity.");
                    }
                    else
                    {
                        ne.SetVisibility(true);
                    }
                    return ne;

                case "button":
                    Action onClickAction = null;
                    if (entityInfo.onClick != null)
                    {
                        onClickAction = new Action(() =>
                        {
                            WebVerseRuntime.Instance.javascriptHandler.Run(entityInfo.onClick);
                        });
                    }

                    newEntityID = WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadButtonEntity(parentCE,
                        entityInfo.positionPercent.ToVector2(), entityInfo.sizePercent.ToVector2(), onClickAction,
                        Guid.Parse(entityInfo.id), null);
                    ne = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(Guid.Parse(entityInfo.id));
                    if (ne == null)
                    {
                        LogSystem.LogWarning("[VOSSynchronizer->AddEntity] Could not find entity.");
                    }
                    else
                    {
                        ne.SetVisibility(true);
                    }
                    return ne;

                case "canvas":
                    newEntityID = WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadCanvasEntity(parentEntity,
                        entityInfo.position.ToVector3(), entityInfo.rotation.ToQuaternion(), scaleSize, Guid.Parse(entityInfo.id),
                        isSize, null);
                    ne = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(Guid.Parse(entityInfo.id));
                    if (ne == null)
                    {
                        LogSystem.LogWarning("[VOSSynchronizer->AddEntity] Could not find entity.");
                    }
                    else
                    {
                        ne.SetVisibility(true);
                    }
                    return ne;

                case "input":
                    newEntityID = WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadInputEntity(parentCE,
                        entityInfo.positionPercent.ToVector2(), entityInfo.sizePercent.ToVector2(),
                        Guid.Parse(entityInfo.id), null);
                    ne = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(Guid.Parse(entityInfo.id));
                    if (ne == null)
                    {
                        LogSystem.LogWarning("[VOSSynchronizer->AddEntity] Could not find entity.");
                    }
                    else
                    {
                        ne.SetVisibility(true);
                    }
                    return ne;

                case "light":
                    newEntityID = WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadLightEntity(parentEntity,
                        entityInfo.position.ToVector3(), entityInfo.rotation.ToQuaternion(), Guid.Parse(entityInfo.id),
                        null);
                    ne = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(Guid.Parse(entityInfo.id));
                    if (ne == null)
                    {
                        LogSystem.LogWarning("[VOSSynchronizer->AddEntity] Could not find entity.");
                    }
                    else
                    {
                        ne.SetVisibility(true);
                    }
                    return ne;

                case "terrain":
                    newEntityID = WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadTerrainEntity(entityInfo.length,
                        entityInfo.width, entityInfo.height, entityInfo.heights, parentEntity, entityInfo.position.ToVector3(),
                        entityInfo.rotation.ToQuaternion(), scaleSize, Guid.Parse(entityInfo.id), isSize, null);
                    ne = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(Guid.Parse(entityInfo.id));
                    if (ne == null)
                    {
                        LogSystem.LogWarning("[VOSSynchronizer->AddEntity] Could not find entity.");
                    }
                    else
                    {
                        ne.SetVisibility(true);
                    }
                    return ne;

                case "text":
                    newEntityID = WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadTextEntity(entityInfo.text,
                        entityInfo.fontSize, parentCE, entityInfo.positionPercent.ToVector2(),
                        entityInfo.sizePercent.ToVector2(), Guid.Parse(entityInfo.id), null);
                    ne = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(Guid.Parse(entityInfo.id));
                    if (ne == null)
                    {
                        LogSystem.LogWarning("[VOSSynchronizer->AddEntity] Could not find entity.");
                    }
                    else
                    {
                        ne.SetVisibility(true);
                    }
                    return ne;

                case "voxel":
                    newEntityID = WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadVoxelEntity(parentEntity,
                        entityInfo.position.ToVector3(), entityInfo.rotation.ToQuaternion(), entityInfo.scale.ToVector3(),
                        Guid.Parse(entityInfo.id), null);
                    ne = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(Guid.Parse(entityInfo.id));
                    if (ne == null)
                    {
                        LogSystem.LogWarning("[VOSSynchronizer->AddEntity] Could not find entity.");
                    }
                    else
                    {
                        ne.SetVisibility(true);
                    }
                    return ne;
            }

            return null;
        }
    }
}