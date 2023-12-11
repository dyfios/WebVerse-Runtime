// Copyright (c) 2019-2023 Five Squared Interactive. All rights reserved.

using Jint;
using FiveSQD.WebVerse.Utilities;
using FiveSQD.WebVerse.WorldEngine.Utilities;
using FiveSQD.WebVerse.Handlers.Javascript.APIs.WorldTypes;
using FiveSQD.WebVerse.Handlers.Javascript.APIs.Entity;
using FiveSQD.WebVerse.Handlers.Javascript.APIs.Networking;

namespace FiveSQD.WebVerse.Handlers.Javascript
{
    /// <summary>
    /// Class for the JavaScript Handler.
    /// </summary>
    public class JavascriptHandler : BaseHandler
    {
        /// <summary>
        /// Tuple of class names and internal types.
        /// </summary>
        private static readonly System.Tuple<string, System.Type>[] apis = new System.Tuple<string, System.Type>[]
        {
            // World Types.
            new System.Tuple<string, System.Type>("Color", typeof(Color)),
            new System.Tuple<string, System.Type>("Vector2", typeof(Vector2)),
            new System.Tuple<string, System.Type>("Vector2D", typeof(Vector2D)),
            new System.Tuple<string, System.Type>("Vector2Int", typeof(Vector2Int)),
            new System.Tuple<string, System.Type>("Vector3", typeof(Vector3)),
            new System.Tuple<string, System.Type>("Vector3D", typeof(Vector3D)),
            new System.Tuple<string, System.Type>("Vector3Int", typeof(Vector3Int)),
            new System.Tuple<string, System.Type>("Vector4", typeof(Vector4)),
            new System.Tuple<string, System.Type>("Vector4D", typeof(Vector4D)),
            new System.Tuple<string, System.Type>("Vector4Int", typeof(Vector4Int)),
            new System.Tuple<string, System.Type>("Quaternion", typeof(Quaternion)),
            new System.Tuple<string, System.Type>("QuaternionD", typeof(QuaternionD)),
            new System.Tuple<string, System.Type>("UUID", typeof(UUID)),

            // Entity.
            new System.Tuple<string, System.Type>("Entity", typeof(Entity)),
            new System.Tuple<string, System.Type>("ButtonEntity", typeof(ButtonEntity)),
            new System.Tuple<string, System.Type>("CanvasEntity", typeof(CanvasEntity)),
            new System.Tuple<string, System.Type>("CharacterEntity", typeof(CharacterEntity)),
            new System.Tuple<string, System.Type>("ContainerEntity", typeof(ContainerEntity)),
            new System.Tuple<string, System.Type>("InputEntity", typeof(InputEntity)),
            new System.Tuple<string, System.Type>("LightEntity", typeof(LightEntity)),
            new System.Tuple<string, System.Type>("MeshEntity", typeof(MeshEntity)),
            new System.Tuple<string, System.Type>("TerrainEntity", typeof(TerrainEntity)),
            new System.Tuple<string, System.Type>("TextEntity", typeof(TextEntity)),
            new System.Tuple<string, System.Type>("VoxelEntity", typeof(VoxelEntity)),
            new System.Tuple<string, System.Type>("InteractionState", typeof(InteractionState)),
            new System.Tuple<string, System.Type>("EntityMotion", typeof(EntityMotion)),
            new System.Tuple<string, System.Type>("EntityPhysicalProperties", typeof(EntityPhysicalProperties)),
            new System.Tuple<string, System.Type>("LightProperties", typeof(LightProperties)),
            new System.Tuple<string, System.Type>("LightType", typeof(LightType)),
            new System.Tuple<string, System.Type>("VoxelBlockInfo", typeof(VoxelBlockInfo)),
            new System.Tuple<string, System.Type>("VoxelBlockSubType", typeof(VoxelBlockSubType)),

            // Networking.
            new System.Tuple<string, System.Type>("HTTPNetworking", typeof(HTTPNetworking)),
#if USE_WEBINTERFACE
            new System.Tuple<string, System.Type>("MQTTClient", typeof(MQTTClient)),
            new System.Tuple<string, System.Type>("WebSocket", typeof(WebSocket)),
#endif

            // Input.
            new System.Tuple<string, System.Type>("Input", typeof(APIs.Input.Input)),

#if USE_WEBINTERFACE
            // VOS Synchronization.
            new System.Tuple<string, System.Type>("VOSSynchronization", typeof(APIs.VOSSynchronization.VOSSynchronization)),
#endif

            // World Browser Utilities.
            new System.Tuple<string, System.Type>("Camera", typeof(APIs.Utilities.Camera)),
            new System.Tuple<string, System.Type>("Context", typeof(APIs.Utilities.Context)),
            new System.Tuple<string, System.Type>("LocalStorage", typeof(APIs.Utilities.LocalStorage)),
            new System.Tuple<string, System.Type>("Logging", typeof(APIs.Utilities.Logging)),
            new System.Tuple<string, System.Type>("Time", typeof(APIs.Utilities.Time)),
            new System.Tuple<string, System.Type>("WorldStorage", typeof(APIs.Utilities.WorldStorage))
        };

        /// <summary>
        /// Reference to the JS engine instance.
        /// </summary>
        private Engine engine;

        /// <summary>
        /// Initialize the JavascriptHandler.
        /// </summary>
        public override void Initialize()
        {
            engine = new Engine();
            RegisterAllAPIs();

            base.Initialize();
            EntityAPIHelper.InitializeEntityMapping();
        }

        /// <summary>
        /// Terminate the JavascriptHandler.
        /// </summary>
        public override void Terminate()
        {
            base.Terminate();
        }
        
        /// <summary>
        /// Run a script.
        /// </summary>
        /// <param name="script">The script to run.</param>
        public void RunScript(string script)
        {
            if (engine == null)
            {
                LogSystem.LogError("[JavascriptHandler->RunScript] No engine reference.");
                return;
            }

            engine = engine.Execute(script);
        }

        /// <summary>
        /// Run logic.
        /// </summary>
        /// <param name="logic">The logic to run.</param>
        /// <returns>The engine that is running the logic.</returns>
        public object Run(string logic)
        {
            if (engine == null)
            {
                LogSystem.LogError("[JavascriptManager->Run] No engine reference.");
                return null;
            }
            
            return engine.Evaluate(logic);
        }

        /// <summary>
        /// Set the value of a variable
        /// </summary>
        /// <param name="variableName">The name of the variable to set.</param>
        /// <param name="value">The value to set the variable to.</param>
        public void SetValue(string variableName, bool value)
        {
            if (engine == null)
            {
                LogSystem.LogError("[JavascriptManager->SetValue] No engine reference.");
                return;
            }

            engine.SetValue(variableName, value);
        }

        /// <summary>
        /// Set the value of a variable
        /// </summary>
        /// <param name="variableName">The name of the variable to set.</param>
        /// <param name="value">The value to set the variable to.</param>
        public void SetValue(string variableName, byte value)
        {
            if (engine == null)
            {
                LogSystem.LogError("[JavascriptManager->SetValue] No engine reference.");
                return;
            }

            engine.SetValue(variableName, value);
        }

        /// <summary>
        /// Set the value of a variable
        /// </summary>
        /// <param name="variableName">The name of the variable to set.</param>
        /// <param name="value">The value to set the variable to.</param>
        public void SetValue(string variableName, byte[] value)
        {
            if (engine == null)
            {
                LogSystem.LogError("[JavascriptManager->SetValue] No engine reference.");
                return;
            }

            engine.SetValue(variableName, value);
        }

        /// <summary>
        /// Set the value of a variable
        /// </summary>
        /// <param name="variableName">The name of the variable to set.</param>
        /// <param name="value">The value to set the variable to.</param>
        public void SetValue(string variableName, float value)
        {
            if (engine == null)
            {
                LogSystem.LogError("[JavascriptManager->SetValue] No engine reference.");
                return;
            }

            engine.SetValue(variableName, value);
        }

        /// <summary>
        /// Set the value of a variable
        /// </summary>
        /// <param name="variableName">The name of the variable to set.</param>
        /// <param name="value">The value to set the variable to.</param>
        public void SetValue(string variableName, double value)
        {
            if (engine == null)
            {
                LogSystem.LogError("[JavascriptManager->SetValue] No engine reference.");
                return;
            }

            engine.SetValue(variableName, value);
        }

        /// <summary>
        /// Set the value of a variable
        /// </summary>
        /// <param name="variableName">The name of the variable to set.</param>
        /// <param name="value">The value to set the variable to.</param>
        public void SetValue(string variableName, int value)
        {
            if (engine == null)
            {
                LogSystem.LogError("[JavascriptManager->SetValue] No engine reference.");
                return;
            }

            engine.SetValue(variableName, value);
        }

        /// <summary>
        /// Set the value of a variable
        /// </summary>
        /// <param name="variableName">The name of the variable to set.</param>
        /// <param name="value">The value to set the variable to.</param>
        public void SetValue(string variableName, string value)
        {
            if (engine == null)
            {
                LogSystem.LogError("[JavascriptManager->SetValue] No engine reference.");
                return;
            }

            engine.SetValue(variableName, value);
        }

        /// <summary>
        /// Set the value of a variable
        /// </summary>
        /// <param name="variableName">The name of the variable to set.</param>
        /// <returns>The value of the variable, or null.</returns>
        public object GetValue(string variableName)
        {
            if (engine == null)
            {
                LogSystem.LogError("[JavascriptManager->GetValue] No engine reference.");
                return null;
            }

            return engine.GetValue(variableName);
        }

        /// <summary>
        /// Set a context.
        /// </summary>
        /// <param name="contextName">Name of the context.</param>
        /// <param name="context">Context.</param>
        public void DefineContext(string contextName, object context)
        {
            if (engine == null)
            {
                LogSystem.LogError("[JavascriptManager->DefineContext] No engine reference.");
                return;
            }

            engine.SetValue("INTERNAL_CONTEXTS_" + contextName, context);
        }

        /// <summary>
        /// Get a context.
        /// </summary>
        /// <param name="contextName">Name of the context.</param>
        /// <returns>Context.</returns>
        public object GetContext(string contextName)
        {
            if (engine == null)
            {
                LogSystem.LogError("[JavascriptManager->DefineContext] No engine reference.");
                return null;
            }

            return engine.GetValue("INTERNAL_CONTEXTS_" + contextName);
        }

        /// <summary>
        /// Register the APIs.
        /// </summary>
        private void RegisterAllAPIs()
        {
            if (engine == null)
            {
                LogSystem.LogError("[JavascriptHandler->RegisterAllAPIs] No engine reference.");
                return;
            }

            foreach (System.Tuple<string, System.Type> api in apis)
            {
                RegisterAPI(api.Item1, api.Item2);
            }
        }

        /// <summary>
        /// Register an API.
        /// </summary>
        private void RegisterAPI(string name, System.Type type)
        {
            if (engine == null)
            {
                LogSystem.LogError("[JavascriptHandler->RegisterAPI] No engine reference.");
                return;
            }

            engine.SetValue(name, type);
        }
    }
}