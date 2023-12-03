// Copyright (c) 2019-2023 Five Squared Interactive. All rights reserved.

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
        /// <summary>
        /// Load a GLTF model asynchronously.
        /// </summary>
        /// <param name="pathToModel">Path to the GLTF model.</param>
        /// <param name="callback">Action to invoke when loading is complete. Provides reference
        /// to the loaded gameobject and any animation clips.</param>
        public void LoadModelAsync(string pathToModel, Action<GameObject, AnimationClip[]> callback)
        {
#if UNITY_WEBGL
            StartCoroutine(LoadModelCoroutine(pathToModel, callback));
#else
            Importer.ImportGLTFAsync(pathToModel, new ImportSettings(), callback);
#endif
        }

        /// <summary>
        /// Load a GLB model asynchronously.
        /// </summary>
        /// <param name="modelData">GLB model data.</param>
        /// <param name="callback">Action to invoke when loading is complete. Provides reference
        /// to the loaded gameobject and any animation clips.</param>
        public void LoadModelAsync(byte[] modelData, Action<GameObject, AnimationClip[]> callback)
        {
#if UNITY_WEBGL
            StartCoroutine(LoadModelCoroutine(modelData, callback));
#endif
        }

        /// <summary>
        /// Load a GLTF model.
        /// </summary>
        /// <param name="pathToModel">Path to the GLTF model.</param>
        public static GameObject LoadModel(string pathToModel)
        {
            return Importer.LoadFromFile(pathToModel);
        }

        /// <summary>
        /// Load a GLTF model in a coroutine.
        /// </summary>
        /// <param name="pathToModel">Path to the GLTF model.</param>
        /// <param name="callback">Action to invoke when loading is complete. Takes a reference
        /// to the loaded gameobject and any animation clips.</param>
        private System.Collections.IEnumerator LoadModelCoroutine(string pathToModel, Action<GameObject, AnimationClip[]> callback)
        {
            GameObject loadedObj = Importer.LoadFromFile(pathToModel);

            if (callback != null)
            {
                callback.Invoke(loadedObj, null);
            }

            yield return null;
        }

        /// <summary>
        /// Load a GLB model directly in a coroutine.
        /// </summary>
        /// <param name="data">GLB model data.</param>
        /// <param name="callback">Action to invoke when loading is complete. Takes a reference
        /// to the loaded gameobject and any animation clips.</param>
        private System.Collections.IEnumerator LoadModelCoroutine(byte[] data, Action<GameObject, AnimationClip[]> callback)
        {
            GameObject loadedObj = Importer.LoadFromBytes(data);

            if (callback != null)
            {
                callback.Invoke(loadedObj, null);
            }

            yield return null;
        }
    }
}