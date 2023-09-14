// Copyright (c) 2019-2023 Five Squared Interactive. All rights reserved.

using System;
using System.Collections;
using FiveSQD.WebVerse.LocalStorage;
using FiveSQD.WebVerse.Runtime;
using FiveSQD.WebVerse.WorldEngine;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

/// <summary>
/// Unit tests for the PNG Handler.
/// </summary>
public class PNGHandlerTests
{
    private float waitTime = 10;

    [UnityTest]
    public IEnumerator PNGHandlerTests_General()
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

        // Load Image that does not exist.
        string pngURI = System.IO.Path.Combine(runtime.fileHandler.fileDirectory, "test/test.png");
        Texture2D loadedPNG = null;
        Assert.Throws<System.IO.DirectoryNotFoundException>(() =>
        {
            loadedPNG = runtime.pngHandler.LoadImage(pngURI);
        });
        Assert.IsNull(loadedPNG);

        // Download PNG that does not exist.
        bool downloadComplete = false;
        System.Action onDownloaded = () =>
        {
            downloadComplete = true;
            Assert.Throws<System.IO.DirectoryNotFoundException>(() =>
            {
                Assert.IsNull(runtime.pngHandler.LoadImage(System.IO.Path.Combine(
                    runtime.fileHandler.fileDirectory, "https~/invalidurlforthistest.com/invalid.png")));
            });
        };
        runtime.pngHandler.DownloadPNG("https://invalidurlforthistest.com/invalid.png", onDownloaded, true);
        yield return new WaitForSeconds(waitTime);
        Assert.IsTrue(downloadComplete);

        // Download PNG that does exist.
        downloadComplete = false;
        onDownloaded = () =>
        {
            downloadComplete = true;
            Assert.IsNotNull(runtime.pngHandler.LoadImage(System.IO.Path.Combine(
                    runtime.fileHandler.fileDirectory,
                    "https~/www.google.com/images/branding/googlelogo/1x/googlelogo_light_color_272x92dp.png")));
        };
        runtime.pngHandler.DownloadPNG("https://www.google.com/images/branding/googlelogo/1x/googlelogo_light_color_272x92dp.png", onDownloaded, true);
        yield return new WaitForSeconds(waitTime);
        Assert.IsTrue(downloadComplete);

        downloadComplete = false;
        Action<Texture2D> onLoaded = new Action<Texture2D>((tex) =>
        {
            downloadComplete = true;
            Assert.IsNotNull(tex);
        });
        runtime.pngHandler.LoadImageResourceAsTexture2D(
            "https://file-examples.com/storage/fe3b4f721f64dfeffa49f02/2017/10/file_example_PNG_500kB.png", onLoaded);
        yield return new WaitForSeconds(waitTime);
        Assert.IsTrue(downloadComplete);
    }
}