// Copyright (c) 2019-2025 Five Squared Interactive. All rights reserved.

// Example: How to set up and configure Mobile Input in WebVerse-Runtime

using UnityEngine;
using FiveSQD.WebVerse.Input;
using FiveSQD.WebVerse.Input.Mobile;
using FiveSQD.WebVerse.Runtime;

/// <summary>
/// Example class demonstrating mobile input setup and configuration.
/// This would typically be integrated into the main runtime initialization.
/// </summary>
public class MobileInputSetupExample : MonoBehaviour
{
    [Header("Mobile Input Configuration")]
    public float touchSensitivity = 1.0f;
    public float dragThreshold = 10.0f;
    public float tapTimeLimit = 0.3f;
    public float tapDistanceLimit = 50.0f;
    public bool enablePinchZoom = true;

    private MobileInput mobileInput;
    private InputManager inputManager;

    void Start()
    {
        // Get the input manager from WebVerse Runtime
        inputManager = WebVerseRuntime.Instance.inputManager;
        
        // Set up mobile input if running on mobile platform
        if (Application.isMobilePlatform)
        {
            SetupMobileInput();
        }
    }

    void SetupMobileInput()
    {
        // Create mobile input component
        mobileInput = gameObject.AddComponent<MobileInput>();
        
        // Configure mobile input settings
        mobileInput.touchSensitivity = touchSensitivity;
        mobileInput.touchDragThreshold = dragThreshold;
        mobileInput.tapTimeThreshold = tapTimeLimit;
        mobileInput.tapDistanceThreshold = tapDistanceLimit;
        mobileInput.pinchZoomEnabled = enablePinchZoom;
        
        // Enable all touch features
        mobileInput.touchInputEnabled = true;
        mobileInput.touchMovementEnabled = true;
        mobileInput.touchLookEnabled = true;
        
        // Assign mobile input as the platform input
        inputManager.platformInput = mobileInput;
        
        Debug.Log("Mobile input configured and enabled");
        
        // Register input events for custom handling (optional)
        RegisterMobileInputEvents();
    }

    void RegisterMobileInputEvents()
    {
        // Example: Register custom JavaScript functions for mobile-specific events
        // These would be called when touch events occur
        
        // Single tap events
        inputManager.RegisterInputEvent("left", "handleMobileTap(?)");
        inputManager.RegisterInputEvent("endleft", "handleMobileTapEnd(?)");
        
        // Touch drag/look events  
        inputManager.RegisterInputEvent("look", "handleMobileLook(?)");
        inputManager.RegisterInputEvent("endlook", "handleMobileLookEnd(?)");
        
        // Zoom/pinch events (mapped to middle mouse)
        inputManager.RegisterInputEvent("middle", "handleMobileZoom(?)");
        inputManager.RegisterInputEvent("endmiddle", "handleMobileZoomEnd(?)");
        
        Debug.Log("Mobile input events registered");
    }

    void Update()
    {
        // Example: Monitor mobile input state
        if (mobileInput != null)
        {
            // Display current touch information (for debugging)
            if (mobileInput.touchCount > 0)
            {
                Vector2 primaryPos = mobileInput.primaryTouchPosition;
                Debug.Log($"Touch Count: {mobileInput.touchCount}, Primary Position: {primaryPos}");
                
                if (mobileInput.touchCount >= 2)
                {
                    Vector2 secondaryPos = mobileInput.secondaryTouchPosition;
                    Debug.Log($"Secondary Position: {secondaryPos}");
                }
            }
        }
    }

    void OnGUI()
    {
        // Simple UI to demonstrate mobile input status
        if (mobileInput != null)
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 200));
            GUILayout.Label("Mobile Input Status:");
            GUILayout.Label($"Touch Input Enabled: {mobileInput.touchInputEnabled}");
            GUILayout.Label($"Touch Count: {mobileInput.touchCount}");
            GUILayout.Label($"Primary Position: {mobileInput.primaryTouchPosition}");
            
            if (mobileInput.touchCount >= 2)
            {
                GUILayout.Label($"Secondary Position: {mobileInput.secondaryTouchPosition}");
            }
            
            GUILayout.Label($"Touch Sensitivity: {mobileInput.touchSensitivity}");
            GUILayout.Label($"Pinch Zoom: {mobileInput.pinchZoomEnabled}");
            GUILayout.EndArea();
        }
    }

    // Example method to demonstrate runtime configuration changes
    public void ConfigureMobileInput(bool enableTouch, float sensitivity, bool enablePinch)
    {
        if (mobileInput != null)
        {
            mobileInput.touchInputEnabled = enableTouch;
            mobileInput.touchSensitivity = sensitivity;
            mobileInput.pinchZoomEnabled = enablePinch;
            
            Debug.Log($"Mobile input reconfigured: Touch={enableTouch}, Sensitivity={sensitivity}, Pinch={enablePinch}");
        }
    }

    // Example method to demonstrate pointer raycasting with touch
    public void TestTouchRaycast()
    {
        if (mobileInput != null && mobileInput.touchCount > 0)
        {
            // Get raycast from primary touch position
            var raycastResult = mobileInput.GetPointerRaycast(Vector3.forward, 0);
            
            if (raycastResult != null)
            {
                Debug.Log($"Touch raycast hit: {raycastResult.Item1.collider.name} at {raycastResult.Item1.point}");
            }
            else
            {
                Debug.Log("Touch raycast missed");
            }
        }
    }
}