// Copyright (c) 2019-2025 Five Squared Interactive. All rights reserved.

using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using FiveSQD.WebVerse.Input.Mobile;
using FiveSQD.WebVerse.Input;

/// <summary>
/// Unit tests for the Mobile Input.
/// </summary>
public class MobileInputTests
{
    [Test]
    public void MobileInputTests_Initialization()
    {
        // Test basic mobile input creation and configuration
        GameObject mobileInputGameObject = new GameObject("MobileInput");
        MobileInput mobileInput = mobileInputGameObject.AddComponent<MobileInput>();
        
        Assert.IsNotNull(mobileInput);
        Assert.IsTrue(mobileInput.touchInputEnabled);
        Assert.IsTrue(mobileInput.touchMovementEnabled);
        Assert.IsTrue(mobileInput.touchLookEnabled);
        Assert.IsTrue(mobileInput.pinchZoomEnabled);
        Assert.AreEqual(1.0f, mobileInput.touchSensitivity);
        Assert.AreEqual(10.0f, mobileInput.touchDragThreshold);
        Assert.AreEqual(0.3f, mobileInput.tapTimeThreshold);
        Assert.AreEqual(50.0f, mobileInput.tapDistanceThreshold);
        
        Object.DestroyImmediate(mobileInputGameObject);
    }

    [Test]
    public void MobileInputTests_BasePlatformInputInheritance()
    {
        // Test that MobileInput correctly inherits from BasePlatformInput
        GameObject mobileInputGameObject = new GameObject("MobileInput");
        MobileInput mobileInput = mobileInputGameObject.AddComponent<MobileInput>();
        
        Assert.IsInstanceOf<BasePlatformInput>(mobileInput);
        
        Object.DestroyImmediate(mobileInputGameObject);
    }

    [Test]
    public void MobileInputTests_TouchProperties()
    {
        // Test touch position and count properties
        GameObject mobileInputGameObject = new GameObject("MobileInput");
        MobileInput mobileInput = mobileInputGameObject.AddComponent<MobileInput>();
        
        // Initial state should have no touches
        Assert.AreEqual(0, mobileInput.touchCount);
        Assert.AreEqual(Vector2.zero, mobileInput.primaryTouchPosition);
        Assert.AreEqual(Vector2.zero, mobileInput.secondaryTouchPosition);
        
        Object.DestroyImmediate(mobileInputGameObject);
    }

    [Test]
    public void MobileInputTests_GetPointerRaycastInvalidIndex()
    {
        // Test that GetPointerRaycast returns null for invalid indices
        GameObject mobileInputGameObject = new GameObject("MobileInput");
        MobileInput mobileInput = mobileInputGameObject.AddComponent<MobileInput>();
        
        // This should return null for index > 1 when no active world is available
        var result = mobileInput.GetPointerRaycast(Vector3.forward, 2);
        Assert.IsNull(result);
        
        Object.DestroyImmediate(mobileInputGameObject);
    }

    [Test]
    public void MobileInputTests_ConfigurationProperties()
    {
        // Test setting configuration properties
        GameObject mobileInputGameObject = new GameObject("MobileInput");
        MobileInput mobileInput = mobileInputGameObject.AddComponent<MobileInput>();
        
        mobileInput.touchInputEnabled = false;
        mobileInput.touchMovementEnabled = false;
        mobileInput.touchLookEnabled = false;
        mobileInput.pinchZoomEnabled = false;
        mobileInput.touchSensitivity = 2.0f;
        mobileInput.touchDragThreshold = 20.0f;
        mobileInput.tapTimeThreshold = 0.5f;
        mobileInput.tapDistanceThreshold = 100.0f;
        
        Assert.IsFalse(mobileInput.touchInputEnabled);
        Assert.IsFalse(mobileInput.touchMovementEnabled);
        Assert.IsFalse(mobileInput.touchLookEnabled);
        Assert.IsFalse(mobileInput.pinchZoomEnabled);
        Assert.AreEqual(2.0f, mobileInput.touchSensitivity);
        Assert.AreEqual(20.0f, mobileInput.touchDragThreshold);
        Assert.AreEqual(0.5f, mobileInput.tapTimeThreshold);
        Assert.AreEqual(100.0f, mobileInput.tapDistanceThreshold);
        
        Object.DestroyImmediate(mobileInputGameObject);
    }
}