// Copyright (c) 2019-2023 Five Squared Interactive. All rights reserved.

using System;
using System.Collections;
using System.IO;
using FiveSQD.WebVerse.LocalStorage;
using FiveSQD.WebVerse.Runtime;
using FiveSQD.StraightFour;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

/// <summary>
/// Unit tests for the Image Handler.
/// </summary>
public class ImageHandlerTests
{
    private float waitTime = 10;

    [UnityTest]
    public IEnumerator ImageHandlerTests_General()
    {
        GameObject runtimeGO = new GameObject("runtime");
        WebVerseRuntime runtime = runtimeGO.AddComponent<WebVerseRuntime>();
        runtime.highlightMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        runtime.skyMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/WebVerse-WorldEngine/Assets/WorldEngine/Environment/Materials/Skybox.mat");
        runtime.characterControllerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/WebVerse-WorldEngine/Assets/WorldEngine/Entity/Character/Prefabs/UserAvatar.prefab");
        runtime.inputEntityPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/WebVerse-WorldEngine/Assets/WorldEngine/Entity/UI/UIElement/Input/Prefabs/InputEntity.prefab");
        runtime.voxelPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/WebVerse-WorldEngine/Assets/WorldEngine/Entity/Voxel/Prefabs/Voxel.prefab");
        runtime.Initialize(LocalStorageManager.LocalStorageMode.Cache, 128, 128, 128, Path.Combine(Application.dataPath, "Files"));
        StraightFour.LoadWorld("test");

        // Load Image that does not exist.
        string imageURI = System.IO.Path.Combine(runtime.fileHandler.fileDirectory, "test/test.png");
        Texture2D loadedImage = null;
        Assert.Throws<System.IO.DirectoryNotFoundException>(() =>
        {
            loadedImage = runtime.imageHandler.LoadImage(imageURI);
        });
        Assert.IsNull(loadedImage);

        // Download image that does not exist.
        bool downloadComplete = false;
        System.Action onDownloaded = () =>
        {
            downloadComplete = true;
            Assert.Throws<System.IO.DirectoryNotFoundException>(() =>
            {
                Assert.IsNull(runtime.imageHandler.LoadImage(System.IO.Path.Combine(
                    runtime.fileHandler.fileDirectory, "https~/invalidurlforthistest.com/invalid.png")));
            });
        };
        runtime.imageHandler.DownloadImage("https://invalidurlforthistest.com/invalid.png", onDownloaded);
        yield return new WaitForSeconds(waitTime);
        Assert.IsTrue(downloadComplete);

        // Download image that does exist.
        downloadComplete = false;
        onDownloaded = () =>
        {
            downloadComplete = true;
            Assert.IsNotNull(runtime.imageHandler.LoadImage(System.IO.Path.Combine(
                    runtime.fileHandler.fileDirectory,
                    "https~/www.google.com/images/branding/googlelogo/1x/googlelogo_light_color_272x92dp.png")));
        };
        runtime.imageHandler.DownloadImage("https://www.google.com/images/branding/googlelogo/1x/googlelogo_light_color_272x92dp.png", onDownloaded);
        yield return new WaitForSeconds(waitTime);
        Assert.IsTrue(downloadComplete);

        downloadComplete = false;
        Action<Texture2D> onLoaded = new Action<Texture2D>((tex) =>
        {
            downloadComplete = true;
            Assert.IsNotNull(tex);
        });
        runtime.imageHandler.LoadImageResourceAsTexture2D(
            "https://file-examples.com/storage/fe3b4f721f64dfeffa49f02/2017/10/file_example_PNG_500kB.png", onLoaded);
        yield return new WaitForSeconds(waitTime);
        Assert.IsTrue(downloadComplete);
    }
}