using UnityEngine;
using FiveSQD.WebVerse.Utilities;
using FiveSQD.WebVerse.WorldEngine;
using FiveSQD.WebVerse.LocalStorage;
using FiveSQD.WebVerse.WebInterface;
using FiveSQD.WebVerse.Handlers.File;
using FiveSQD.WebVerse.Handlers.GLTF;
using FiveSQD.WebVerse.Handlers.PNG;
using FiveSQD.WebVerse.Handlers.Javascript;
using FiveSQD.WebVerse.VOSSynchronization;

namespace FiveSQD.WebVerse.Runtime
{
    public class WebVerseRuntime : MonoBehaviour
    {
        public static WebVerseRuntime Instance;

        public WorldEngine.WorldEngine worldEngine { get; private set; }

        public FileHandler fileHandler { get; private set; }

        public PNGHandler pngHandler { get; private set; }

        public JavascriptHandler javascriptHandler { get; private set; }

        public GLTFHandler gltfHandler { get; private set; }

        public VOSSynchronizationManager vosSynchronizationManager { get; private set; }

        public LocalStorageManager localStorageManager { get; private set; }

        public void Initialize()
        {
            Instance = this;
        }

        public void Terminate()
        {

        }

        private void InitializeComponents()
        {

        }
    }
}