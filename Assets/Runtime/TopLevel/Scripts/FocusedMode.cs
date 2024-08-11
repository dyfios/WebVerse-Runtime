// Copyright (c) 2019-2024 Five Squared Interactive. All rights reserved.

using FiveSQD.WebVerse.Handlers.Javascript.APIs.Utilities;
using System;
using UnityEngine;

namespace FiveSQD.WebVerse.Runtime
{
    /// <summary>
    /// WebVerse Focused Mode.
    /// </summary>
    public class FocusedMode : MonoBehaviour
    {
        /// <summary>
        /// URI to use in Unity Editor tests.
        /// </summary>
        [Tooltip("URI to use in Unity Editor tests.")]
        public string testURI;

        /// <summary>
        /// Storage mode to use in Unity Editor tests.
        /// </summary>
        [Tooltip("Storage mode to use in Unity Editor tests.")]
        public string testStorageMode = "persistent";

        /// <summary>
        /// Maximum local storage entries to use in Unity Editor tests.
        /// </summary>
        [Tooltip("Maximum local storage entries to use in Unity Editor tests.")]
        public string testMaxEntries = "65536";

        /// <summary>
        /// Maximum local storage entry length to use in Unity Editor tests.
        /// </summary>
        [Tooltip("Maximum local storage entry length to use in Unity Editor tests.")]
        public string testMaxEntryLength = "16384";

        /// <summary>
        /// Maximum local storage key length to use in Unity Editor tests.
        /// </summary>
        [Tooltip("Maximum local storage key length to use in Unity Editor tests.")]
        public string testMaxKeyLength = "512";

        /// <summary>
        /// Directory to use for files in Unity Editor tests.
        /// </summary>
        [Tooltip("Directory to use for files in Unity Editory Tests")]
        public string testFilesDirectory = System.IO.Path.Combine(Application.dataPath, "Files");

        /// <summary>
        /// Daemon port to use in Unity Editor tests.
        /// </summary>
        [Tooltip("Daemon port to use in Unity Editor tests.")]
        public string testDaemonPort = "0";

        /// <summary>
        /// Main app ID to use in Unity Editor tests.
        /// </summary>
        [Tooltip("Main app ID to use in Unity Editor tests.")]
        public string testMainAppID;

        /// <summary>
        /// Tab ID to use in Unity Editor tests.
        /// </summary>
        [Tooltip("Tab ID to use in Unity Editor tests.")]
        public string testTabID;

        /// <summary>
        /// World Load Timeout to use in Unity Editor tests.
        /// </summary>
        [Tooltip("World Load Timeout to use in Unity Editor tests.")]
        public string testWorldLoadTimeout;

        /// <summary>
        /// History to use in Unity Editor tests.
        /// </summary>
        [Tooltip("History to use in Unity Editor tests.")]
        public string testHistory;

        /// <summary>
        /// WebVerse Runtime.
        /// </summary>
        [Tooltip("WebVerse Runtime.")]
        public WebVerseRuntime runtime;

        private void Awake()
        {
            LoadRuntime();
        }

        /// <summary>
        /// Load the runtime.
        /// </summary>
        private void LoadRuntime()
        {
            string uri = GetWorldURI();
            if (string.IsNullOrEmpty(uri))
            {
                Logging.LogError("[FocusedMode->LoadRuntime] Could not get world URI.");
                return;
            }

            LocalStorage.LocalStorageManager.LocalStorageMode storageMode = GetStorageMode();
            if (storageMode != LocalStorage.LocalStorageManager.LocalStorageMode.Cache &&
                storageMode != LocalStorage.LocalStorageManager.LocalStorageMode.Persistent)
            {
                Logging.LogError("[FocusedMode->LoadRuntime] Could not get storage mode.");
                return;
            }

            int maxEntries = GetMaxEntries();
            if (maxEntries <= 0 || maxEntries >= 262144)
            {
                Logging.LogError("[FocusedMode->LoadRuntime] Invalid max entries value.");
                return;
            }

            int maxEntryLength = GetMaxEntryLength();
            if (maxEntryLength <= 8 || maxEntryLength >= 131072)
            {
                Logging.LogError("[FocusedMode->LoadRuntime] Invalid max entry length value.");
                return;
            }

            int maxKeyLength = GetMaxKeyLength();
            if (maxKeyLength <= 4 || maxKeyLength >= 8192)
            {
                Logging.LogError("[FocusedMode->LoadRuntime] Invalid max key length value.");
                return;
            }

            string filesDirectory = GetFilesDirectory();
            if (string.IsNullOrEmpty(filesDirectory))
            {
                Logging.LogError("[LightweightMode->LoadRuntime] Invalid files directory value.");
                return;
            }

            uint daemonPort = GetDaemonPort();
            if (daemonPort <= 0 || daemonPort >= 65535)
            {
                Logging.LogError("[FocusedMode->LoadRuntime] Invalid daemon port value.");
            }

            Guid mainAppID = GetMainAppID();
            if (mainAppID == Guid.Empty)
            {
                Logging.LogError("[FocusedMode->LoadRuntime] Invalid main app ID value.");
            }

            int tabID = GetTabID();
            if (tabID < 1)
            {
                Logging.LogError("[FocusedMode->LoadRuntime] Invalid tab ID value.");
            }

            float worldLoadTimeout = GetWorldLoadTimeout();
            if (worldLoadTimeout <= 0)
            {
                Logging.LogError("[FocusedMode->LoadRuntime] Invalid world load timeout.");
                worldLoadTimeout = 120;
            }

            string history = GetHistory();

            runtime.Initialize(storageMode, maxEntries, maxEntryLength, maxKeyLength,
                filesDirectory, daemonPort, mainAppID, tabID, worldLoadTimeout);
            runtime.handMenuController.UpdateHistory(history);
            runtime.LoadURL(uri);
        }

        /// <summary>
        /// Get the World URI, provided by command line in built app, and by 'testURI'
        /// variable in Editor mode.
        /// </summary>
        /// <returns>World URI.</returns>
        private string GetWorldURI()
        {
#if UNITY_EDITOR
            return testURI;
#else
            foreach (string arg in System.Environment.GetCommandLineArgs())
            {
                if (arg.StartsWith("uri="))
                {
                    return arg.Substring(4);
                }
            }
            return null;
#endif
        }

        /// <summary>
        /// Get the Local Storage Mode, provided by command line in built app, and by 'testStorageMode'
        /// variable in Editor mode.
        /// </summary>
        /// <returns>Local Storage Mode.</returns>
        private LocalStorage.LocalStorageManager.LocalStorageMode GetStorageMode()
        {
            string storageMode = "";

#if UNITY_EDITOR
            storageMode = testStorageMode;
#else
            foreach (string arg in System.Environment.GetCommandLineArgs())
            {
                if (arg.StartsWith("storagemode="))
                {
                    storageMode = arg.Substring(12);
                }
            }
#endif
            if (storageMode.ToLower() == "persistent")
            {
                return LocalStorage.LocalStorageManager.LocalStorageMode.Persistent;
            }
            else if (storageMode.ToLower() == "cache")
            {
                return LocalStorage.LocalStorageManager.LocalStorageMode.Cache;
            }
            else
            {
                Logging.LogError("[FocusedMode->GetStorageMode] Invalid storage mode.");
                return LocalStorage.LocalStorageManager.LocalStorageMode.Uninitialized;
            }
        }

        /// <summary>
        /// Get the Max Local Storage Entries, provided by command line in built app, and by 'testMaxEntries'
        /// variable in Editor mode.
        /// </summary>
        /// <returns>Max Local Storage Entries.</returns>
        private int GetMaxEntries()
        {
            string maxEntries = "";

#if UNITY_EDITOR
            maxEntries = testMaxEntries;
#else
            foreach (string arg in System.Environment.GetCommandLineArgs())
            {
                if (arg.StartsWith("maxentries="))
                {
                    maxEntries = arg.Substring(11);
                }
            }
#endif
            return int.Parse(maxEntries);
        }

        /// <summary>
        /// Get the Max Local Storage Entry Length, provided by command line in built app, and by 'testMaxEntryLength'
        /// variable in Editor mode.
        /// </summary>
        /// <returns>Max Local Storage Entry Length.</returns>
        private int GetMaxEntryLength()
        {
            string maxEntryLength = "";

#if UNITY_EDITOR
            maxEntryLength = testMaxEntryLength;
#else
            foreach (string arg in System.Environment.GetCommandLineArgs())
            {
                if (arg.StartsWith("maxentrylength="))
                {
                    maxEntryLength = arg.Substring(15);
                }
            }
#endif
            return int.Parse(maxEntryLength);
        }

        /// <summary>
        /// Get the Max Local Storage Key Length, provided by command line in built app, and by 'testMaxKeyLength'
        /// variable in Editor mode.
        /// </summary>
        /// <returns>Max Local Storage Key Length.</returns>
        private int GetMaxKeyLength()
        {
            string maxKeyLength = "";

#if UNITY_EDITOR
            maxKeyLength = testMaxKeyLength;
#else
            foreach (string arg in System.Environment.GetCommandLineArgs())
            {
                if (arg.StartsWith("maxkeylength="))
                {
                    maxKeyLength = arg.Substring(13);
                }
            }
#endif
            return int.Parse(maxKeyLength);
        }

        /// <summary>
        /// Get the Files Directory, provided by command line in built app, and by 'testFilesDirectory'
        /// variable in Editor mode.
        /// </summary>
        /// <returns>Files Directory.</returns>
        private string GetFilesDirectory()
        {
            string filesDirectory = "";

#if UNITY_EDITOR
            filesDirectory = testFilesDirectory;
#else
            foreach (string arg in System.Environment.GetCommandLineArgs())
            {
                if (arg.StartsWith("filesdirectory="))
                {
                    filesDirectory = arg.Substring(15);
                }
            }
#endif
            return filesDirectory;
        }

        /// <summary>
        /// Get the Daemon Port, provided by command line in built app, and by 'testDaemonPort'
        /// variable in Editor mode.
        /// </summary>
        /// <returns>Daemon Port.</returns>
        private uint GetDaemonPort()
        {
            string daemonPort = "";
#if UNITY_EDITOR
            daemonPort = testDaemonPort;
#else
            foreach (string arg in System.Environment.GetCommandLineArgs())
            {
                if (arg.StartsWith("daemonport="))
                {
                    daemonPort = arg.Substring(11);
                }
            }
#endif
            return uint.Parse(daemonPort);
        }

        /// <summary>
        /// Get the Main App ID, provided by command line in built app, and by 'testMainAppID'
        /// variable in Editor mode.
        /// </summary>
        /// <returns>Main App ID.</returns>
        private Guid GetMainAppID()
        {
            string mainAppID = "";
#if UNITY_EDITOR
            mainAppID = testMainAppID;
#else
            foreach (string arg in System.Environment.GetCommandLineArgs())
            {
                if (arg.StartsWith("mainappid="))
                {
                    mainAppID = arg.Substring(10);
                }
            }
#endif
            return Guid.Parse(mainAppID);
        }

        /// <summary>
        /// Get the Tab ID, provided by command line in built app, and by 'testTabID'
        /// variable in Editor mode.
        /// </summary>
        /// <returns>Tab ID.</returns>
        private int GetTabID()
        {
            string tabID = "";
#if UNITY_EDITOR
            tabID = testTabID;
#else
            foreach (string arg in System.Environment.GetCommandLineArgs())
            {
                if (arg.StartsWith("tabid="))
                {
                    tabID = arg.Substring(6);
                }
            }
#endif
            return int.Parse(tabID);
        }

        /// <summary>
        /// Get the World Load Timeout, provided by command line in built app, and by 'testWorldLoadTimeout'
        /// variable in Editor mode.
        /// </summary>
        /// <returns>World Load Timeout.</returns>
        private float GetWorldLoadTimeout()
        {
            string timeout = "";
#if UNITY_EDITOR
            timeout = testWorldLoadTimeout;
#else
            foreach (string arg in System.Environment.GetCommandLineArgs())
            {
                if (arg.StartsWith("worldloadtimeout="))
                {
                    timeout = arg.Substring(17);
                }
            }
#endif
            return float.Parse(timeout);
        }

        /// <summary>
        /// Get the History, provided by command line in built app, and by 'testHistory'
        /// variable in Editor mode.
        /// </summary>
        /// <returns>History.</returns>
        private string GetHistory()
        {
            string history = "";
#if UNITY_EDITOR
            history = testHistory;
#else
            foreach (string arg in System.Environment.GetCommandLineArgs())
            {
                if (arg.StartsWith("history="))
                {
                    history = arg.Substring(8);
                }
            }
#endif
            return history;
        }
    }
}