// Copyright (c) 2019-2023 Five Squared Interactive. All rights reserved.

using FiveSQD.WebVerse.Utilities;
using FiveSQD.WebVerse.WebInterface.MQTT;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FiveSQD.WebVerse.VOSSynchronization
{
    /// <summary>
    /// Class for a VOS Synchronization Manager.
    /// </summary>
    public class VOSSynchronizationManager : BaseManager
    {
        /// <summary>
        /// VOS synchronizers being managed.
        /// </summary>
        private List<VOSSynchronizer> vosSynchronizers;

        /// <summary>
        /// Mapping of VOS synchronizers to session IDs.
        /// </summary>
        private Dictionary<string, Tuple<VOSSynchronizer, Guid>> vosSynchronizersAndSessions;

        /// <summary>
        /// Initialize the synchronization manager.
        /// </summary>
        public override void Initialize()
        {
            if (vosSynchronizers != null)
            {
                Logging.LogError("[VOSSynchronizationManager->Initialize] Already initialized.");
                return;
            }

            vosSynchronizers = new List<VOSSynchronizer>();
            vosSynchronizersAndSessions = new Dictionary<string, Tuple<VOSSynchronizer, Guid>>();
        }

        /// <summary>
        /// Terminate the synchronization manager.
        /// </summary>
        public override void Terminate()
        {
            base.Terminate();

            if (vosSynchronizers != null)
            {
                foreach (VOSSynchronizer synchronizer in vosSynchronizers)
                {
                    synchronizer.Terminate();
                }
            }
            vosSynchronizers = new List<VOSSynchronizer>();
            vosSynchronizersAndSessions = new Dictionary<string, Tuple<VOSSynchronizer, Guid>>();
        }

        /// <summary>
        /// Add a synchronizer and session.
        /// </summary>
        /// <param name="id">ID for the session.</param>
        /// <param name="host">Host for the synchronizer.</param>
        /// <param name="port">Port for the synchronizer.</param>
        /// <param name="tls">Whether or not to use TLS.</param>
        /// <param name="transport">Transport to use.</param>
        /// <param name="session">Name for the session.</param>
        /// <returns>Tuple containing the synchronizer and session ID.</returns>
        public Tuple<VOSSynchronizer, Guid> AddSynchronizerAndSession(string id, string host, int port, bool tls,
            MQTTClient.Transports transport, string session)
        {
            if (vosSynchronizers == null)
            {
                Logging.LogError("[VOSSynchronizationManager->AddSynchronizerAndSession] Not initialized.");
                return null;
            }

            VOSSynchronizer newSync = GetSynchronizer(host, port);
            if (newSync == null)
            {
                newSync = AddSynchronizer(host, port, tls, transport);
            }
            Guid newSessionID = Guid.NewGuid();
            Action onConnected = () =>
            {
                newSync.CreateSession(newSessionID, session);
                newSync.JoinSession(newSessionID, "VEML");
            };
            newSync.Connect(onConnected);
            Tuple<VOSSynchronizer, Guid> combo = new Tuple<VOSSynchronizer, Guid>(newSync, newSessionID);
            vosSynchronizersAndSessions.Add(id, combo);

            return combo;
        }

        /// <summary>
        /// Add a synchronizer.
        /// </summary>
        /// <param name="host">Host for the synchronizer.</param>
        /// <param name="port">Port for the synchronizer.</param>
        /// <param name="tls">Whether or not to use TLS.</param>
        /// <param name="transport">Transport to use.</param>
        /// <returns>Synchronizer that has been added.</returns>
        public VOSSynchronizer AddSynchronizer(string host, int port, bool tls, MQTTClient.Transports transport)
        {
            if (vosSynchronizers == null)
            {
                Logging.LogError("[VOSSynchronizationManager->AddSynchronizer] Not initialized.");
                return null;
            }

            GameObject newSynchronizerGO = new GameObject("Synchronizer-"
                + host + ":" + port);
            VOSSynchronizer vosSynchronizer = newSynchronizerGO.AddComponent<VOSSynchronizer>();
            vosSynchronizer.Initialize(host, port, tls, transport);
            vosSynchronizers.Add(vosSynchronizer);
            return vosSynchronizer;
        }

        /// <summary>
        /// Remove a synchronizer.
        /// </summary>
        /// <param name="synchronizer">Synchronizer to use.</param>
        public void RemoveSynchronizer(VOSSynchronizer synchronizer)
        {
            if (vosSynchronizers == null)
            {
                Logging.LogError("[VOSSynchronizationManager->RemoveSynchronizer] Not initialized.");
                return;
            }

            if (vosSynchronizers.Contains(synchronizer))
            {
                vosSynchronizers.Remove(synchronizer);
            }
        }

        /// <summary>
        /// Remove a synchronizer.
        /// </summary>
        /// <param name="host">Host of the synchronizer to remove.</param>
        /// <param name="port">Port of the synchronizer to remove.</param>
        public void RemoveSynchronizer(string host, int port)
        {
            VOSSynchronizer vosSynchronizer = GetSynchronizer(host, port);
            if (vosSynchronizer == null)
            {
                Logging.LogWarning("[VOSSynchronizationManager->RemoveSynchronizer] Synchronizer not found.");
                return;
            }
            RemoveSynchronizer(vosSynchronizer);
        }

        /// <summary>
        /// Get a synchronizer.
        /// </summary>
        /// <param name="host">Host of the synchronizer to get.</param>
        /// <param name="port">Port of the synchronizer to get.</param>
        /// <returns>Synchronizer, or null if not found.</returns>
        public VOSSynchronizer GetSynchronizer(string host, int port)
        {
            if (vosSynchronizers == null)
            {
                Logging.LogError("[VOSSynchronizationManager->GetSynchronizer] Not initialized.");
                return null;
            }

            foreach (VOSSynchronizer vosSynchronizer in vosSynchronizers)
            {
                if (vosSynchronizer.host == host && vosSynchronizer.port == port)
                {
                    return vosSynchronizer;
                }
            }
            return null;
        }

        /// <summary>
        /// Get a synchronizer and session.
        /// </summary>
        /// <param name="id">ID of the session.</param>
        /// <returns>A tuple containing the synchronizer and session, or null if not found.</returns>
        public Tuple<VOSSynchronizer, Guid> GetSynchronizerAndSession(string id)
        {
            if (vosSynchronizersAndSessions == null)
            {
                Logging.LogError("[VOSSynchronizationManager->GetSynchronizerAndSession] Not initialized.");
                return null;
            }

            if (vosSynchronizersAndSessions.ContainsKey(id))
            {
                return vosSynchronizersAndSessions[id];
            }
            return null;
        }
    }
}