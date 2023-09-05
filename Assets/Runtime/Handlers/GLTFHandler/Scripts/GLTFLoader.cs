using System;
using UnityEngine;
using Siccity.GLTFUtility;

namespace FiveSQD.WebVerse.Handlers.GLTF
{
    /// <summary>
    /// Component to load a GLTF scene with
    /// </summary>
    public class GLTFLoader : MonoBehaviour
    {
        private static GLTFLoader instance;

        private void Awake()
        {
            instance = this;
        }

        public static void LoadModelAsync(string pathToModel, Action<GameObject, AnimationClip[]> callback)
        {
#if UNITY_WEBGL
        instance.StartCoroutine(instance.LoadModelCoroutine(pathToModel, callback));
#else
            Importer.ImportGLTFAsync(pathToModel, new ImportSettings(), callback);
#endif
        }

        public static GameObject LoadModel(string pathToModel)
        {
            return Importer.LoadFromFile(pathToModel);
        }

        private System.Collections.IEnumerator LoadModelCoroutine(string pathToModel, Action<GameObject, AnimationClip[]> callback)
        {
            GameObject loadedObj = Importer.LoadFromFile(pathToModel);

            if (callback != null)
            {
                callback.Invoke(loadedObj, null);
            }

            yield return null;
        }
    }
}