// TypeScript Declaration File for WebVerse Input Control Flags
// This file provides type definitions for input control flags in WebVerse-Runtime
// Reference: Assets/Runtime/Handlers/VEMLHandler/Scripts/VEMLHandler.cs - ProcessControlFlags method

/**
 * Enumeration for VR pointer modes.
 */
declare enum VRPointerMode {
    /** No pointer active */
    None = 0,
    /** Teleport pointer mode */
    Teleport = 1,
    /** UI interaction pointer mode */
    UI = 2
}

/**
 * Enumeration for VR turn locomotion modes.
 */
declare enum VRTurnLocomotionMode {
    /** No turn locomotion */
    None = 0,
    /** Smooth continuous turning */
    Smooth = 1,
    /** Snap turning in increments */
    Snap = 2
}

/**
 * Input control flags and methods for WebVerse-Runtime.
 * Provides access to VR and Desktop input control configuration.
 */
declare namespace Input {
    // ============================================================
    // VR Control Flags
    // ============================================================

    /**
     * Whether or not joystick motion is enabled for VR.
     * Controls movement using VR controller joysticks.
     */
    let joystickMotionEnabled: boolean;

    /**
     * Whether or not left hand grab move is enabled for VR.
     * Allows grabbing and pulling the world with the left controller.
     */
    let leftGrabMoveEnabled: boolean;

    /**
     * Whether or not right hand grab move is enabled for VR.
     * Allows grabbing and pulling the world with the right controller.
     */
    let rightGrabMoveEnabled: boolean;

    /**
     * Whether or not left hand interaction is enabled for VR.
     * Controls if the left hand can interact with objects.
     */
    let leftInteractionEnabled: boolean;

    /**
     * Whether or not right hand interaction is enabled for VR.
     * Controls if the right hand can interact with objects.
     */
    let rightInteractionEnabled: boolean;

    /**
     * The pointer mode for the left VR controller.
     * Can be None, Teleport, or UI.
     */
    let leftVRPointerMode: VRPointerMode;

    /**
     * The pointer mode for the right VR controller.
     * Can be None, Teleport, or UI.
     */
    let rightVRPointerMode: VRPointerMode;

    /**
     * Whether or not the left VR poker is enabled.
     * Controls direct touch/poke interaction with the left hand.
     */
    let leftVRPokerEnabled: boolean;

    /**
     * Whether or not the right VR poker is enabled.
     * Controls direct touch/poke interaction with the right hand.
     */
    let rightVRPokerEnabled: boolean;

    /**
     * The turn locomotion mode for VR.
     * Can be None, Smooth, or Snap.
     */
    let turnLocomotionMode: VRTurnLocomotionMode;

    /**
     * Whether or not two-handed grab move is enabled for VR.
     * Allows grabbing and scaling/rotating the world with both hands.
     */
    let twoHandedGrabMoveEnabled: boolean;

    // ============================================================
    // Desktop Control Flags
    // ============================================================

    /**
     * Whether or not gravity is enabled for desktop input.
     * When disabled, the player can fly freely without being pulled down.
     */
    let gravityEnabled: boolean;

    /**
     * Whether or not WASD motion is enabled for desktop input.
     * Controls movement using the W, A, S, D keys.
     */
    let wasdMotionEnabled: boolean;

    /**
     * Whether or not mouse look is enabled for desktop input.
     * Controls camera rotation using mouse movement.
     */
    let mouseLookEnabled: boolean;

    /**
     * Whether or not jump is enabled for desktop input.
     * Controls if the player can jump using the spacebar.
     */
    let jumpEnabled: boolean;

    /**
     * The movement speed for desktop input.
     * Higher values make the player move faster.
     */
    let movementSpeed: number;

    /**
     * The look speed (mouse sensitivity) for desktop input.
     * Higher values make the camera rotate faster with mouse movement.
     */
    let lookSpeed: number;

    // ============================================================
    // Desktop Control Methods
    // ============================================================

    /**
     * Set the avatar entity by tag for desktop input.
     * The avatar entity is the 3D model representing the player.
     * 
     * @param entityTag The tag of the entity to use as the avatar
     * @returns Whether or not the operation was successful
     * 
     * @example
     * ```typescript
     * // Set the player's avatar to an entity with tag "PlayerModel"
     * const success = Input.SetAvatarEntityByTag("PlayerModel");
     * if (success) {
     *     console.log("Avatar set successfully");
     * }
     * ```
     */
    function SetAvatarEntityByTag(entityTag: string): boolean;

    /**
     * Set the rig offset from a string for desktop input.
     * The rig offset adjusts the position of the player's viewpoint relative to the avatar.
     * 
     * @param rigOffsetString The rig offset string in format "x,y,z" (e.g., "0,1.6,0")
     * @returns Whether or not the operation was successful
     * 
     * @example
     * ```typescript
     * // Set rig offset to 1.6 units above the avatar (eye level)
     * const success = Input.SetRigOffset("0,1.6,0");
     * if (success) {
     *     console.log("Rig offset applied");
     * }
     * ```
     */
    function SetRigOffset(rigOffsetString: string): boolean;
}

// ============================================================
// Usage Examples
// ============================================================

/**
 * Example: Configure VR Controls
 * 
 * ```typescript
 * // Enable joystick locomotion
 * Input.joystickMotionEnabled = true;
 * 
 * // Configure pointer modes
 * Input.leftVRPointerMode = VRPointerMode.Teleport;
 * Input.rightVRPointerMode = VRPointerMode.UI;
 * 
 * // Enable grab movement on both hands
 * Input.leftGrabMoveEnabled = true;
 * Input.rightGrabMoveEnabled = true;
 * 
 * // Set smooth turning
 * Input.turnLocomotionMode = VRTurnLocomotionMode.Smooth;
 * ```
 */

/**
 * Example: Configure Desktop Controls
 * 
 * ```typescript
 * // Enable standard FPS controls
 * Input.wasdMotionEnabled = true;
 * Input.mouseLookEnabled = true;
 * Input.jumpEnabled = true;
 * Input.gravityEnabled = true;
 * 
 * // Adjust movement and look speeds
 * Input.movementSpeed = 5.0;
 * Input.lookSpeed = 2.0;
 * 
 * // Set up player avatar
 * Input.SetAvatarEntityByTag("PlayerCharacter");
 * Input.SetRigOffset("0,1.7,0"); // Eye level at 1.7 units
 * ```
 */

/**
 * Example: Toggle Between Flight and Walking Mode
 * 
 * ```typescript
 * function toggleFlightMode(enabled: boolean) {
 *     if (enabled) {
 *         // Enable flight mode
 *         Input.gravityEnabled = false;
 *         Input.movementSpeed = 10.0;
 *         console.log("Flight mode enabled");
 *     } else {
 *         // Enable walking mode
 *         Input.gravityEnabled = true;
 *         Input.jumpEnabled = true;
 *         Input.movementSpeed = 5.0;
 *         console.log("Walking mode enabled");
 *     }
 * }
 * 
 * // Toggle flight mode on/off
 * toggleFlightMode(true);  // Enable flight
 * toggleFlightMode(false); // Enable walking
 * ```
 */

/**
 * Example: Disable All Player Controls
 * 
 * ```typescript
 * function disablePlayerControls() {
 *     // Desktop controls
 *     Input.wasdMotionEnabled = false;
 *     Input.mouseLookEnabled = false;
 *     Input.jumpEnabled = false;
 *     
 *     // VR controls
 *     Input.joystickMotionEnabled = false;
 *     Input.leftGrabMoveEnabled = false;
 *     Input.rightGrabMoveEnabled = false;
 *     Input.leftInteractionEnabled = false;
 *     Input.rightInteractionEnabled = false;
 * }
 * 
 * function enablePlayerControls() {
 *     // Desktop controls
 *     Input.wasdMotionEnabled = true;
 *     Input.mouseLookEnabled = true;
 *     Input.jumpEnabled = true;
 *     
 *     // VR controls  
 *     Input.joystickMotionEnabled = true;
 *     Input.leftGrabMoveEnabled = true;
 *     Input.rightGrabMoveEnabled = true;
 *     Input.leftInteractionEnabled = true;
 *     Input.rightInteractionEnabled = true;
 * }
 * ```
 */
