// Copyright (c) 2019-2023 Five Squared Interactive. All rights reserved.

using FiveSQD.WebVerse.Runtime;
using UnityEngine;

namespace FiveSQD.WebVerse.Handlers.Javascript.APIs.Input
{
    /// <summary>
    /// Input methods.
    /// </summary>
    public class Input
    {
        /// <summary>
        /// Get the current move value.
        /// </summary>
        /// <returns>A Vector2 representation of the current move value.</returns>
        public static Vector2 GetMoveValue()
        {
            return WebVerseRuntime.Instance.inputManager.moveValue;
        }

        /// <summary>
        /// Get the current look value.
        /// </summary>
        /// <returns>A Vector2 representation of the current look value.</returns>
        public static Vector2 GetLookValue()
        {
            return WebVerseRuntime.Instance.inputManager.lookValue;
        }

        /// <summary>
        /// Get the current pressed state of a key.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>Whether or not the key is pressed.</returns>
        public static bool GetKeyValue(string key)
        {
            return WebVerseRuntime.Instance.inputManager.pressedKeys.Contains(key);
        }

        /// <summary>
        /// Get the current pressed state of a key.
        /// </summary>
        /// <param name="keycode">The keycode to check.</param>
        /// <returns>Whether or not the key is pressed.</returns>
        public static bool GetKeyCodeValue(string keycode)
        {
            return WebVerseRuntime.Instance.inputManager.pressedKeyCodes.Contains(keycode);
        }

        /// <summary>
        /// Get the current pressed state of the left mouse button.
        /// </summary>
        /// <returns>Whether or not the left mouse button is pressed.</returns>
        public static bool GetLeft()
        {
            return WebVerseRuntime.Instance.inputManager.leftValue;
        }

        /// <summary>
        /// Get the current pressed state of the middle mouse button.
        /// </summary>
        /// <returns>Whether or not the middle mouse button is pressed.</returns>
        public static bool GetMiddle()
        {
            return WebVerseRuntime.Instance.inputManager.middleValue;
        }

        /// <summary>
        /// Get the current pressed state of the right mouse button.
        /// </summary>
        /// <returns>Whether or not the right mouse button is pressed.</returns>
        public static bool GetRight()
        {
            return WebVerseRuntime.Instance.inputManager.rightValue;
        }
    }
}