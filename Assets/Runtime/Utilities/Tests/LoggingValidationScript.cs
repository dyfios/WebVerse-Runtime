// Copyright (c) 2019-2025 Five Squared Interactive. All rights reserved.

using UnityEngine;
using FiveSQD.WebVerse.Utilities;
using FiveSQD.WebVerse.Runtime;
using System;
using System.Collections.Generic;

namespace FiveSQD.WebVerse.Utilities.Tests
{
    /// <summary>
    /// Simple validation script to test logging configuration functionality.
    /// </summary>
    public class LoggingValidationScript : MonoBehaviour
    {
        /// <summary>
        /// Test the logging configuration system.
        /// </summary>
        public void TestLoggingConfiguration()
        {
            Debug.Log("=== Starting Logging Configuration Test ===");

            // Test 1: Default configuration
            var defaultConfig = LoggingConfiguration.CreateDefault();
            Logging.SetConfiguration(defaultConfig);
            Debug.Log("✓ Default configuration applied");

            // Test 2: Production configuration  
            var productionConfig = LoggingConfiguration.CreateProduction();
            Logging.SetConfiguration(productionConfig);
            Debug.Log("✓ Production configuration applied");

            // Test 3: Custom configuration
            var customConfig = new LoggingConfiguration
            {
                enableConsoleOutput = true,
                enableDefault = true,
                enableDebug = false,
                enableWarning = true,
                enableError = true,
                enableScriptDefault = true,
                enableScriptDebug = false,
                enableScriptWarning = true,
                enableScriptError = true
            };
            Logging.SetConfiguration(customConfig);
            Debug.Log("✓ Custom configuration applied");

            // Test 4: Verify configuration getter
            var retrievedConfig = Logging.GetConfiguration();
            if (retrievedConfig.enableDefault == customConfig.enableDefault &&
                retrievedConfig.enableDebug == customConfig.enableDebug &&
                retrievedConfig.enableWarning == customConfig.enableWarning)
            {
                Debug.Log("✓ Configuration getter works correctly");
            }
            else
            {
                Debug.LogError("✗ Configuration getter failed");
            }

            // Test 5: Type checking
            if (Logging.IsLogTypeEnabled(Logging.Type.Default) &&
                !Logging.IsLogTypeEnabled(Logging.Type.Debug) &&
                Logging.IsLogTypeEnabled(Logging.Type.Warning))
            {
                Debug.Log("✓ Log type checking works correctly");
            }
            else
            {
                Debug.LogError("✗ Log type checking failed");
            }

            // Test 6: Message filtering
            List<string> capturedMessages = new List<string>();
            Action<string, Logging.Type> callback = (msg, type) => capturedMessages.Add(msg);
            Logging.RegisterCallback(callback);

            Logging.Log("This should appear", Logging.Type.Default);
            Logging.Log("This should NOT appear", Logging.Type.Debug);
            Logging.Log("This should appear", Logging.Type.Warning);

            Logging.RemoveCallback(callback);

            if (capturedMessages.Count == 2 &&
                capturedMessages[0] == "This should appear" &&
                capturedMessages[1] == "This should appear")
            {
                Debug.Log("✓ Message filtering works correctly");
            }
            else
            {
                Debug.LogError("✗ Message filtering failed. Got " + capturedMessages.Count + " messages");
            }

            Debug.Log("=== Logging Configuration Test Complete ===");
        }

        /// <summary>
        /// Test WebVerseRuntime integration.
        /// </summary>
        public void TestWebVerseRuntimeIntegration()
        {
            Debug.Log("=== Testing WebVerseRuntime Integration ===");

            var settings = new WebVerseRuntime.RuntimeSettings
            {
                storageMode = "cache",
                maxEntries = 1024,
                maxEntryLength = 8192,
                maxKeyLength = 512,
                filesDirectory = Application.temporaryDataPath + "/test",
                timeout = 60,
                loggingConfiguration = LoggingConfiguration.CreateProduction()
            };

            Debug.Log("✓ RuntimeSettings with logging configuration created successfully");
            Debug.Log("=== WebVerseRuntime Integration Test Complete ===");
        }

        private void Start()
        {
            // Run tests when script starts
            TestLoggingConfiguration();
            TestWebVerseRuntimeIntegration();
        }
    }
}