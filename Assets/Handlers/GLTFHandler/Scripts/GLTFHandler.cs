using System.Collections.Generic;
using UnityEngine;
using FiveSQD.WebVerse.Utilities;
using FiveSQD.WebVerse.Handlers.File;
using System;
using FiveSQD.WebVerse.Runtime;
using FiveSQD.WebVerse.WebInterface.HTTP;
using FiveSQD.WebVerse.WorldEngine.Entity;

namespace FiveSQD.WebVerse.Handlers.GLTF
{
    public class GLTFHandler : BaseHandler
    {
        public WebVerseRuntime runtime;

        public GameObject meshPrefabContainer;

        private static readonly Vector3 prefabLocation = new Vector3(9999, 9999, 9999);

        private Dictionary<string, GameObject> gltfMeshPrefabs = new Dictionary<string, GameObject>();

        public Guid LoadGLTFResourceAsMeshEntity(string resourceURI, Guid? id = null, Action<MeshEntity> onLoaded = null)
        {
            Guid guid = id.HasValue ? id.Value : Guid.NewGuid();
            Action onDownloaded = () =>
            {
                LoadGLTF(
                    System.IO.Path.Combine(runtime.fileHandler.fileDirectory, FileHandler.ToFileURI(resourceURI)),
                    new Action<GameObject>((meshObject) =>
                    {
                        MeshEntity meshEntity = SetUpLoadedGLTFMeshAsMeshEntity(meshObject, guid);
                        if (onLoaded != null)
                        {
                            onLoaded.Invoke(meshEntity);
                        }
                    }));
            };
            DownloadGLTF(resourceURI, onDownloaded);
            return guid;
        }

        public void DownloadGLTF(string uri, Action onDownloaded, bool reDownload = false)
        {
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
        }

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
                GLTFLoader.LoadModelAsync(path, callback);
            }
        }

        public void MeshLoadedFromGLTF(string path, GameObject result, AnimationClip[] clips, Action<GameObject> callback)
        {
            gltfMeshPrefabs.Add(path, result);
            result.transform.position = prefabLocation;
            SetUpMeshPrefab(result);
            InstantiateMeshFromPrefab(result, callback);
        }

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

        private void InstantiateMeshFromPrefab(GameObject prefab, Action<GameObject> callback)
        {
            GameObject loadedMesh = Instantiate(prefab);
            loadedMesh.SetActive(true);
            callback.Invoke(loadedMesh);
        }

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

        private MeshEntity SetUpLoadedGLTFMeshAsMeshEntity(GameObject loadedMesh, Guid guid)
        {
            MeshEntity meshEntity = loadedMesh.AddComponent<MeshEntity>();
            meshEntity.Initialize(guid);
            return meshEntity;
        }
    }
}