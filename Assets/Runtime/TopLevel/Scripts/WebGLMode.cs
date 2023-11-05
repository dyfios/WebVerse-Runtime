// Copyright (c) 2019-2023 Five Squared Interactive. All rights reserved.

using System;
using UnityEngine;
using FiveSQD.WebVerse.Utilities;

namespace FiveSQD.WebVerse.Runtime
{
    /// <summary>
    /// WebVerse WebGL Mode.
    /// </summary>
    public class WebGLMode : MonoBehaviour
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
                Logging.LogError("[WebGLMode->LoadWorld] No runtime.");
                return;
            }

            runtime.LoadWorld(uri);
        }

        /// <summary>
        /// Unload the current world.
        /// </summary>
        public void UnloadWorld()
        {
            if (runtime == null)
            {
                Logging.LogError("[WebGLMode->LoadWorld] No runtime.");
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
                Logging.LogError("[WebGLMode->LoadRuntime] Invalid max entries value.");
                return;
            }

            int maxEntryLength = GetMaxEntryLength();
            if (maxEntryLength <= 8 || maxEntryLength >= 131072)
            {
                Logging.LogError("[WebGLMode->LoadRuntime] Invalid max entry length value.");
                return;
            }

            int maxKeyLength = GetMaxKeyLength();
            if (maxKeyLength <= 4 || maxKeyLength >= 8192)
            {
                Logging.LogError("[WebGLMode->LoadRuntime] Invalid max key length value.");
                return;
            }

            uint daemonPort = GetDaemonPort();
            if (daemonPort <= 0 || daemonPort >= 65535)
            {
                Logging.LogError("[WebGLMode->LoadRuntime] Invalid daemon port value.");
            }

            Guid mainAppID = GetMainAppID();
            if (mainAppID == Guid.Empty)
            {
                Logging.LogError("[WebGLMode->LoadRuntime] Invalid main app ID value.");
            }

            runtime.Initialize(LocalStorage.LocalStorageManager.LocalStorageMode.Cache,
                maxEntries, maxEntryLength, maxKeyLength, daemonPort, mainAppID);
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
                        if (key.ToLower() == "maxentries")
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
                        if (key.ToLower() == "maxentrylength")
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
                        if (key.ToLower() == "maxkeylength")
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
                        if (key.ToLower() == "daemonport")
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
                        if (key.ToLower() == "mainappid")
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
    }
}