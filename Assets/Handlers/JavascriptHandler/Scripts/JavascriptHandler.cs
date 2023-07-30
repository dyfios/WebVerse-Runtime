using Jint;
using FiveSQD.WebVerse.Utilities;
using FiveSQD.WebVerse.WorldEngine.Utilities;
using FiveSQD.WebVerse.Handlers.Javascript.APIs.WorldTypes;

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

            engine.Execute(script);
        }

        public object Run(string logic)
        {
            if (engine == null)
            {
                LogSystem.LogError("[JavascriptManager->Run] No engine reference.");
                return null;
            }

            return engine.Evaluate(logic);
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