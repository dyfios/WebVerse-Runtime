// Copyright (c) 2019-2025 Five Squared Interactive. All rights reserved.

using NUnit.Framework;
using FiveSQD.WebVerse.Utilities;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace FiveSQD.WebVerse.Utilities.Tests
{
    /// <summary>
    /// Tests for the logging configuration system.
    /// </summary>
    public class LoggingConfigurationTests
    {
        private List<string> capturedMessages;
        private List<Logging.Type> capturedTypes;
        private Action<string, Logging.Type> testCallback;

        [SetUp]
        public void SetUp()
        {
            capturedMessages = new List<string>();
            capturedTypes = new List<Logging.Type>();
            testCallback = (message, type) =>
            {
                capturedMessages.Add(message);
                capturedTypes.Add(type);
            };
            Logging.RegisterCallback(testCallback);
        }

        [TearDown]
        public void TearDown()
        {
            Logging.RemoveCallback(testCallback);
            // Reset to default configuration
            Logging.SetConfiguration(LoggingConfiguration.CreateDefault());
        }

        [Test]
        public void TestDefaultConfiguration()
        {
            var config = LoggingConfiguration.CreateDefault();
            
            Assert.IsTrue(config.enableConsoleOutput);
            Assert.IsTrue(config.enableDefault);
            Assert.IsTrue(config.enableDebug);
            Assert.IsTrue(config.enableWarning);
            Assert.IsTrue(config.enableError);
            Assert.IsTrue(config.enableScriptDefault);
            Assert.IsTrue(config.enableScriptDebug);
            Assert.IsTrue(config.enableScriptWarning);
            Assert.IsTrue(config.enableScriptError);
        }

        [Test]
        public void TestProductionConfiguration()
        {
            var config = LoggingConfiguration.CreateProduction();
            
            Assert.IsFalse(config.enableConsoleOutput);
            Assert.IsFalse(config.enableDefault);
            Assert.IsFalse(config.enableDebug);
            Assert.IsTrue(config.enableWarning);
            Assert.IsTrue(config.enableError);
            Assert.IsFalse(config.enableScriptDefault);
            Assert.IsFalse(config.enableScriptDebug);
            Assert.IsTrue(config.enableScriptWarning);
            Assert.IsTrue(config.enableScriptError);
        }

        [Test]
        public void TestLoggingConfigurationFiltering()
        {
            // Create a configuration that only allows errors and warnings
            var config = new LoggingConfiguration
            {
                enableConsoleOutput = true,
                enableDefault = false,
                enableDebug = false,
                enableWarning = true,
                enableError = true,
                enableScriptDefault = false,
                enableScriptDebug = false,
                enableScriptWarning = true,
                enableScriptError = true
            };

            Logging.SetConfiguration(config);

            // Test that filtered messages don't get through
            Logging.Log("Default message", Logging.Type.Default);
            Logging.Log("Debug message", Logging.Type.Debug);
            
            // Test that allowed messages do get through
            Logging.Log("Warning message", Logging.Type.Warning);
            Logging.Log("Error message", Logging.Type.Error);
            Logging.Log("Script warning", Logging.Type.ScriptWarning);
            Logging.Log("Script error", Logging.Type.ScriptError);

            // Should only capture the 4 allowed messages
            Assert.AreEqual(4, capturedMessages.Count);
            Assert.AreEqual("Warning message", capturedMessages[0]);
            Assert.AreEqual("Error message", capturedMessages[1]);
            Assert.AreEqual("Script warning", capturedMessages[2]);
            Assert.AreEqual("Script error", capturedMessages[3]);
        }

        [Test]
        public void TestLogTypeEnabledCheck()
        {
            var config = new LoggingConfiguration
            {
                enableDefault = true,
                enableDebug = false,
                enableWarning = true,
                enableError = true,
                enableScriptDefault = false,
                enableScriptDebug = false,
                enableScriptWarning = true,
                enableScriptError = true
            };

            Logging.SetConfiguration(config);

            Assert.IsTrue(Logging.IsLogTypeEnabled(Logging.Type.Default));
            Assert.IsFalse(Logging.IsLogTypeEnabled(Logging.Type.Debug));
            Assert.IsTrue(Logging.IsLogTypeEnabled(Logging.Type.Warning));
            Assert.IsTrue(Logging.IsLogTypeEnabled(Logging.Type.Error));
            Assert.IsFalse(Logging.IsLogTypeEnabled(Logging.Type.ScriptDefault));
            Assert.IsFalse(Logging.IsLogTypeEnabled(Logging.Type.ScriptDebug));
            Assert.IsTrue(Logging.IsLogTypeEnabled(Logging.Type.ScriptWarning));
            Assert.IsTrue(Logging.IsLogTypeEnabled(Logging.Type.ScriptError));
        }

        [Test]
        public void TestConsoleOutputDisabling()
        {
            var config = LoggingConfiguration.CreateDefault();
            config.enableConsoleOutput = false;
            Logging.SetConfiguration(config);

            // Messages should still reach callbacks even if console output is disabled
            Logging.Log("Test message", Logging.Type.Default);
            
            Assert.AreEqual(1, capturedMessages.Count);
            Assert.AreEqual("Test message", capturedMessages[0]);
        }

        [Test]
        public void TestGetSetConfiguration()
        {
            var config = LoggingConfiguration.CreateProduction();
            Logging.SetConfiguration(config);
            
            var retrievedConfig = Logging.GetConfiguration();
            
            Assert.AreEqual(config.enableConsoleOutput, retrievedConfig.enableConsoleOutput);
            Assert.AreEqual(config.enableDefault, retrievedConfig.enableDefault);
            Assert.AreEqual(config.enableDebug, retrievedConfig.enableDebug);
            Assert.AreEqual(config.enableWarning, retrievedConfig.enableWarning);
            Assert.AreEqual(config.enableError, retrievedConfig.enableError);
        }
    }
}