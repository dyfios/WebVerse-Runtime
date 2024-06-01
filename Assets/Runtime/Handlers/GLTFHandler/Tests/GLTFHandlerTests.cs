// Copyright (c) 2019-2023 Five Squared Interactive. All rights reserved.

using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System;
using FiveSQD.WebVerse.WorldEngine.Entity;
using FiveSQD.WebVerse.Runtime;
using FiveSQD.WebVerse.LocalStorage;
using UnityEditor;
using FiveSQD.WebVerse.WorldEngine;
using FiveSQD.WebVerse.Handlers.File;
using System.IO;

/// <summary>
/// Unit tests for the GLTF Handler.
/// </summary>
public class GLTFHandlerTests
{
    private float waitTime = 10;

    [UnityTest]
    public IEnumerator GLTFHandlerTests_General()
    {
        // Set up WebVerse Runtime.
        GameObject runtimeGO = new GameObject("runtime");
        WebVerseRuntime runtime = runtimeGO.AddComponent<WebVerseRuntime>();
        runtime.highlightMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        runtime.skyMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/WebVerse-WorldEngine/Assets/WorldEngine/Environment/Materials/Skybox.mat");
        runtime.characterControllerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/WebVerse-WorldEngine/Assets/WorldEngine/Entity/Character/Prefabs/UserAvatar.prefab");
        runtime.inputEntityPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/WebVerse-WorldEngine/Assets/WorldEngine/Entity/UI/UIElement/Input/Prefabs/InputEntity.prefab");
        runtime.voxelPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/WebVerse-WorldEngine/Assets/WorldEngine/Entity/Voxel/Prefabs/Voxel.prefab");
        runtime.Initialize(LocalStorageManager.LocalStorageMode.Cache, 128, 128, 128, Path.Combine(Application.dataPath, "Files"));
        WorldEngine.LoadWorld("test");

        // Load GLTF Resource as Mesh Entity.
        MeshEntity mEntity = null;
        Action<MeshEntity> onLoaded = new Action<MeshEntity>((meshEntity) =>
        {
            mEntity = meshEntity;
        });
        runtime.gltfHandler.LoadGLTFResourceAsMeshEntity(
            "https://raw.githubusercontent.com/KhronosGroup/glTF-Sample-Models/master/2.0/2CylinderEngine/glTF-Draco/2CylinderEngine.gltf",
            new string[] {
            "https://raw.githubusercontent.com/KhronosGroup/glTF-Sample-Models/master/2.0/2CylinderEngine/glTF-Draco/2CylinderEngine.gltf",
            "https://raw.githubusercontent.com/KhronosGroup/glTF-Sample-Models/master/2.0/2CylinderEngine/glTF-Draco/2CylinderEngine.bin" }, null, onLoaded, 10);
        yield return new WaitForSeconds(waitTime);
        Assert.IsNotNull(mEntity);

        // Download GLTF Resource.
        bool firstDownloaded = false;
        Action onFirstDownloaded = new Action(() =>
        {
            firstDownloaded = true;
        });
        runtime.gltfHandler.DownloadGLTFResource("https://raw.githubusercontent.com/KhronosGroup/glTF-Sample-Models/master/2.0/Box/glTF/Box.gltf",
            onFirstDownloaded);
        bool secondDownloaded = false;
        Action onSecondDownloaded = new Action(() =>
        {
            secondDownloaded = true;
        });
        runtime.gltfHandler.DownloadGLTFResource("https://raw.githubusercontent.com/KhronosGroup/glTF-Sample-Models/master/2.0/Box/glTF/Box0.bin",
            onSecondDownloaded);

        yield return new WaitForSeconds(waitTime);

        Assert.IsTrue(firstDownloaded);

        Assert.IsTrue(secondDownloaded);

        // Load GLTF.
        GameObject m = null;
        Action<GameObject> onGOLoaded = new Action<GameObject>((mesh) =>
        {
            m = mesh;
        });
        runtime.gltfHandler.LoadGLTF(System.IO.Path.Combine(runtime.fileHandler.fileDirectory,
            FileHandler.ToFileURI("https://raw.githubusercontent.com/KhronosGroup/glTF-Sample-Models/master/2.0/Box/glTF/Box.gltf")), onGOLoaded);

        yield return new WaitForSeconds(waitTime);
        Assert.IsNotNull(m);
    }
}