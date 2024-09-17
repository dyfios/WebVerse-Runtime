// Copyright (c) 2019-2023 Five Squared Interactive. All rights reserved.

using System;
using System.Collections.Generic;
using UnityEngine;

namespace FiveSQD.WebVerse.Utilities
{
    /// <summary>
    /// Log system for the World Browser.
    /// </summary>
    public class Logging
    {
        /// <summary>
        /// Type for a log message.
        /// </summary>
        public enum Type { Default, Debug, Warning, Error };

        /// <summary>
        /// Log callbacks being maintained.
        /// </summary>
        private static List<Action<string, Type>> callbacks = new List<Action<string, Type>>();

        /// <summary>
        /// Log a message.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="type">Type of the message.</param>
        public static void Log(string message, Type type = Type.Default)
        {
            // Forward to Unity's Logging System.
            switch (type)
            {
                case Type.Debug:
                    // TODO: Only in development build.
                    Debug.Log(message);
                    break;

                case Type.Warning:
                    Debug.LogWarning(message);
                    break;

                case Type.Error:
                    Debug.LogError(message);
                    break;

                case Type.Default:
                default:
                    Debug.Log(message);
                    break;
            }

            // Forward to callbacks.
            foreach (Action<string, Type> callback in callbacks)
            {
                if (callback != null)
                {
                    callback.Invoke(message, type);
                }
            }
        }

        /// <summary>
        /// Log a debug message.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public static void LogDebug(string message)
        {
            Log(message, Type.Debug);
        }

        /// <summary>
        /// Log a warning message.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public static void LogWarning(string message)
        {
            Log(message, Type.Warning);
        }

        /// <summary>
        /// Log an error message.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public static void LogError(string message)
        {
            Log(message, Type.Error);
        }

        /// <summary>
        /// Register a log callback.
        /// </summary>
        /// <param name="callback">Callback to register.</param>
        public static void RegisterCallback(Action<string, Type> callback)
        {
            callbacks.Add(callback);
        }

        /// <summary>
        /// Remove a log callback.
        /// </summary>
        /// <param name="callback">Callback to remove.</param>
        public static void RemoveCallback(Action<string, Type> callback)
        {
            if (callbacks.Contains(callback))
            {
                callbacks.Remove(callback);
            }
        }
    }
}