using UnityEngine;
using FiveSQD.WebVerse.Utilities;
using FiveSQD.WebVerse.LocalStorage;
using FiveSQD.WebVerse.Handlers.File;
using FiveSQD.WebVerse.Handlers.GLTF;
using FiveSQD.WebVerse.Handlers.PNG;
using FiveSQD.WebVerse.Handlers.Javascript;
using FiveSQD.WebVerse.VOSSynchronization;
using System.IO;
using FiveSQD.WebVerse.Handlers.VEML;
using System;
using FiveSQD.WebVerse.Input;

namespace FiveSQD.WebVerse.Runtime
{
    public class WebVerseRuntime : MonoBehaviour
    {
        // Max Entries: 65536
        // Max Entry Length: 16384
        // Max Key Length: 512
        public struct RuntimeSettings
        {
            public string storageMode;

            public int maxEntries;

            public int maxEntryLength;

            public int maxKeyLength;
        }

        public static WebVerseRuntime Instance;

        public WorldEngine.WorldEngine worldEngine { get; private set; }

        public FileHandler fileHandler { get; private set; }

        public PNGHandler pngHandler { get; private set; }

        public JavascriptHandler javascriptHandler { get; private set; }

        public GLTFHandler gltfHandler { get; private set; }

        public VEMLHandler vemlHandler { get; private set; }

        public VOSSynchronizationManager vosSynchronizationManager { get; private set; }

        public LocalStorageManager localStorageManager { get; private set; }

        public InputManager inputManager { get; private set; }

        public Material highlightMaterial;

        public Material skyMaterial;

        public GameObject inputEntityPrefab;

        public GameObject characterControllerPrefab;

        public GameObject voxelPrefab;

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

            Initialize(mode, settings.maxEntries, settings.maxEntryLength, settings.maxKeyLength);
        }

        public void Initialize(LocalStorageManager.LocalStorageMode storageMode,
            int maxEntries, int maxEntryLength, int maxKeyLength)
        {
            if (Instance != null)
            {
                Logging.LogError("[WebVerseRuntime->Initialize] Already initialized.");
                return;
            }

            Instance = this;

            InitializeComponents(storageMode, maxEntries, maxEntryLength, maxKeyLength);
        }

        public void Terminate()
        {
            if (Instance == null)
            {
                Logging.LogError("[WebVerseRuntime->Terminate] Not initialized.");
                return;
            }

            TerminateComponents();
        }

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

        private void InitializeComponents(LocalStorageManager.LocalStorageMode storageMode,
            int maxEntries, int maxEntryLength, int maxKeyLength)
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
            vosSynchronizationManager = vosSynchronizationManagerGO.AddComponent<VOSSynchronizationManager>();
            vosSynchronizationManager.Initialize();

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
        }

        private void TerminateComponents()
        {
            // Terminate Input Manager.
            inputManager.Terminate();
            Destroy(inputManager.gameObject);

            // Terminate Local Storage Manager.
            localStorageManager.Terminate();
            Destroy(localStorageManager.gameObject);

            // Terminate VOS Synchronization Manager.
            vosSynchronizationManager.Terminate();
            Destroy(vosSynchronizationManager.gameObject);

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