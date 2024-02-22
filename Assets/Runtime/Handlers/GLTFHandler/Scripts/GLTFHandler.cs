// Copyright (c) 2019-2024 Five Squared Interactive. All rights reserved.

using System.Collections.Generic;
using UnityEngine;
using FiveSQD.WebVerse.Utilities;
using FiveSQD.WebVerse.Handlers.File;
using System;
using FiveSQD.WebVerse.Runtime;
#if USE_WEBINTERFACE
using FiveSQD.WebVerse.WebInterface.HTTP;
#endif
using FiveSQD.WebVerse.WorldEngine.Entity;
using System.Collections;
using UnityEditor;

namespace FiveSQD.WebVerse.Handlers.GLTF
{
    /// <summary>
    /// WebVerse's GLTF Handler.
    /// </summary>
    public class GLTFHandler : BaseHandler
    {
        /// <summary>
        /// The WebVerse runtime.
        /// </summary>
        public WebVerseRuntime runtime;

        /// <summary>
        /// Container to use for mesh prefabs.
        /// </summary>
        public GameObject meshPrefabContainer;

        /// <summary>
        /// Position to apply to mesh prefabs.
        /// </summary>
        private static readonly Vector3 prefabLocation = new Vector3(9999, 9999, 9999);

        /// <summary>
        /// Dictionary of mesh paths and their corresponding prefabs.
        /// </summary>
        private Dictionary<string, GameObject> gltfMeshPrefabs = new Dictionary<string, GameObject>();

        /// <summary>
        /// The GLTF loader.
        /// </summary>
        private GLTFLoader gltfLoader;

        public override void Initialize()
        {
            gltfLoader = gameObject.AddComponent<GLTFLoader>();
            base.Initialize();
        }

        /// <summary>
        /// Load a GLTF resource as a mesh entity.
        /// </summary>
        /// <param name="gltfResourceURI">URI of the top-level GLTF resource.</param>
        /// <param name="resourceURIs">URIs of resources needed by the top-level GLTF.</param>
        /// <param name="id">ID of the mesh entity.</param>
        /// <param name="onLoaded">Action to invoke when loading is complete. Provides reference
        /// to the loaded mesh entity.</param>
        /// <param name="timeout">Timeout period after which loading will be aborted.</param>
        /// <returns>ID of the mesh entity being loaded.</returns>
        public Guid LoadGLTFResourceAsMeshEntity(string gltfResourceURI, string[] resourceURIs,
            Guid? id = null, Action<MeshEntity> onLoaded = null, float timeout = 10)
        {
#if !UNITY_WEBGL
            Dictionary<string, bool> downloadedState = new Dictionary<string, bool>();

            if (resourceURIs != null)
            {
                foreach (string resourceURI in resourceURIs)
                {
                    if (!string.IsNullOrEmpty(resourceURI))
                    {
                        downloadedState.Add(resourceURI, false);
                        Action onResouceDownloaded = () =>
                        {
                            downloadedState[resourceURI] = true;
                        };

                        DownloadGLTFResource(resourceURI, onResouceDownloaded);
                    }
                }
            }
            Guid guid = id.HasValue ? id.Value : Guid.NewGuid();
            Action onDownloaded = () =>
            {
                LoadGLTF(
                    System.IO.Path.Combine(runtime.fileHandler.fileDirectory, FileHandler.ToFileURI(gltfResourceURI)),
                    new Action<GameObject>((meshObject) =>
                    {
                        SetUpLoadedGLTFMeshAsMeshEntity(meshObject, guid, onLoaded);
                    }));
            };

            DownloadGLTFResource(gltfResourceURI, onDownloaded);
            return guid;
#else
            Guid guid = id.HasValue ? id.Value : Guid.NewGuid();
            Action<byte[]> onDownloaded = (data) =>
            {
                LoadGLTF(gltfResourceURI.Substring(gltfResourceURI.LastIndexOf("/")), data,
                    new Action<GameObject>((meshObject) =>
                    {
                        SetUpLoadedGLTFMeshAsMeshEntity(meshObject, guid, onLoaded);
                    }));
            };

            DownloadGLTFDirect(gltfResourceURI, onDownloaded);
            return guid;
#endif
        }

        /// <summary>
        /// Download a GLTF resource.
        /// </summary>
        /// <param name="uri">URI of the GLTF resource.</param>
        /// <param name="onDownloaded">Action to invoke when downloading is complete.</param>
        /// <param name="reDownload">Whether or not to redownload the resource if it already
        /// exists locally.</param>
        public void DownloadGLTFResource(string uri, Action onDownloaded, bool reDownload = false)
        {
#if USE_WEBINTERFACE
            if (reDownload == false)
            {
                if (runtime.fileHandler.FileExistsInFileDirectory(FileHandler.ToFileURI(uri)))
                {
                    Logging.Log("[GLTFHandler->DownloadGLTF] File " + uri + " already exists. Using stored version.");
                    onDownloaded.Invoke();
                    return;
                }
            }

            Action<int, byte[]> onDownloadedAction = new Action<int, byte[]>((code, data) =>
            {
                FinishGLTFDownload(uri, code, data);
                onDownloaded.Invoke();
            });

            HTTPRequest request = new HTTPRequest(uri, HTTPRequest.HTTPMethod.Get, onDownloadedAction);
            request.Send();
#endif
        }

        /// <summary>
        /// Download a GLTF model and load directly.
        /// </summary>
        /// <param name="uri">URI of the GLTF resource.</param>
        /// <param name="onDownloaded">Action to invoke when downloading is complete.</param>
        /// <param name="reDownload">Whether or not to redownload the resource if it already
        /// exists locally.</param>
        public void DownloadGLTFDirect(string uri, Action<byte[]> onDownloaded)
        {
#if USE_WEBINTERFACE
            Action<int, byte[]> onDownloadedAction = new Action<int, byte[]>((code, data) =>
            {
                onDownloaded.Invoke(data);
            });
            HTTPRequest request = new HTTPRequest(uri, HTTPRequest.HTTPMethod.Get, onDownloadedAction);
            request.Send();
#endif
        }

        /// <summary>
        /// Load a local GLTF resource.
        /// </summary>
        /// <param name="path">path to the GLTF resource (relative to the file directory).</param>
        /// <param name="onLoaded">Action to invoke when loading is complete. Provides reference
        /// to the loaded gameobject.</param>
        public void LoadGLTF(string path, Action<GameObject> onLoaded)
        {
            if (gltfMeshPrefabs.ContainsKey(path))
            {
                InstantiateMeshFromPrefab(gltfMeshPrefabs[path], onLoaded);
            }
            else
            {
                if (!runtime.fileHandler.FileExistsInFileDirectory(path))
                {
                    Logging.LogWarning("[GLTFHandler->LoadGLTF] File not found: " + path);
                    return;
                }
                
                Action<GameObject, AnimationClip[]> callback =
                    (GameObject go, AnimationClip[] ac) => { MeshLoadedFromGLTF(path, go, ac, onLoaded); };
                gltfLoader.LoadModelAsync(path, callback);
            }
        }

        /// <summary>
        /// Load a GLB resource directly.
        /// </summary>
        /// <param name="modelName">GLB model name.</param>
        /// <param name="modelData">GLB resource data.</param>
        /// <param name="onLoaded">Action to invoke when loading is complete. Provides reference
        /// to the loaded gameobject.</param>
        public void LoadGLTF(string modelName, byte[] modelData, Action<GameObject> onLoaded)
        {
            if (gltfMeshPrefabs.ContainsKey(modelName))
            {
                InstantiateMeshFromPrefab(gltfMeshPrefabs[modelName], onLoaded);
            }
            else
            {
                Action<GameObject, AnimationClip[]> callback =
                    (GameObject go, AnimationClip[] ac) => { MeshLoadedFromGLTF(modelName, go, ac, onLoaded); };
                gltfLoader.LoadModelAsync(modelData, callback);
            }
        }

        /// <summary>
        /// Invoked when a mesh is loaded from a GLTF.
        /// </summary>
        /// <param name="path">Path to the GLTF resource.</param>
        /// <param name="result">Resultant gameobject or null.</param>
        /// <param name="clips">Animation clips associated with the GLTF.</param>
        /// <param name="callback">Callback to invoke. Takes a reference to the loaded gameobject.</param>
        private void MeshLoadedFromGLTF(string path, GameObject result, AnimationClip[] clips, Action<GameObject> callback)
        {
            if (gltfMeshPrefabs.ContainsKey(path))
            {
                gltfMeshPrefabs[path] = result;
            }
            else
            {
                gltfMeshPrefabs.Add(path, result);
            }
            result.transform.position = prefabLocation;
            SetUpMeshPrefab(result);
            callback.Invoke(result);
            //InstantiateMeshFromPrefab(result, callback);
        }

        /// <summary>
        /// Invoked to finish the downloading of a GLTF resource post-receiving an HTTP response.
        /// </summary>
        /// <param name="uri">URI of the GLTF resource.</param>
        /// <param name="responseCode">Received response code.</param>
        /// <param name="rawData">Data received in the response.</param>
        private void FinishGLTFDownload(string uri, int responseCode, byte[] rawData)
        {
            Logging.Log("[GLTFHandler->FinishGLTFDownload] Got response " + responseCode + " for request " + uri);

            if (responseCode != 200)
            {
                Logging.Log("[GLTFHandler->FinishGLTFDownload] Error loading file.");
                return;
            }

            string filePath = FileHandler.ToFileURI(uri);
            if (runtime.fileHandler.FileExistsInFileDirectory(filePath))
            {
                runtime.fileHandler.DeleteFileInFileDirectory(filePath);
            }
            runtime.fileHandler.CreateFileInFileDirectory(filePath, rawData);
        }

        /// <summary>
        /// Instantiate a mesh from an existing prefab.
        /// </summary>
        /// <param name="prefab">Prefab to load instance of.</param>
        /// <param name="callback">Callback to invoke when instantiation is complete.
        /// Takes a reference to the loaded gameobject.</param>
        /// <param name="timeout">Timeout period after which loading will be aborted.</param>
        private void InstantiateMeshFromPrefab(GameObject prefab, Action<GameObject> callback)
        {
            GameObject loadedMesh = Instantiate(prefab);
            loadedMesh.SetActive(true);
            callback.Invoke(loadedMesh);
        }

        /// <summary>
        /// Set up a gameobject as a mesh prefab.
        /// </summary>
        /// <param name="prefab">GameObject to set up as a mesh prefab.</param>
        private void SetUpMeshPrefab(GameObject prefab)
        {
            prefab.SetActive(false);
            prefab.transform.SetParent(meshPrefabContainer.transform);
            MeshFilter[] meshFilters = prefab.GetComponentsInChildren<MeshFilter>();
            List<Mesh> meshes = new List<Mesh>();
            if (meshFilters.Length > 0)
            {
                foreach (MeshFilter meshFilter in meshFilters)
                {
                    meshes.Add(meshFilter.mesh);

                    GameObject mcGO = new GameObject("MeshCollider");
                    mcGO.transform.SetParent(meshFilter.transform);
                    mcGO.transform.localPosition = Vector3.zero;
                    mcGO.transform.localRotation = Quaternion.identity;
                    mcGO.transform.localScale = Vector3.one;
                    MeshCollider meshCollider = mcGO.AddComponent<MeshCollider>();
                    meshCollider.sharedMesh = meshFilter.mesh;
                    meshCollider.tag = WorldEngine.Tags.TagManager.meshColliderTag;
                }
            }
            else
            {
                Logging.LogWarning("[GLTFHandler->SetUpMeshPrefab] Unable to set up mesh.");
                return;
            }

            Bounds bounds = new Bounds(prefab.transform.position, Vector3.zero);
            MeshRenderer[] rends = prefab.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer rend in rends)
            {
                bounds.Encapsulate(rend.bounds);
            }

            bounds.center = bounds.center - prefab.transform.position;

            GameObject bcGO = new GameObject("BoxCollider");
            bcGO.transform.SetParent(prefab.transform);
            bcGO.transform.localPosition = Vector3.zero;
            BoxCollider boxCollider = bcGO.AddComponent<BoxCollider>();
            boxCollider.tag = WorldEngine.Tags.TagManager.physicsColliderTag;
            boxCollider.center = bounds.center;
            boxCollider.size = bounds.size;

            Rigidbody rigidbody = prefab.AddComponent<Rigidbody>();
            rigidbody.isKinematic = true;
        }

        /// <summary>
        /// Set up a loaded GLTF as a mesh entity.
        /// </summary>
        /// <param name="loadedMesh">Loaded gameobject containing mesh.</param>
        /// <param name="guid">ID of the mesh entity.</param>
        /// <param name="onLoaded">Action to invoke when setup is complete. Takes a reference
        /// to the set up mesh entity.</param>
        private void SetUpLoadedGLTFMeshAsMeshEntity(GameObject loadedMesh, Guid guid, Action<MeshEntity> onLoaded)
        {
            Action onLoad = () =>
            {
                BaseEntity entity = WorldEngine.WorldEngine.ActiveWorld.entityManager.FindEntity(guid);
                if (entity == null)
                {
                    Logging.LogError("[GLTFHandler->SetUpLoadedGLTFMeshAsMeshEntity] Unable to find loaded entity.");
                    return;
                }

                if (onLoaded != null)
                {
                    onLoaded.Invoke((MeshEntity) entity);
                }
            };

            WorldEngine.WorldEngine.ActiveWorld.entityManager.LoadMeshEntity(
                null, loadedMesh, Vector3.zero, Quaternion.identity, guid, null, onLoad);
        }

        /// <summary>
        /// Load a GLTF as a mesh entity in the background using a coroutine.
        /// </summary>
        /// <param name="guid">ID of the mesh entity.</param>
        /// <param name="gltfResourceURI">URI of the top-level GLTF resource.</param>
        /// <param name="resourceDownloadStates">Download states of resources the GLTF
        /// is dependent on. Dictionary of resource URIs and whether or not they have
        /// been downlaoded.</param>
        /// <param name="onLoaded">Action to invoke when loading is complete. Takes a reference
        /// to the loaded mesh entity.</param>
        /// <param name="timeout">Timeout period after which loading will be aborted.</param>
        private IEnumerator LoadGLTFInBackground(Guid guid, string gltfResourceURI,
            Dictionary<string, bool> resourceDownloadStates, Action<MeshEntity> onLoaded, float timeout = 10)
        {
            bool allLoaded = true;
            float elapsedTime = 0f;
            if (resourceDownloadStates != null)
            {
                do
                {
                    allLoaded = true;
                    foreach (bool resource in resourceDownloadStates.Values)
                    {
                        if (resource == false)
                        {
                            allLoaded = false;
                            yield return new WaitForSeconds(0.25f);
                            elapsedTime += 0.25f;
                            break;
                        }
                    }
                } while (allLoaded == false && elapsedTime < timeout);

                LoadGLTF(
                    System.IO.Path.Combine(runtime.fileHandler.fileDirectory, FileHandler.ToFileURI(gltfResourceURI)),
                    new Action<GameObject>((meshObject) =>
                    {
                        SetUpLoadedGLTFMeshAsMeshEntity(meshObject, guid, onLoaded);
                    }));

                yield return null;
            }
        }
    }
}