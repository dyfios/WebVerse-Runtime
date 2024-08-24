// Copyright (c) 2019-2024 Five Squared Interactive. All rights reserved.

using System;
using UnityEngine;
using FiveSQD.WebVerse.Utilities;
using System.IO;

namespace FiveSQD.WebVerse.Runtime
{
    /// <summary>
    /// WebVerse Lightweight Mode.
    /// </summary>
    public class LightweightMode : MonoBehaviour
    {
        /// <summary>
        /// Maximum local storage entries to use in Unity Editor tests.
        /// </summary>
        [Tooltip("Maximum local storage entries to use in Unity Editor tests.")]
        public string testMaxEntries = "2048";

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
        /// URL to load in Unity Editor tests.
        /// </summary>
        [Tooltip("URL to load in Unity Editor tests.")]
        public string testWorldURL = "https://raw.githubusercontent.com/Five-Squared-Interactive/WebVerse-Samples/main/Simple-Meshes-Scene/index.veml";

        /// <summary>
        /// World Load Timeout to use in Unity Editor tests.
        /// </summary>
        [Tooltip("World Load Timeout to use in Unity Editor tests.")]
        public string testWorldLoadTimeout;

        /// <summary>
        /// WebVerse Runtime.
        /// </summary>
        [Tooltip("WebVerse Runtime.")]
        public WebVerseRuntime runtime;

        /// <summary>
        /// Load a world.
        /// </summary>
        /// <param name="uri">URI of the world to load.</param>
        public void LoadWorld(string uri)
        {
            if (runtime == null)
            {
                Logging.LogError("[LightweightMode->LoadWorld] No runtime.");
                return;
            }

            runtime.LoadURL(uri);
        }

        /// <summary>
        /// Unload the current world.
        /// </summary>
        public void UnloadWorld()
        {
            if (runtime == null)
            {
                Logging.LogError("[LightweightMode->LoadWorld] No runtime.");
                return;
            }

            runtime.UnloadWorld();
        }

        private void Awake()
        {

            LoadRuntime();
        }

        /// <summary>
        /// Load the runtime.
        /// </summary>
        private void LoadRuntime()
        {
            int maxEntries = GetMaxEntries();
            if (maxEntries <= 0 || maxEntries >= 8192)
            {
                Logging.LogError("[LightweightMode->LoadRuntime] Invalid max entries value.");
                return;
            }

            int maxEntryLength = GetMaxEntryLength();
            if (maxEntryLength <= 8 || maxEntryLength >= 131072)
            {
                Logging.LogError("[LightweightMode->LoadRuntime] Invalid max entry length value.");
                return;
            }

            int maxKeyLength = GetMaxKeyLength();
            if (maxKeyLength <= 4 || maxKeyLength >= 8192)
            {
                Logging.LogError("[LightweightMode->LoadRuntime] Invalid max key length value.");
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
                Logging.LogError("[LightweightMode->LoadRuntime] Invalid daemon port value.");
            }

            Guid mainAppID = GetMainAppID();
            if (mainAppID == Guid.Empty)
            {
                Logging.Log("[LightweightMode->LoadRuntime] Main app ID not set.");
            }

            int tabID = GetTabID();
            if (tabID < 0)
            {
                Logging.LogError("[LightweightMode->LoadRuntime] Invalid tab ID value.");
            }

            string worldURL = GetWorldURL();
            if (string.IsNullOrEmpty(worldURL))
            {
                Logging.LogError("[LightweightMode->LoadRuntime] Invalid world URL value.");
            }

            float worldLoadTimeout = GetWorldLoadTimeout();
            if (worldLoadTimeout <= 0)
            {
                Logging.LogError("[LightweightMode->LoadRuntime] Invalid world load timeout.");
                worldLoadTimeout = 120;
            }

            runtime.Initialize(LocalStorage.LocalStorageManager.LocalStorageMode.Cache,
                maxEntries, maxEntryLength, maxKeyLength, filesDirectory, daemonPort, mainAppID, tabID, worldLoadTimeout);

            if (!string.IsNullOrEmpty(worldURL))
            {
                LoadWorld(worldURL);
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
#elif UNITY_WEBGL
            int queryStart = Application.absoluteURL.IndexOf("?") + 1;
            if (queryStart > 1 && queryStart < Application.absoluteURL.Length - 1)
            {
                string query = Application.absoluteURL.Substring(queryStart);

                string[] sections = query.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string section in sections)
                {
                    int valueStart = section.IndexOf("=") + 1;
                    string[] keyValue = section.Split('=');
                    if (keyValue.Length >= 2)
                    {
                        string key = keyValue[0];
                        string value = section.Substring(valueStart);
                        if (key.ToLower() == "max_entries")
                        {
                            maxEntries = value;
                            break;
                        }
                    }
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
#elif UNITY_WEBGL
            int queryStart = Application.absoluteURL.IndexOf("?") + 1;
            if (queryStart > 1 && queryStart < Application.absoluteURL.Length - 1)
            {
                string query = Application.absoluteURL.Substring(queryStart);

                string[] sections = query.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string section in sections)
                {
                    int valueStart = section.IndexOf("=") + 1;
                    string[] keyValue = section.Split('=');
                    if (keyValue.Length >= 2)
                    {
                        string key = keyValue[0];
                        string value = section.Substring(valueStart);
                        if (key.ToLower() == "max_entry_length")
                        {
                            maxEntryLength = value;
                            break;
                        }
                    }
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
#elif UNITY_WEBGL
            int queryStart = Application.absoluteURL.IndexOf("?") + 1;
            if (queryStart > 1 && queryStart < Application.absoluteURL.Length - 1)
            {
                string query = Application.absoluteURL.Substring(queryStart);

                string[] sections = query.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string section in sections)
                {
                    int valueStart = section.IndexOf("=") + 1;
                    string[] keyValue = section.Split('=');
                    if (keyValue.Length >= 2)
                    {
                        string key = keyValue[0];
                        string value = section.Substring(valueStart);
                        if (key.ToLower() == "max_key_length")
                        {
                            maxKeyLength = value;
                            break;
                        }
                    }
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
            filesDirectory = Path.Combine(Application.dataPath, testFilesDirectory);
#elif UNITY_WEBGL
            int queryStart = Application.absoluteURL.IndexOf("?") + 1;
            if (queryStart > 1 && queryStart < Application.absoluteURL.Length - 1)
            {
                string query = Application.absoluteURL.Substring(queryStart);

                string[] sections = query.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string section in sections)
                {
                    int valueStart = section.IndexOf("=") + 1;
                    string[] keyValue = section.Split('=');
                    if (keyValue.Length >= 2)
                    {
                        string key = keyValue[0];
                        string value = section.Substring(valueStart);
                        if (key.ToLower() == "files_directory")
                        {
                            filesDirectory = value;
                            break;
                        }
                    }
                }
            }
#endif
            return filesDirectory;
        }

        private uint GetDaemonPort()
        {
            string daemonPort = "";
#if UNITY_EDITOR
            daemonPort = testDaemonPort;
#elif UNITY_WEBGL
            int queryStart = Application.absoluteURL.IndexOf("?") + 1;
            if (queryStart > 1 && queryStart < Application.absoluteURL.Length - 1)
            {
                string query = Application.absoluteURL.Substring(queryStart);

                string[] sections = query.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string section in sections)
                {
                    int valueStart = section.IndexOf("=") + 1;
                    string[] keyValue = section.Split('=');
                    if (keyValue.Length >= 2)
                    {
                        string key = keyValue[0];
                        string value = section.Substring(valueStart);
                        if (key.ToLower() == "daemon_port")
                        {
                            daemonPort = value;
                            break;
                        }
                    }
                }
            }
#endif
            return uint.Parse(daemonPort);
        }

        private Guid GetMainAppID()
        {
            string mainAppID = "";
#if UNITY_EDITOR
            mainAppID = testMainAppID;
#elif UNITY_WEBGL
            int queryStart = Application.absoluteURL.IndexOf("?") + 1;
            if (queryStart > 1 && queryStart < Application.absoluteURL.Length - 1)
            {
                string query = Application.absoluteURL.Substring(queryStart);

                string[] sections = query.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string section in sections)
                {
                    int valueStart = section.IndexOf("=") + 1;
                    string[] keyValue = section.Split('=');
                    if (keyValue.Length >= 2)
                    {
                        string key = keyValue[0];
                        string value = section.Substring(valueStart);
                        if (key.ToLower() == "main_app_id")
                        {
                            mainAppID = value;
                            break;
                        }
                    }
                }
            }
#endif
            return Guid.Parse(mainAppID);
        }

        private int GetTabID()
        {
            string tabID = "";
#if UNITY_EDITOR
            tabID = testTabID;
#elif UNITY_WEBGL
            int queryStart = Application.absoluteURL.IndexOf("?") + 1;
            if (queryStart > 1 && queryStart < Application.absoluteURL.Length - 1)
            {
                string query = Application.absoluteURL.Substring(queryStart);

                string[] sections = query.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string section in sections)
                {
                    int valueStart = section.IndexOf("=") + 1;
                    string[] keyValue = section.Split('=');
                    if (keyValue.Length >= 2)
                    {
                        string key = keyValue[0];
                        string value = section.Substring(valueStart);
                        if (key.ToLower() == "tab_id")
                        {
                            tabID = value;
                            break;
                        }
                    }
                }
            }
#endif
            return int.Parse(tabID);
        }

        private string GetWorldURL()
        {
            string worldURL = "";
#if UNITY_EDITOR
            worldURL = testWorldURL;
#elif UNITY_WEBGL
            int queryStart = Application.absoluteURL.IndexOf("?") + 1;
            if (queryStart > 1 && queryStart < Application.absoluteURL.Length - 1)
            {
                string query = Application.absoluteURL.Substring(queryStart);
                
                string[] sections = query.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string section in sections)
                {
                    int valueStart = section.IndexOf("=") + 1;
                    string[] keyValue = section.Split('=');
                    if (keyValue.Length >= 2)
                    {
                        string key = keyValue[0];
                        string value = section.Substring(valueStart);
                        if (key.ToLower() == "world_url")
                        {
                            worldURL = value;
                            break;
                        }
                    }
                }
            }
#endif
            
            return worldURL;
        }

        private float GetWorldLoadTimeout()
        {
            string timeout = "";
#if UNITY_EDITOR
            timeout = testWorldLoadTimeout;
#elif UNITY_WEBGL
            int queryStart = Application.absoluteURL.IndexOf("?") + 1;
            if (queryStart > 1 && queryStart < Application.absoluteURL.Length - 1)
            {
                string query = Application.absoluteURL.Substring(queryStart);
                
                string[] sections = query.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string section in sections)
                {
                    int valueStart = section.IndexOf("=") + 1;
                    string[] keyValue = section.Split('=');
                    if (keyValue.Length >= 2)
                    {
                        string key = keyValue[0];
                        string value = section.Substring(valueStart);
                        if (key.ToLower() == "world_load_timeout")
                        {
                            timeout = value;
                            break;
                        }
                    }
                }
            }
#endif

            return float.Parse(timeout);
        }
    }
}