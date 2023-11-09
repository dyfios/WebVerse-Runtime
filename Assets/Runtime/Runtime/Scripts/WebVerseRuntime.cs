// Copyright (c) 2019-2023 Five Squared Interactive. All rights reserved.

using UnityEngine;
using FiveSQD.WebVerse.Utilities;
using FiveSQD.WebVerse.LocalStorage;
using FiveSQD.WebVerse.Handlers.File;
using FiveSQD.WebVerse.Handlers.GLTF;
using FiveSQD.WebVerse.Handlers.PNG;
using FiveSQD.WebVerse.Handlers.Javascript;
#if USE_WEBINTERFACE
using FiveSQD.WebVerse.VOSSynchronization;
#endif
using System.IO;
using FiveSQD.WebVerse.Handlers.VEML;
using System;
using FiveSQD.WebVerse.Input;
using FiveSQD.WebVerse.Daemon;

namespace FiveSQD.WebVerse.Runtime
{
    /// <summary>
    /// The WebVerse Runtime
    /// </summary>
    public class WebVerseRuntime : MonoBehaviour
    {
        /// <summary>
        /// WebVerse Runtime settings.
        /// </summary>
        public struct RuntimeSettings
        {
            /// <summary>
            /// The storage mode. Valid values: cache, persistent.
            /// </summary>
            public string storageMode;

            /// <summary>
            /// Maximum storage entries.
            /// </summary>
            public int maxEntries;

            /// <summary>
            /// Maximum storage entry length.
            /// </summary>
            public int maxEntryLength;

            /// <summary>
            /// Maximum storage entry key length.
            /// </summary>
            public int maxKeyLength;

            /// <summary>
            /// Daemon Port.
            /// </summary>
            public uint? daemonPort;

            /// <summary>
            /// Main App ID.
            /// </summary>
            public Guid? mainAppID;
        }

        /// <summary>
        /// Static reference to the WebVerse runtime.
        /// </summary>
        public static WebVerseRuntime Instance;

        /// <summary>
        /// The World Engine.
        /// </summary>
        [Tooltip("The World Engine.")]
        public WorldEngine.WorldEngine worldEngine { get; private set; }

        /// <summary>
        /// The File Handler
        /// </summary>
        [Tooltip("The File Handler.")]
        public FileHandler fileHandler { get; private set; }

        /// <summary>
        /// The PNG Handler.
        /// </summary>
        [Tooltip("The PNG Handler.")]
        public PNGHandler pngHandler { get; private set; }

        /// <summary>
        /// The Javascript Handler.
        /// </summary>
        [Tooltip("The Javascript Handler.")]
        public JavascriptHandler javascriptHandler { get; private set; }

        /// <summary>
        /// The GLTF Handler.
        /// </summary>
        [Tooltip("The GLTF Handler.")]
        public GLTFHandler gltfHandler { get; private set; }

        /// <summary>
        /// The VEML Handler.
        /// </summary>
        [Tooltip("The VEML Handler.")]
        public VEMLHandler vemlHandler { get; private set; }

#if USE_WEBINTERFACE
        /// <summary>
        /// The VOS Synchronization Manager.
        /// </summary>
        [Tooltip("The VOS Synchronization Manager.")]
        public VOSSynchronizationManager vosSynchronizationManager { get; private set; }
#endif

        /// <summary>
        /// The Local Storage Manager.
        /// </summary>
        [Tooltip("The Local Storage Manager.")]
        public LocalStorageManager localStorageManager { get; private set; }

        /// <summary>
        /// The Input Manager.
        /// </summary>
        [Tooltip("The Input Manager.")]
        public InputManager inputManager { get; private set; }

        /// <summary>
        /// The Daemon Manager.
        /// </summary>
        [Tooltip("The Daemon Manager.")]
        public WebVerseDaemonManager webVerseDaemonManager { get; private set; }

        /// <summary>
        /// Material to use for highlighting.
        /// </summary>
        [Tooltip("Material to use for highlighting.")]
        public Material highlightMaterial;

        /// <summary>
        /// Material to use for the sky.
        /// </summary>
        [Tooltip("Material to use for the sky.")]
        public Material skyMaterial;

        /// <summary>
        /// Prefab for an Input Entity.
        /// </summary>
        [Tooltip("Prefab for an Input Entity.")]
        public GameObject inputEntityPrefab;

        /// <summary>
        /// Prefab for a Character Controller.
        /// </summary>
        [Tooltip("Prefab for a Character Controller.")]
        public GameObject characterControllerPrefab;

        /// <summary>
        /// Prefab for a Voxel.
        /// </summary>
        [Tooltip("Prefab for a Voxel.")]
        public GameObject voxelPrefab;

        /// <summary>
        /// Initialize the WebVerse Runtime.
        /// </summary>
        /// <param name="settings">The runtime settings to use.</param>
        public void Initialize(RuntimeSettings settings)
        {
            LocalStorageManager.LocalStorageMode mode = LocalStorageManager.LocalStorageMode.Uninitialized;
            if (settings.storageMode.ToLower() == "cache")
            {
                mode = LocalStorageManager.LocalStorageMode.Cache;
            }
            else if (settings.storageMode.ToLower() == "persistent")
            {
                mode = LocalStorageManager.LocalStorageMode.Persistent;
            }
            else
            {
                Logging.LogError("[WebVerseRuntime->Initialize] Invalid storage mode.");
                return;
            }

            if (settings.maxEntries <= 0 || settings.maxEntries >= 262144)
            {
                Logging.LogError("[WebVerseRuntime->Initialize] Invalid max entries value.");
                return;
            }

            if (settings.maxEntryLength <= 8 || settings.maxEntryLength >= 131072)
            {
                Logging.LogError("[WebVerseRuntime->Initialize] Invalid max entry length value.");
                return;
            }

            if (settings.maxKeyLength <= 4 || settings.maxKeyLength >= 8192)
            {
                Logging.LogError("[WebVerseRuntime->Initialize] Invalid max key length value.");
            }

            Initialize(mode, settings.maxEntries, settings.maxEntryLength,
                settings.maxKeyLength, settings.daemonPort, settings.mainAppID);
        }

        /// <summary>
        /// Initialize the WebVerse Runtime.
        /// </summary>
        /// <param name="storageMode">Mode to set local storage to. Must be cache or persistent.</param>
        /// <param name="maxEntries">Maximum number of storage entries.</param>
        /// <param name="maxEntryLength">Maximum length of a storage entry.</param>
        /// <param name="maxKeyLength">Maximum length of a storage entry key.</param>
        /// <param name="daemonPort">Daemon Port.</param>
        /// <param name="mainAppID">Main App ID.</param>
        public void Initialize(LocalStorageManager.LocalStorageMode storageMode,
            int maxEntries, int maxEntryLength, int maxKeyLength,
            uint? daemonPort = null, Guid? mainAppID = null)
        {
            if (Instance != null)
            {
                Logging.LogError("[WebVerseRuntime->Initialize] Already initialized.");
                return;
            }

            Instance = this;

            InitializeComponents(storageMode, maxEntries, maxEntryLength, maxKeyLength, daemonPort, mainAppID);
        }

        /// <summary>
        /// Terminate the WebVerse Runtime.
        /// </summary>
        public void Terminate()
        {
            if (Instance == null)
            {
                Logging.LogError("[WebVerseRuntime->Terminate] Not initialized.");
                return;
            }

            TerminateComponents();
        }

        /// <summary>
        /// Load a world.
        /// </summary>
        /// <param name="url">URL containing the world to load.</param>
        public void LoadWorld(string url)
        {
            if (worldEngine == null)
            {
                Logging.LogError("[WebVerseRuntime->LoadWorld] World Engine not initialized.");
                return;
            }

            if (WorldEngine.WorldEngine.ActiveWorld != null)
            {
                UnloadWorld();
            }

            Action<string> onFound = (title) =>
            {
                WorldEngine.WorldEngine.LoadWorld(title);
                vemlHandler.LoadVEMLDocumentIntoWorld(url);
            };

            vemlHandler.GetWorldName(url, onFound);
        }

        /// <summary>
        /// Unload a world.
        /// </summary>
        public void UnloadWorld()
        {
            if (WorldEngine.WorldEngine.ActiveWorld == null)
            {
                Logging.LogWarning("[WebVerseRuntime->UnloadWorld] No world to unload.");
                return;
            }

            Logging.Log("[WebVerseRuntime->UnloadWorld] Unloading world: "
                + WorldEngine.WorldEngine.ActiveWorld.siteName + ".");
            WorldEngine.WorldEngine.UnloadWorld();
        }

        /// <summary>
        /// Initialize the WebVerse Runtime components.
        /// </summary>
        /// <param name="storageMode">Mode to set local storage to. Must be cache or persistent.</param>
        /// <param name="maxEntries">Maximum number of storage entries.</param>
        /// <param name="maxEntryLength">Maximum length of a storage entry.</param>
        /// <param name="maxKeyLength">Maximum length of a storage entry key.</param>
        /// <param name="daemonPort">Daemon Port.</param>
        /// <param name="mainAppID">Main App ID.</param>
        private void InitializeComponents(LocalStorageManager.LocalStorageMode storageMode,
            int maxEntries, int maxEntryLength, int maxKeyLength, uint? daemonPort = null,
            Guid? mainAppID = null)
        {
            // Set up World Engine.
            GameObject worldEngineGO = new GameObject("WorldEngine");
            worldEngineGO.transform.SetParent(transform);
            worldEngine = worldEngineGO.AddComponent<WorldEngine.WorldEngine>();
            worldEngine.highlightMaterial = highlightMaterial;
            worldEngine.skyMaterial = skyMaterial;
            worldEngine.inputEntityPrefab = inputEntityPrefab;
            worldEngine.characterControllerPrefab = characterControllerPrefab;
            worldEngine.voxelPrefab = voxelPrefab;

            // Set up Handlers.
            GameObject handlersGO = new GameObject("Handlers");
            handlersGO.transform.SetParent(transform);
            GameObject fileHandlerGO = new GameObject("File");
            fileHandlerGO.transform.SetParent(handlersGO.transform);
            fileHandler = fileHandlerGO.AddComponent<FileHandler>();
            fileHandler.Initialize(Path.Combine(Application.dataPath, "Files"));
            GameObject pngHandlerGO = new GameObject("PNG");
            pngHandlerGO.transform.SetParent(handlersGO.transform);
            pngHandler = pngHandlerGO.AddComponent<PNGHandler>();
            pngHandler.runtime = this;
            pngHandler.Initialize();
            GameObject javascriptHandlerGO = new GameObject("Javascript");
            javascriptHandlerGO.transform.SetParent(handlersGO.transform);
            javascriptHandler = javascriptHandlerGO.AddComponent<JavascriptHandler>();
            javascriptHandler.Initialize();
            GameObject gltfHandlerGO = new GameObject("GLTF");
            gltfHandlerGO.transform.SetParent(handlersGO.transform);
            gltfHandler = gltfHandlerGO.AddComponent<GLTFHandler>();
            gltfHandler.Initialize();
            gltfHandler.runtime = this;
            GameObject gltfPrefabContainer = new GameObject("PrefabContainer");
            gltfPrefabContainer.transform.SetParent(gltfHandler.transform);
            gltfHandler.meshPrefabContainer = gltfPrefabContainer;
            GameObject vemlHandlerGO = new GameObject("VEML");
            vemlHandlerGO.transform.SetParent(handlersGO.transform);
            vemlHandler = vemlHandlerGO.AddComponent<VEMLHandler>();
            vemlHandler.runtime = this;
            vemlHandler.Initialize();

            // Set up VOS Synchronization Manager.
            GameObject vosSynchronizationManagerGO = new GameObject("VOSSynchronizationManager");
            vosSynchronizationManagerGO.transform.SetParent(transform);
#if USE_WEBINTERFACE
            vosSynchronizationManager = vosSynchronizationManagerGO.AddComponent<VOSSynchronizationManager>();
            vosSynchronizationManager.Initialize();
#endif

            // Set up Local Storage Manager.
            GameObject localStorageManagerGO = new GameObject("LocalStorageManager");
            localStorageManagerGO.transform.SetParent(transform);
            localStorageManager = localStorageManagerGO.AddComponent<LocalStorageManager>();
            localStorageManager.Initialize(storageMode, maxEntries, maxEntryLength, maxKeyLength);

            // Set up Input Manager.
            GameObject inputManagerGO = new GameObject("InputManager");
            inputManagerGO.transform.SetParent(transform);
            inputManager = inputManagerGO.AddComponent<InputManager>();
            inputManager.Initialize();

            if (daemonPort != null && mainAppID != null)
            {
                // Set up Daemon Manager.
                GameObject daemonManagerGO = new GameObject("DaemonManager");
                daemonManagerGO.transform.SetParent(transform);
                webVerseDaemonManager = daemonManagerGO.AddComponent<WebVerseDaemonManager>();
                webVerseDaemonManager.Initialize();
                webVerseDaemonManager.ConnectToDaemon(daemonPort.Value, mainAppID.Value);
            }
        }

        /// <summary>
        /// Terminate the WebVerse Runtime components.
        /// </summary>
        private void TerminateComponents()
        {
            if (webVerseDaemonManager != null)
            {
                // Terminate Daemon Manager.
                webVerseDaemonManager.Terminate();
                Destroy(webVerseDaemonManager.gameObject);
            }

            // Terminate Input Manager.
            inputManager.Terminate();
            Destroy(inputManager.gameObject);

            // Terminate Local Storage Manager.
            localStorageManager.Terminate();
            Destroy(localStorageManager.gameObject);

#if USE_WEBINTERFACE
            // Terminate VOS Synchronization Manager.
            vosSynchronizationManager.Terminate();
            Destroy(vosSynchronizationManager.gameObject);
#endif

            // Terminate Handlers.
            vemlHandler.Terminate();
            gltfHandler.Terminate();
            javascriptHandler.Terminate();
            pngHandler.Terminate();
            fileHandler.Terminate();
            Destroy(fileHandler.transform.parent.gameObject);

            // Terminate World Engine.
            if (WorldEngine.WorldEngine.ActiveWorld != null)
            {
                WorldEngine.WorldEngine.UnloadWorld();
            }
            Destroy(worldEngine.gameObject);
        }
    }
}