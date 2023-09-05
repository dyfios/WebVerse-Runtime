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

public class GLTFHandlerTests
{
    private float waitTime = 10;

    [UnityTest]
    public IEnumerator GLTFHandlerTests_General()
    {
        GameObject runtimeGO = new GameObject("runtime");
        WebVerseRuntime runtime = runtimeGO.AddComponent<WebVerseRuntime>();
        runtime.highlightMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        runtime.skyMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/WebVerse-WorldEngine/Assets/WorldEngine/Environment/Materials/Skybox.mat");
        runtime.characterControllerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/WebVerse-WorldEngine/Assets/WorldEngine/Entity/Character/Prefabs/UserAvatar.prefab");
        runtime.inputEntityPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/WebVerse-WorldEngine/Assets/WorldEngine/Entity/UI/UIElement/Input/Prefabs/InputEntity.prefab");
        runtime.voxelPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/WebVerse-WorldEngine/Assets/WorldEngine/Entity/Voxel/Prefabs/Voxel.prefab");
        runtime.Initialize(LocalStorageManager.LocalStorageMode.Cache, 128, 128, 128);
        WorldEngine.LoadWorld("test");

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

        bool firstDownloaded = false;
        Action onFirstDownloaded = new Action(() =>
        {
            firstDownloaded = true;
        });
        runtime.gltfHandler.DownloadGLTFResource("https://raw.githubusercontent.com/KhronosGroup/glTF-Sample-Models/master/2.0/Box/glTF/Box.gltf",
            onFirstDownloaded, true);

        bool secondDownloaded = false;
        Action onSecondDownloaded = new Action(() =>
        {
            secondDownloaded = true;
        });
        runtime.gltfHandler.DownloadGLTFResource("https://raw.githubusercontent.com/KhronosGroup/glTF-Sample-Models/master/2.0/Box/glTF/Box0.bin",
            onSecondDownloaded, true);

        yield return new WaitForSeconds(waitTime);

        Assert.IsTrue(firstDownloaded);

        Assert.IsTrue(secondDownloaded);

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