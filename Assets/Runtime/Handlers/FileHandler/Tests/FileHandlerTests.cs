// Copyright (c) 2019-2023 Five Squared Interactive. All rights reserved.

using FiveSQD.WebVerse.Handlers.File;
using FiveSQD.WebVerse.Runtime;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using FiveSQD.WebVerse.LocalStorage;
using FiveSQD.WebVerse.WorldEngine;
using System.IO;

/// <summary>
/// Unit tests for the File Handler.
/// </summary>
public class FileHandlerTests
{
    [Test]
    public void FileHandlerTests_General()
    {
        // To File URI.
        string rawURI = "https://test.com/some/path/to/a/resource.html#thisthing";
        string fileURI = FileHandler.ToFileURI(rawURI);
        Assert.AreEqual("https~//test.com/some/path/to/a/resource.html#thisthing", fileURI);

        // From File URI.
        string restoredURI = FileHandler.FromFileURI(fileURI);
        Assert.AreEqual(rawURI, restoredURI);

        // Set up WebVerse Runtime.
        GameObject runtimeGO = new GameObject("runtime");
        WebVerseRuntime runtime = runtimeGO.AddComponent<WebVerseRuntime>();
        runtime.highlightMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        runtime.skyMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/WebVerse-WorldEngine/Assets/WorldEngine/Environment/Materials/Skybox.mat");
        runtime.characterControllerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/WebVerse-WorldEngine/Assets/WorldEngine/Entity/Character/Prefabs/UserAvatar.prefab");
        runtime.inputEntityPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/WebVerse-WorldEngine/Assets/WorldEngine/Entity/UI/UIElement/Input/Prefabs/InputEntity.prefab");
        runtime.voxelPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/WebVerse-WorldEngine/Assets/WorldEngine/Entity/Voxel/Prefabs/Voxel.prefab");
        runtime.webVerseWebViewPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Runtime/WebView/Prefabs/WebViewPrefab.prefab");
        runtime.Initialize(LocalStorageManager.LocalStorageMode.Cache, 128, 128, 128, Path.Combine(Application.dataPath, "Files"));
        WorldEngine.LoadWorld("test");

        // Create File in File Directory (byte array).
        byte[] fileData = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
        runtime.fileHandler.CreateFileInFileDirectory("path/to/testfile", fileData);
        Assert.IsTrue(runtime.fileHandler.FileExistsInFileDirectory("path/to/testfile"));

        // Get File in File Directory.
        byte[] readData = runtime.fileHandler.GetFileInFileDirectory("path/to/testfile");
        Assert.AreEqual(fileData, readData);

        // Delete File in File Directory.
        runtime.fileHandler.DeleteFileInFileDirectory("path/to/testfile");
        Assert.IsFalse(runtime.fileHandler.FileExistsInFileDirectory("path/to/testfile"));

        // Create File in File Directory (Texture2D).
        Texture2D image = new Texture2D(16, 16);
        image.filterMode = FilterMode.Bilinear;
        Color[] colors = new Color[256];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.red;
        }
        image.SetPixels(colors);
        runtime.fileHandler.CreateFileInFileDirectory("path/to/testfile", image);
        Assert.IsTrue(runtime.fileHandler.FileExistsInFileDirectory("path/to/testfile"));

        // Get File in File Directory.
        readData = runtime.fileHandler.GetFileInFileDirectory("path/to/testfile");
        Assert.AreEqual(image.EncodeToPNG(), readData);

        // Delete File in File Directory.
        runtime.fileHandler.DeleteFileInFileDirectory("path/to/testfile");
        Assert.IsFalse(runtime.fileHandler.FileExistsInFileDirectory("path/to/testfile"));
    }
}