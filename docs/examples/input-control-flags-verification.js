// Example verification script for Input Control Flags JavaScript APIs
// This script can be executed in WebVerse-Runtime to verify the control flags work correctly
// Place this in a VEML document's script section or run via the JavaScript console

// ==============================================================
// Verification Tests for Input Control Flags
// ==============================================================

Logging.Log("Starting Input Control Flags Verification...");

// Test 1: VR Control Flags (Read/Write)
try {
    Logging.Log("Test 1: VR Control Flags");
    
    // Test joystick motion
    var originalJoystickMotion = Input.joystickMotionEnabled;
    Input.joystickMotionEnabled = !originalJoystickMotion;
    var newJoystickMotion = Input.joystickMotionEnabled;
    Logging.Log("  joystickMotionEnabled: " + (originalJoystickMotion !== newJoystickMotion ? "PASS" : "WARN - no change detected"));
    Input.joystickMotionEnabled = originalJoystickMotion; // Restore
    
    // Test grab move flags
    var originalLeftGrabMove = Input.leftGrabMoveEnabled;
    Input.leftGrabMoveEnabled = !originalLeftGrabMove;
    Logging.Log("  leftGrabMoveEnabled: " + (Input.leftGrabMoveEnabled !== originalLeftGrabMove ? "PASS" : "WARN - no change detected"));
    Input.leftGrabMoveEnabled = originalLeftGrabMove; // Restore
    
    var originalRightGrabMove = Input.rightGrabMoveEnabled;
    Input.rightGrabMoveEnabled = !originalRightGrabMove;
    Logging.Log("  rightGrabMoveEnabled: " + (Input.rightGrabMoveEnabled !== originalRightGrabMove ? "PASS" : "WARN - no change detected"));
    Input.rightGrabMoveEnabled = originalRightGrabMove; // Restore
    
    Logging.Log("Test 1: COMPLETED");
} catch (e) {
    Logging.LogError("Test 1 FAILED: " + e);
}

// Test 2: Desktop Control Flags (Read/Write)
try {
    Logging.Log("Test 2: Desktop Control Flags");
    
    // Test gravity enabled
    var originalGravity = Input.gravityEnabled;
    Input.gravityEnabled = !originalGravity;
    var newGravity = Input.gravityEnabled;
    Logging.Log("  gravityEnabled: " + (originalGravity !== newGravity ? "PASS" : "WARN - no change detected"));
    Input.gravityEnabled = originalGravity; // Restore
    
    // Test WASD motion
    var originalWASD = Input.wasdMotionEnabled;
    Input.wasdMotionEnabled = !originalWASD;
    var newWASD = Input.wasdMotionEnabled;
    Logging.Log("  wasdMotionEnabled: " + (originalWASD !== newWASD ? "PASS" : "WARN - no change detected"));
    Input.wasdMotionEnabled = originalWASD; // Restore
    
    // Test mouse look
    var originalMouseLook = Input.mouseLookEnabled;
    Input.mouseLookEnabled = !originalMouseLook;
    var newMouseLook = Input.mouseLookEnabled;
    Logging.Log("  mouseLookEnabled: " + (originalMouseLook !== newMouseLook ? "PASS" : "WARN - no change detected"));
    Input.mouseLookEnabled = originalMouseLook; // Restore
    
    // Test jump enabled
    var originalJump = Input.jumpEnabled;
    Input.jumpEnabled = !originalJump;
    var newJump = Input.jumpEnabled;
    Logging.Log("  jumpEnabled: " + (originalJump !== newJump ? "PASS" : "WARN - no change detected"));
    Input.jumpEnabled = originalJump; // Restore
    
    Logging.Log("Test 2: COMPLETED");
} catch (e) {
    Logging.LogError("Test 2 FAILED: " + e);
}

// Test 3: Movement Speed and Look Speed
try {
    Logging.Log("Test 3: Movement Speed and Look Speed");
    
    // Test movement speed
    var originalMovementSpeed = Input.movementSpeed;
    Input.movementSpeed = 10.0;
    var newMovementSpeed = Input.movementSpeed;
    Logging.Log("  movementSpeed: " + (newMovementSpeed === 10.0 ? "PASS" : "WARN - value=" + newMovementSpeed));
    Input.movementSpeed = originalMovementSpeed; // Restore
    
    // Test look speed
    var originalLookSpeed = Input.lookSpeed;
    Input.lookSpeed = 3.5;
    var newLookSpeed = Input.lookSpeed;
    Logging.Log("  lookSpeed: " + (newLookSpeed === 3.5 ? "PASS" : "WARN - value=" + newLookSpeed));
    Input.lookSpeed = originalLookSpeed; // Restore
    
    Logging.Log("Test 3: COMPLETED");
} catch (e) {
    Logging.LogError("Test 3 FAILED: " + e);
}

// Test 4: Avatar Entity and Rig Offset Methods
try {
    Logging.Log("Test 4: Avatar Entity and Rig Offset Methods");
    
    // Test SetAvatarEntityByTag (with non-existent tag, should return false)
    var result1 = Input.SetAvatarEntityByTag("NonExistentTag");
    Logging.Log("  SetAvatarEntityByTag (non-existent): " + (result1 === false ? "PASS (correctly returned false)" : "WARN - returned " + result1));
    
    // Test SetRigOffset
    var result2 = Input.SetRigOffset("0,1.7,0");
    Logging.Log("  SetRigOffset: " + (result2 === true || result2 === false ? "PASS (method callable)" : "FAIL - unexpected result"));
    
    Logging.Log("Test 4: COMPLETED");
} catch (e) {
    Logging.LogError("Test 4 FAILED: " + e);
}

// Test 5: Practical Use Case - Toggle Flight Mode
try {
    Logging.Log("Test 5: Practical Use Case - Toggle Flight Mode");
    
    function toggleFlightMode(enabled) {
        if (enabled) {
            // Enable flight mode
            Input.gravityEnabled = false;
            Input.movementSpeed = 10.0;
            Logging.Log("  Flight mode enabled");
        } else {
            // Enable walking mode
            Input.gravityEnabled = true;
            Input.jumpEnabled = true;
            Input.movementSpeed = 5.0;
            Logging.Log("  Walking mode enabled");
        }
    }
    
    // Save original values
    var originalGrav = Input.gravityEnabled;
    var originalJmp = Input.jumpEnabled;
    var originalSpeed = Input.movementSpeed;
    
    // Test flight mode
    toggleFlightMode(true);
    var flightModeActive = (Input.gravityEnabled === false && Input.movementSpeed === 10.0);
    Logging.Log("  Flight mode toggle: " + (flightModeActive ? "PASS" : "WARN - state incorrect"));
    
    // Test walking mode
    toggleFlightMode(false);
    var walkingModeActive = (Input.gravityEnabled === true && Input.jumpEnabled === true);
    Logging.Log("  Walking mode toggle: " + (walkingModeActive ? "PASS" : "WARN - state incorrect"));
    
    // Restore original values
    Input.gravityEnabled = originalGrav;
    Input.jumpEnabled = originalJmp;
    Input.movementSpeed = originalSpeed;
    
    Logging.Log("Test 5: COMPLETED");
} catch (e) {
    Logging.LogError("Test 5 FAILED: " + e);
}

Logging.Log("Input Control Flags Verification Complete!");
Logging.Log("NOTE: WARN messages are expected if desktop rig or VR rig are not available in current context");
