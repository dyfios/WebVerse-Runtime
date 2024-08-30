// Copyright (c) 2019-2024 Five Squared Interactive. All rights reserved.

using FiveSQD.WebVerse.Utilities;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FiveSQD.WebVerse.Interface.Console
{
    /// <summary>
    /// Class for console.
    /// </summary>
    public class Console : MonoBehaviour
    {
        /// <summary>
        /// Class for a console message.
        /// </summary>
        private class ConsoleMessage
        {
            /// <summary>
            /// Message.
            /// </summary>
            public string message;

            /// <summary>
            /// Message type.
            /// </summary>
            public Logging.Type type;

            /// <summary>
            /// Timestamp.
            /// </summary>
            public DateTime timestamp;

            /// <summary>
            /// Constructor for a console message.
            /// </summary>
            /// <param name="message">Message.</param>
            /// <param name="type">Message type.</param>
            public ConsoleMessage(string message, Logging.Type type, DateTime timestamp)
            {
                this.message = message;
                this.type = type;
                this.timestamp = timestamp;
            }
        }

        /// <summary>
        /// The console text.
        /// </summary>
        public TMP_Text consoleText;

        /// <summary>
        /// The warning toggle.
        /// </summary>
        public Toggle warningToggle;

        /// <summary>
        /// The normal toggle.
        /// </summary>
        public Toggle normalToggle;

        /// <summary>
        /// The debug toggle.
        /// </summary>
        public Toggle debugToggle;

        /// <summary>
        /// The font size slider.
        /// </summary>
        public Slider fontSizeSlider;

        /// <summary>
        /// The minimum font size.
        /// </summary>
        public float minimumFontSize = 5;

        /// <summary>
        /// The maximum font size.
        /// </summary>
        public float maximumFontSize = 30;

        /// <summary>
        /// The default font size slider value.
        /// </summary>
        public float defaultFontSliderValue = 0.36f;

        /// <summary>
        /// Console messages.
        /// </summary>
        private List<ConsoleMessage> consoleMessages;

        /// <summary>
        /// Initialize console.
        /// </summary>
        public void Initialize()
        {
            consoleMessages = new List<ConsoleMessage>();
            fontSizeSlider.value = defaultFontSliderValue;
            ResizeConsoleText();
        }

        /// <summary>
        /// Terminate console.
        /// </summary>
        public void Terminate()
        {
            consoleMessages.Clear();
        }

        /// <summary>
        /// Return from console.
        /// </summary>
        public void Return()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Log a console message.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="type">Type of message.</param>
        public void LogConsoleMessage(string message, Logging.Type type)
        {
            ConsoleMessage newMsg = new ConsoleMessage(message, type, DateTime.Now);
            consoleMessages.Add(newMsg);

            AddMessageToConsole(newMsg);
        }

        /// <summary>
        /// Reload the console.
        /// </summary>
        public void ReloadConsole()
        {
            consoleText.text = "";

            foreach (ConsoleMessage message in consoleMessages)
            {
                AddMessageToConsole(message);
            }
        }

        /// <summary>
        /// Resize the console text.
        /// </summary>
        public void ResizeConsoleText()
        {
            consoleText.fontSize = minimumFontSize + fontSizeSlider.value * (maximumFontSize - minimumFontSize);
        }

        /// <summary>
        /// Add a message to console.
        /// </summary>
        /// <param name="message">Message to add to console.</param>
        private void AddMessageToConsole(ConsoleMessage message)
        {
            switch (message.type)
            {
                case Logging.Type.Error:
                    AppendConsoleWithMessage(message);
                    break;

                case Logging.Type.Warning:
                    if (warningToggle.isOn)
                    {
                        AppendConsoleWithMessage(message);
                    }
                    break;

                case Logging.Type.Default:
                    if (normalToggle.isOn)
                    {
                        AppendConsoleWithMessage(message);
                    }
                    break;

                case Logging.Type.Debug:
                    if (debugToggle.isOn)
                    {
                        AppendConsoleWithMessage(message);
                    }
                    break;

                default:
                    Debug.LogError("[Console->AddMessageToConsole] Invalid message type.");
                    break;
            }
        }

        /// <summary>
        /// Append the console with a message.
        /// </summary>
        /// <param name="message">Message to append.</param>
        private void AppendConsoleWithMessage(ConsoleMessage message)
        {
            consoleText.text = consoleText.text + "\n" + message.timestamp.ToString()
                + ": [" +
                (message.type == Logging.Type.Error ? "Err" :
                message.type == Logging.Type.Warning ? "Warn" :
                message.type == Logging.Type.Default ? "Msg" : "Deb") + "] " + message.message;
        }
    }
}