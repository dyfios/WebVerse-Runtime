using Jint;
using FiveSQD.WebVerse.Utilities;
using FiveSQD.WebVerse.WorldEngine.Utilities;
using FiveSQD.WebVerse.Handlers.Javascript.APIs.WorldTypes;
using FiveSQD.WebVerse.Handlers.Javascript.APIs.Entity;
using FiveSQD.WebVerse.Handlers.Javascript.APIs.Networking;

namespace FiveSQD.WebVerse.Handlers.Javascript
{
    public class JavascriptHandler : BaseHandler
    {
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

            // Entity.
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
            new System.Tuple<string, System.Type>("MQTTClient", typeof(MQTTClient)),
            new System.Tuple<string, System.Type>("WebSocket", typeof(WebSocket)),

            // Input.
            new System.Tuple<string, System.Type>("Input", typeof(APIs.Input.Input)),

            // VOS Synchronization.
            new System.Tuple<string, System.Type>("VOSSynchronization", typeof(APIs.VOSSynchronization.VOSSynchronization)),

            // World Browser Utilities.
            new System.Tuple<string, System.Type>("LocalStorage", typeof(APIs.Utilities.LocalStorage)),
            new System.Tuple<string, System.Type>("Logging", typeof(APIs.Utilities.Logging)),
            new System.Tuple<string, System.Type>("WorldStorage", typeof(APIs.Utilities.WorldStorage))
        };

        private Engine engine;

        public override void Initialize()
        {
            engine = new Engine();
            RegisterAllAPIs();

            base.Initialize();
        }

        public override void Terminate()
        {
            base.Terminate();
        }

        public void RunScript(string script)
        {
            if (engine == null)
            {
                LogSystem.LogError("[JavascriptHandler->RunScript] No engine reference.");
                return;
            }

            engine = engine.Execute(script);
        }

        public object Run(string logic)
        {
            if (engine == null)
            {
                LogSystem.LogError("[JavascriptManager->Run] No engine reference.");
                return null;
            }
            UnityEngine.Debug.Log(logic);
            return engine.Evaluate(logic);
        }

        public void SetValue(string variableName, bool value)
        {
            if (engine == null)
            {
                LogSystem.LogError("[JavascriptManager->SetValue] No engine reference.");
                return;
            }

            engine.SetValue(variableName, value);
        }

        public void SetValue(string variableName, byte value)
        {
            if (engine == null)
            {
                LogSystem.LogError("[JavascriptManager->SetValue] No engine reference.");
                return;
            }

            engine.SetValue(variableName, value);
        }

        public void SetValue(string variableName, byte[] value)
        {
            if (engine == null)
            {
                LogSystem.LogError("[JavascriptManager->SetValue] No engine reference.");
                return;
            }

            engine.SetValue(variableName, value);
        }

        public void SetValue(string variableName, float value)
        {
            if (engine == null)
            {
                LogSystem.LogError("[JavascriptManager->SetValue] No engine reference.");
                return;
            }

            engine.SetValue(variableName, value);
        }

        public void SetValue(string variableName, double value)
        {
            if (engine == null)
            {
                LogSystem.LogError("[JavascriptManager->SetValue] No engine reference.");
                return;
            }

            engine.SetValue(variableName, value);
        }

        public void SetValue(string variableName, int value)
        {
            if (engine == null)
            {
                LogSystem.LogError("[JavascriptManager->SetValue] No engine reference.");
                return;
            }

            engine.SetValue(variableName, value);
        }

        public void SetValue(string variableName, string value)
        {
            if (engine == null)
            {
                LogSystem.LogError("[JavascriptManager->SetValue] No engine reference.");
                return;
            }

            engine.SetValue(variableName, value);
        }

        public object GetValue(string variableName)
        {
            if (engine == null)
            {
                LogSystem.LogError("[JavascriptManager->GetValue] No engine reference.");
                return null;
            }

            return engine.GetValue(variableName);
        }

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