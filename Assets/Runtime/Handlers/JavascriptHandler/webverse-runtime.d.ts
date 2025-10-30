// TypeScript definitions for WebVerse Runtime JavaScript APIs
// Project: WebVerse Runtime
// Definitions by: WebVerse Copilot

/**
 * WebVerse Runtime JavaScript API Type Definitions
 * 
 * This file provides TypeScript definitions for all APIs exposed by the
 * WebVerse Runtime's JavascriptHandler. These APIs are available globally
 * when scripts are executed within the WebVerse environment.
 * 
 * For more information, see:
 * https://five-squared-interactive.github.io/World-APIs/
 */

// ============================================================================
// World Types
// ============================================================================

/**
 * Class for an RGBA color.
 */
declare class Color {
    /** Red component, from 0-255. */
    r: number;
    /** Green component, from 0-255. */
    g: number;
    /** Blue component, from 0-255. */
    b: number;
    /** Alpha component, from 0-255. */
    a: number;

    /** Returns the color black (RGBA: 0, 0, 0, 1). */
    static readonly black: Color;
    /** Returns the color blue (RGBA: 0, 0, 1, 1). */
    static readonly blue: Color;
    /** Returns the color clear (RGBA: 0, 0, 0, 0). */
    static readonly clear: Color;
    /** Returns the color cyan (RGBA: 0, 1, 1, 1). */
    static readonly cyan: Color;
    /** Returns the color gray (RGBA: 0.5, 0.5, 0.5, 1). */
    static readonly gray: Color;
    /** Returns the color green (RGBA: 0, 1, 0, 1). */
    static readonly green: Color;
    /** Returns the color grey (RGBA: 0.5, 0.5, 0.5, 1). */
    static readonly grey: Color;
    /** Returns the color magenta (RGBA: 1, 0, 1, 1). */
    static readonly magenta: Color;
    /** Returns the color red (RGBA: 1, 0, 0, 1). */
    static readonly red: Color;
    /** Returns the color white (RGBA: 1, 1, 1, 1). */
    static readonly white: Color;
    /** Returns the color yellow (RGBA: 1, 0.92, 0.016, 1). */
    static readonly yellow: Color;

    /**
     * Constructor for a Color.
     * @param r Red component.
     * @param g Green component.
     * @param b Blue component.
     * @param a Alpha component.
     */
    constructor(r: number, g: number, b: number, a: number);
}

/**
 * Struct for raycast hit information.
 */
declare interface RaycastHitInfo {
    /** Entity that was hit. */
    entity: BaseEntity | null;
    /** Origin from which the ray was cast. */
    origin: Vector3;
    /** Point (in world coordinates) at which entity was hit. */
    hitPoint: Vector3;
    /** Normal of the hit point. */
    hitPointNormal: Vector3;
}

/**
 * Class for a 2-dimensional vector.
 */
declare class Vector2 {
    /** X component of the vector. */
    x: number;
    /** Y component of the vector. */
    y: number;
    /** Magnitude of the vector. */
    readonly magnitude: number;
    /** Squared magnitude of the vector. */
    readonly squaredMagnitude: number;

    /** Constructor for a Vector2. */
    constructor();
    /**
     * Constructor for a Vector2.
     * @param x X component.
     * @param y Y component.
     */
    constructor(x: number, y: number);
}

/**
 * Class for a 2-dimensional double vector.
 */
declare class Vector2D {
    /** X component of the vector. */
    x: number;
    /** Y component of the vector. */
    y: number;
    /** Magnitude of the vector. */
    readonly magnitude: number;
    /** Squared magnitude of the vector. */
    readonly squaredMagnitude: number;

    /** Constructor for a Vector2D. */
    constructor();
    /**
     * Constructor for a Vector2D.
     * @param x X component.
     * @param y Y component.
     */
    constructor(x: number, y: number);
}

/**
 * Class for a 2-dimensional integer vector.
 */
declare class Vector2Int {
    /** X component of the vector. */
    x: number;
    /** Y component of the vector. */
    y: number;

    /** Constructor for a Vector2Int. */
    constructor();
    /**
     * Constructor for a Vector2Int.
     * @param x X component.
     * @param y Y component.
     */
    constructor(x: number, y: number);
}

/**
 * Class for a 3-dimensional vector.
 */
declare class Vector3 {
    /** X component of the vector. */
    x: number;
    /** Y component of the vector. */
    y: number;
    /** Z component of the vector. */
    z: number;
    /** Magnitude of the vector. */
    readonly magnitude: number;
    /** Squared magnitude of the vector. */
    readonly squaredMagnitude: number;

    /** Constructor for a Vector3. */
    constructor();
    /**
     * Constructor for a Vector3.
     * @param x X component.
     * @param y Y component.
     * @param z Z component.
     */
    constructor(x: number, y: number, z: number);
}

/**
 * Class for a 3-dimensional double vector.
 */
declare class Vector3D {
    /** X component of the vector. */
    x: number;
    /** Y component of the vector. */
    y: number;
    /** Z component of the vector. */
    z: number;
    /** Magnitude of the vector. */
    readonly magnitude: number;
    /** Squared magnitude of the vector. */
    readonly squaredMagnitude: number;

    /** Constructor for a Vector3D. */
    constructor();
    /**
     * Constructor for a Vector3D.
     * @param x X component.
     * @param y Y component.
     * @param z Z component.
     */
    constructor(x: number, y: number, z: number);
}

/**
 * Class for a 3-dimensional integer vector.
 */
declare class Vector3Int {
    /** X component of the vector. */
    x: number;
    /** Y component of the vector. */
    y: number;
    /** Z component of the vector. */
    z: number;

    /** Constructor for a Vector3Int. */
    constructor();
    /**
     * Constructor for a Vector3Int.
     * @param x X component.
     * @param y Y component.
     * @param z Z component.
     */
    constructor(x: number, y: number, z: number);
}

/**
 * Class for a 4-dimensional vector.
 */
declare class Vector4 {
    /** X component of the vector. */
    x: number;
    /** Y component of the vector. */
    y: number;
    /** Z component of the vector. */
    z: number;
    /** W component of the vector. */
    w: number;
    /** Magnitude of the vector. */
    readonly magnitude: number;
    /** Squared magnitude of the vector. */
    readonly squaredMagnitude: number;

    /** Constructor for a Vector4. */
    constructor();
    /**
     * Constructor for a Vector4.
     * @param x X component.
     * @param y Y component.
     * @param z Z component.
     * @param w W component.
     */
    constructor(x: number, y: number, z: number, w: number);
}

/**
 * Class for a 4-dimensional double vector.
 */
declare class Vector4D {
    /** X component of the vector. */
    x: number;
    /** Y component of the vector. */
    y: number;
    /** Z component of the vector. */
    z: number;
    /** W component of the vector. */
    w: number;
    /** Magnitude of the vector. */
    readonly magnitude: number;
    /** Squared magnitude of the vector. */
    readonly squaredMagnitude: number;

    /** Constructor for a Vector4D. */
    constructor();
    /**
     * Constructor for a Vector4D.
     * @param x X component.
     * @param y Y component.
     * @param z Z component.
     * @param w W component.
     */
    constructor(x: number, y: number, z: number, w: number);
}

/**
 * Class for a 4-dimensional integer vector.
 */
declare class Vector4Int {
    /** X component of the vector. */
    x: number;
    /** Y component of the vector. */
    y: number;
    /** Z component of the vector. */
    z: number;
    /** W component of the vector. */
    w: number;

    /** Constructor for a Vector4Int. */
    constructor();
    /**
     * Constructor for a Vector4Int.
     * @param x X component.
     * @param y Y component.
     * @param z Z component.
     * @param w W component.
     */
    constructor(x: number, y: number, z: number, w: number);
}

/**
 * Class for a quaternion.
 */
declare class Quaternion {
    /** X component of the quaternion. */
    x: number;
    /** Y component of the quaternion. */
    y: number;
    /** Z component of the quaternion. */
    z: number;
    /** W component of the quaternion. */
    w: number;

    /** An identity (0, 0, 0, 1) quaternion. */
    static readonly identity: Quaternion;

    /** Constructor for a Quaternion. */
    constructor();
    /**
     * Constructor for a Quaternion.
     * @param x X component.
     * @param y Y component.
     * @param z Z component.
     * @param w W component.
     */
    constructor(x: number, y: number, z: number, w: number);
    /**
     * Constructor for a Quaternion using an angle axis input.
     * @param angle Angle.
     * @param axis Axis.
     */
    constructor(angle: number, axis: Vector3);

    /**
     * Get the angle between two Quaternions.
     * @param quaternion1 First Quaternion.
     * @param quaternion2 Second Quaternion.
     * @returns The angle between the two quaternions.
     */
    static GetAngle(quaternion1: Quaternion, quaternion2: Quaternion): number;

    /**
     * Get a Quaternion from Euler angles.
     * @param x Rotation around x axis.
     * @param y Rotation around y axis.
     * @param z Rotation around z axis.
     * @returns Quaternion from provided Euler angles.
     */
    static FromEulerAngles(x: number, y: number, z: number): Quaternion;

    /**
     * Get the inverse of a Quaternion.
     * @param quaternion The quaternion.
     * @returns The inverse of the quaternion.
     */
    static GetInverse(quaternion: Quaternion): Quaternion;

    /**
     * Linearly interpolate between two Quaternions percentage-wise.
     * @param quaternion1 First Quaternion.
     * @param quaternion2 Second Quaternion.
     * @param percent Percent between first and second Quaternion.
     * @returns A linearly interpolated Quaternion between the two Quaternions.
     */
    static LinearlyInterpolatePercent(quaternion1: Quaternion, quaternion2: Quaternion, percent: number): Quaternion;

    /**
     * Linearly interpolate between two Quaternions distance-wise.
     * @param quaternion1 First Quaternion.
     * @param quaternion2 Second Quaternion.
     * @param maxDistance Maximum distance between first and second Quaternion.
     * @returns A linearly interpolated Quaternion between the two Quaternions.
     */
    static LinearlyInterpolate(quaternion1: Quaternion, quaternion2: Quaternion, maxDistance: number): Quaternion;

    /**
     * Linearly interpolate unclamped between two Quaternions percentage-wise.
     * @param quaternion1 First Quaternion.
     * @param quaternion2 Second Quaternion.
     * @param percent Percent between first and second Quaternion.
     * @returns A linearly interpolated unclamped Quaternion between the two Quaternions.
     */
    static LinearlyInterpolatePercentUnclamped(quaternion1: Quaternion, quaternion2: Quaternion, percent: number): Quaternion;

    /**
     * Create a quaternion with a given forward and up direction.
     * @param forward Forward direction.
     * @param up Up direction.
     * @returns A quaternion with a given forward and up direction.
     */
    static CreateLookRotation(forward: Vector3, up?: Vector3): Quaternion;

    /**
     * Get Euler angle representation of the quaternion.
     * @returns Euler angle representation of the quaternion.
     */
    GetEulerAngles(): Vector3;

    /**
     * Get the dot product between two Quaternions.
     * @param first First Quaternion.
     * @param second Second Quaternion.
     * @returns The dot product between two Quaternions.
     */
    static GetDotProduct(first: Quaternion, second: Quaternion): number;

    /**
     * Get the rotation from one Vector3 to another Vector3.
     * @param from First Vector3.
     * @param to Second Vector3.
     * @returns The rotation from one Vector3 to another Vector3.
     */
    static GetRotationFromToVector3s(from: Vector3, to: Vector3): Quaternion;

    /**
     * Get a normalized Quaternion (with a magnitude of 1).
     * @param quaternion Input Quaternion.
     * @returns A normalized Quaternion.
     */
    static GetNormalized(quaternion: Quaternion): Quaternion;

    /**
     * Interpolate between two Quaternions.
     * @param quaternion1 First Quaternion.
     * @param quaternion2 Second Quaternion.
     * @param maxDegrees Maximum degrees between first and second Quaternion.
     * @returns An interpolated Quaternion between the two Quaternions.
     */
    static Interpolate(quaternion1: Quaternion, quaternion2: Quaternion, maxDegrees: number): Quaternion;

    /**
     * Spherically interpolate between two Quaternions.
     * @param quaternion1 First Quaternion.
     * @param quaternion2 Second Quaternion.
     * @param percent Percentage between the two quaternions to interpolate.
     * @returns A spherically interpolated Quaternion between the two Quaternions.
     */
    static SphericallyInterpolate(quaternion1: Quaternion, quaternion2: Quaternion, percent: number): Quaternion;

    /**
     * Spherically interpolate unclamped between two Quaternions.
     * @param quaternion1 First Quaternion.
     * @param quaternion2 Second Quaternion.
     * @param percent Percentage between the two quaternions to interpolate.
     * @returns A spherically interpolated unclamped Quaternion between the two Quaternions.
     */
    static SphericallyInterpolateUnclamped(quaternion1: Quaternion, quaternion2: Quaternion, percent: number): Quaternion;

    /**
     * Get angle axis representation of the quaternion.
     * @returns Tuple containing angle and axis.
     */
    GetAngleAxis(): [number, Vector3];

    /**
     * Combine two Quaternions.
     * @param quaternion1 First quaternion.
     * @param quaternion2 Second quaternion.
     * @returns The combination of the two Quaternions.
     */
    static Combine(quaternion1: Quaternion, quaternion2: Quaternion): Quaternion;
}

/**
 * Class for a double-precision quaternion.
 */
declare class QuaternionD {
    /** X component of the quaternion. */
    x: number;
    /** Y component of the quaternion. */
    y: number;
    /** Z component of the quaternion. */
    z: number;
    /** W component of the quaternion. */
    w: number;

    /** An identity (0, 0, 0, 1) quaternion. */
    static readonly identity: QuaternionD;

    /** Constructor for a QuaternionD. */
    constructor();
    /**
     * Constructor for a QuaternionD.
     * @param x X component.
     * @param y Y component.
     * @param z Z component.
     * @param w W component.
     */
    constructor(x: number, y: number, z: number, w: number);
}

/**
 * Class for a UUID.
 */
declare class UUID {
    /**
     * Constructor for a UUID.
     * @param input Input string to use (optional).
     */
    constructor(input?: string);

    /**
     * Get a new UUID.
     * @returns A new UUID.
     */
    static NewUUID(): UUID;

    /**
     * Parse a UUID from a string.
     * @param input Input string to use.
     * @returns A UUID containing the provided value, or null.
     */
    static Parse(input: string): UUID;

    /**
     * Convert UUID to string.
     * @returns String representation of the UUID, or null.
     */
    ToString(): string | null;
}

// ============================================================================
// Entity Types
// ============================================================================

/**
 * Interaction state enum.
 */
declare enum InteractionState {
    Idle = 0,
    Highlighted = 1,
    Pressed = 2
}

/**
 * Struct for entity motion.
 */
declare interface EntityMotion {
    /** Angular velocity. */
    angularVelocity: Vector3;
    /** Velocity. */
    velocity: Vector3;
    /** Static friction. */
    staticFriction: number;
    /** Dynamic friction. */
    dynamicFriction: number;
}

/**
 * Class for entity physical properties.
 */
declare class EntityPhysicalProperties {
    /** Angular drag. */
    angularDrag: number;
    /** Mass. */
    mass: number;
    /** Drag. */
    drag: number;
    /** Whether or not gravity is enabled. */
    gravitational: boolean;
}

/**
 * Struct for light properties.
 */
declare interface LightProperties {
    /** Light type. */
    type: LightType;
    /** Light color. */
    color: Color;
    /** Light intensity. */
    intensity: number;
    /** Light range. */
    range: number;
}

/**
 * Light type enum.
 */
declare enum LightType {
    Directional = 0,
    Spot = 1,
    Point = 2
}

/**
 * Text alignment enum.
 */
declare enum TextAlignment {
    TopLeft = 0,
    TopCenter = 1,
    TopRight = 2,
    MiddleLeft = 3,
    MiddleCenter = 4,
    MiddleRight = 5,
    BottomLeft = 6,
    BottomCenter = 7,
    BottomRight = 8
}

/**
 * Text wrapping enum.
 */
declare enum TextWrapping {
    NoWrap = 0,
    Wrap = 1,
    Overflow = 2
}

/**
 * UI element alignment enum.
 */
declare enum UIElementAlignment {
    TopLeft = 0,
    TopCenter = 1,
    TopRight = 2,
    MiddleLeft = 3,
    MiddleCenter = 4,
    MiddleRight = 5,
    BottomLeft = 6,
    BottomCenter = 7,
    BottomRight = 8,
    Stretch = 9
}

/**
 * Terrain entity brush type enum.
 */
declare enum TerrainEntityBrushType {
    RaiseLower = 0,
    Flatten = 1,
    Smooth = 2,
    Paint = 3
}

/**
 * Struct for terrain entity layer.
 */
declare interface TerrainEntityLayer {
    /** Diffuse texture path. */
    diffuse: string;
    /** Normal texture path. */
    normal: string;
    /** Mask texture path. */
    mask: string;
    /** Specular value. */
    specular: Color;
    /** Metallic value. */
    metallic: number;
    /** Smoothness value. */
    smoothness: number;
    /** Tile size. */
    tileSize: Vector2;
    /** Tile offset. */
    tileOffset: Vector2;
}

/**
 * Class for terrain entity layer mask.
 */
declare class TerrainEntityLayerMask {
    /** Constructor for a terrain entity layer mask. */
    constructor();
}

/**
 * Class for terrain entity layer mask collection.
 */
declare class TerrainEntityLayerMaskCollection {
    /** Constructor for a terrain entity layer mask collection. */
    constructor();
}

/**
 * Class for terrain entity modification.
 */
declare class TerrainEntityModification {
    /** Operation type. */
    operation: TerrainEntityOperation;
    /** Brush type. */
    brushType: TerrainEntityBrushType;
    /** Position. */
    position: Vector3;
    /** Brush size. */
    brushSize: number;
    /** Brush strength. */
    brushStrength: number;
    /** Layer. */
    layer: number;

    /** Constructor for a terrain entity modification. */
    constructor();
}

/**
 * Terrain entity operation enum.
 */
declare enum TerrainEntityOperation {
    Modify = 0,
    Paint = 1
}

/**
 * Class for voxel block information.
 */
declare class VoxelBlockInfo {
    /** Block ID. */
    id: number;
    /** Block subtype. */
    subType: VoxelBlockSubType;

    /** Constructor for voxel block info. */
    constructor();
}

/**
 * Struct for voxel block subtype.
 */
declare interface VoxelBlockSubType {
    /** Subtype ID. */
    id: number;
}

/**
 * Automobile type enum.
 */
declare enum AutomobileType {
    Car = 0,
    Truck = 1
}

/**
 * Class for automobile entity wheel.
 */
declare class AutomobileEntityWheel {
    /** Wheel radius. */
    radius: number;
    /** Suspension distance. */
    suspensionDistance: number;
    /** Suspension spring. */
    suspensionSpring: number;
    /** Suspension damper. */
    suspensionDamper: number;
    /** Suspension target position. */
    suspensionTargetPosition: number;
    /** Forward friction. */
    forwardFriction: number;
    /** Sideways friction. */
    sidewaysFriction: number;

    /** Constructor for automobile entity wheel. */
    constructor();
}

/**
 * Class for a generic entity.
 */
declare class Entity {
    /**
     * Get the entity corresponding to an ID.
     * @param id ID of the entity to get.
     * @returns The entity corresponding to the ID, or null.
     */
    static Get(id: string): BaseEntity | null;

    /**
     * Get the entity corresponding to a tag.
     * @param tag Tag of the entity to get.
     * @returns The entity corresponding to the tag, or null.
     */
    static GetByTag(tag: string): BaseEntity | null;
}

/**
 * Class for a base entity.
 */
declare class BaseEntity {
    /** ID of the entity. */
    readonly id: UUID;
    /** Tag of the entity. */
    tag: string;

    /** Constructor for the entity. */
    constructor();

    /**
     * Get the entity corresponding to an ID.
     * @param id ID of the entity to get.
     * @returns The entity corresponding to the ID, or null.
     */
    static Get(id: string): BaseEntity | null;

    /**
     * Set the parent of the entity.
     * @param parent Entity to make parent of this one, or null.
     * @returns Whether or not the operation was successful.
     */
    SetParent(parent: BaseEntity | null): boolean;

    /**
     * Get the parent of the entity.
     * @returns The parent of the entity, or null.
     */
    GetParent(): BaseEntity | null;

    /**
     * Get the position of the entity.
     * @param local Whether or not to get the local position.
     * @returns The position of the entity.
     */
    GetPosition(local?: boolean): Vector3;

    /**
     * Set the position of the entity.
     * @param position Position to set.
     * @param local Whether or not to set the local position.
     * @returns Whether or not the operation was successful.
     */
    SetPosition(position: Vector3, local?: boolean): boolean;

    /**
     * Get the rotation of the entity.
     * @param local Whether or not to get the local rotation.
     * @returns The rotation of the entity.
     */
    GetRotation(local?: boolean): Quaternion;

    /**
     * Set the rotation of the entity.
     * @param rotation Rotation to set.
     * @param local Whether or not to set the local rotation.
     * @returns Whether or not the operation was successful.
     */
    SetRotation(rotation: Quaternion, local?: boolean): boolean;

    /**
     * Get the scale of the entity.
     * @returns The scale of the entity.
     */
    GetScale(): Vector3;

    /**
     * Set the scale of the entity.
     * @param scale Scale to set.
     * @returns Whether or not the operation was successful.
     */
    SetScale(scale: Vector3): boolean;

    /**
     * Get the size of the entity.
     * @returns The size of the entity.
     */
    GetSize(): Vector3;

    /**
     * Set the size of the entity.
     * @param size Size to set.
     * @returns Whether or not the operation was successful.
     */
    SetSize(size: Vector3): boolean;

    /**
     * Get the visibility of the entity.
     * @returns Whether or not the entity is visible.
     */
    GetVisibility(): boolean;

    /**
     * Set the visibility of the entity.
     * @param visible Whether or not the entity should be visible.
     * @returns Whether or not the operation was successful.
     */
    SetVisibility(visible: boolean): boolean;

    /**
     * Get the highlight state of the entity.
     * @returns Whether or not the entity is highlighted.
     */
    GetHighlight(): boolean;

    /**
     * Set the highlight state of the entity.
     * @param highlight Whether or not the entity should be highlighted.
     * @returns Whether or not the operation was successful.
     */
    SetHighlight(highlight: boolean): boolean;

    /**
     * Get the motion state of the entity.
     * @returns The motion state of the entity.
     */
    GetMotion(): EntityMotion;

    /**
     * Set the motion state of the entity.
     * @param motion Motion state to set.
     * @returns Whether or not the operation was successful.
     */
    SetMotion(motion: EntityMotion): boolean;

    /**
     * Get the physical properties of the entity.
     * @returns The physical properties of the entity.
     */
    GetPhysicalProperties(): EntityPhysicalProperties;

    /**
     * Set the physical properties of the entity.
     * @param properties Physical properties to set.
     * @returns Whether or not the operation was successful.
     */
    SetPhysicalProperties(properties: EntityPhysicalProperties): boolean;

    /**
     * Get the interaction state of the entity.
     * @returns The interaction state of the entity.
     */
    GetInteractionState(): InteractionState;

    /**
     * Set the interaction state of the entity.
     * @param state Interaction state to set.
     * @returns Whether or not the operation was successful.
     */
    SetInteractionState(state: InteractionState): boolean;

    /**
     * Delete the entity.
     * @returns Whether or not the operation was successful.
     */
    Delete(): boolean;

    /**
     * Place camera on the entity.
     */
    PlaceCameraOn(): void;

    /**
     * Perform a raycast from the entity.
     * @param direction Direction to cast the ray.
     * @param distance Maximum distance to cast.
     * @returns Raycast hit information.
     */
    Raycast(direction: Vector3, distance: number): RaycastHitInfo;
}

/**
 * Class for an airplane entity.
 */
declare class AirplaneEntity extends BaseEntity {
    /** Constructor for airplane entity. */
    constructor();
}

/**
 * Class for an audio entity.
 */
declare class AudioEntity extends BaseEntity {
    /** Constructor for audio entity. */
    constructor();

    /**
     * Set the audio clip for the entity.
     * @param url URL of the audio clip.
     * @returns Whether or not the operation was successful.
     */
    SetAudioClip(url: string): boolean;

    /**
     * Play the audio.
     * @returns Whether or not the operation was successful.
     */
    Play(): boolean;

    /**
     * Pause the audio.
     * @returns Whether or not the operation was successful.
     */
    Pause(): boolean;

    /**
     * Stop the audio.
     * @returns Whether or not the operation was successful.
     */
    Stop(): boolean;

    /**
     * Set whether the audio should loop.
     * @param loop Whether to loop.
     * @returns Whether or not the operation was successful.
     */
    SetLoop(loop: boolean): boolean;

    /**
     * Set the volume of the audio.
     * @param volume Volume (0-1).
     * @returns Whether or not the operation was successful.
     */
    SetVolume(volume: number): boolean;
}

/**
 * Class for an automobile entity.
 */
declare class AutomobileEntity extends BaseEntity {
    /** Constructor for automobile entity. */
    constructor();
}

/**
 * Class for a button entity.
 */
declare class ButtonEntity extends BaseEntity {
    /** Constructor for button entity. */
    constructor();

    /**
     * Set the text of the button.
     * @param text Text to set.
     * @returns Whether or not the operation was successful.
     */
    SetText(text: string): boolean;

    /**
     * Get the text of the button.
     * @returns The text of the button.
     */
    GetText(): string;

    /**
     * Set the on click callback.
     * @param callback Callback function name.
     * @returns Whether or not the operation was successful.
     */
    SetOnClick(callback: string): boolean;
}

/**
 * Class for a canvas entity.
 */
declare class CanvasEntity extends BaseEntity {
    /** Constructor for canvas entity. */
    constructor();
}

/**
 * Class for a character entity.
 */
declare class CharacterEntity extends BaseEntity {
    /** Constructor for character entity. */
    constructor();

    /**
     * Set the movement speed of the character.
     * @param speed Movement speed.
     * @returns Whether or not the operation was successful.
     */
    SetMovementSpeed(speed: number): boolean;

    /**
     * Get the movement speed of the character.
     * @returns The movement speed.
     */
    GetMovementSpeed(): number;

    /**
     * Set the jump speed of the character.
     * @param speed Jump speed.
     * @returns Whether or not the operation was successful.
     */
    SetJumpSpeed(speed: number): boolean;

    /**
     * Get the jump speed of the character.
     * @returns The jump speed.
     */
    GetJumpSpeed(): number;
}

/**
 * Class for a container entity.
 */
declare class ContainerEntity extends BaseEntity {
    /** Constructor for container entity. */
    constructor();
}

/**
 * Class for an HTML entity.
 */
declare class HTMLEntity extends BaseEntity {
    /** Constructor for HTML entity. */
    constructor();

    /**
     * Set the HTML content.
     * @param html HTML content.
     * @returns Whether or not the operation was successful.
     */
    SetHTML(html: string): boolean;

    /**
     * Get the HTML content.
     * @returns The HTML content.
     */
    GetHTML(): string;
}

/**
 * Class for an image entity.
 */
declare class ImageEntity extends BaseEntity {
    /** Constructor for image entity. */
    constructor();

    /**
     * Set the image source.
     * @param url URL of the image.
     * @returns Whether or not the operation was successful.
     */
    SetImageSource(url: string): boolean;
}

/**
 * Class for an input entity.
 */
declare class InputEntity extends BaseEntity {
    /** Constructor for input entity. */
    constructor();

    /**
     * Set the placeholder text.
     * @param text Placeholder text.
     * @returns Whether or not the operation was successful.
     */
    SetPlaceholder(text: string): boolean;

    /**
     * Get the placeholder text.
     * @returns The placeholder text.
     */
    GetPlaceholder(): string;

    /**
     * Set the value.
     * @param value Value to set.
     * @returns Whether or not the operation was successful.
     */
    SetValue(value: string): boolean;

    /**
     * Get the value.
     * @returns The value.
     */
    GetValue(): string;
}

/**
 * Class for a light entity.
 */
declare class LightEntity extends BaseEntity {
    /** Constructor for light entity. */
    constructor();

    /**
     * Set the light properties.
     * @param properties Light properties.
     * @returns Whether or not the operation was successful.
     */
    SetLightProperties(properties: LightProperties): boolean;

    /**
     * Get the light properties.
     * @returns The light properties.
     */
    GetLightProperties(): LightProperties;
}

/**
 * Class for a mesh entity.
 */
declare class MeshEntity extends BaseEntity {
    /** Constructor for mesh entity. */
    constructor();

    /**
     * Set the mesh.
     * @param url URL of the mesh.
     * @returns Whether or not the operation was successful.
     */
    SetMesh(url: string): boolean;
}

/**
 * Class for a terrain entity.
 */
declare class TerrainEntity extends BaseEntity {
    /** Constructor for terrain entity. */
    constructor();

    /**
     * Apply a modification to the terrain.
     * @param modification Modification to apply.
     * @returns Whether or not the operation was successful.
     */
    ApplyModification(modification: TerrainEntityModification): boolean;

    /**
     * Set a terrain layer.
     * @param index Layer index.
     * @param layer Layer data.
     * @returns Whether or not the operation was successful.
     */
    SetLayer(index: number, layer: TerrainEntityLayer): boolean;
}

/**
 * Class for a text entity.
 */
declare class TextEntity extends BaseEntity {
    /** Constructor for text entity. */
    constructor();

    /**
     * Set the text.
     * @param text Text to set.
     * @returns Whether or not the operation was successful.
     */
    SetText(text: string): boolean;

    /**
     * Get the text.
     * @returns The text.
     */
    GetText(): string;

    /**
     * Set the font size.
     * @param size Font size.
     * @returns Whether or not the operation was successful.
     */
    SetFontSize(size: number): boolean;

    /**
     * Get the font size.
     * @returns The font size.
     */
    GetFontSize(): number;

    /**
     * Set the text color.
     * @param color Text color.
     * @returns Whether or not the operation was successful.
     */
    SetColor(color: Color): boolean;

    /**
     * Get the text color.
     * @returns The text color.
     */
    GetColor(): Color;
}

/**
 * Class for a voxel entity.
 */
declare class VoxelEntity extends BaseEntity {
    /** Constructor for voxel entity. */
    constructor();

    /**
     * Set a voxel block.
     * @param position Position of the block.
     * @param block Block information.
     * @returns Whether or not the operation was successful.
     */
    SetBlock(position: Vector3Int, block: VoxelBlockInfo): boolean;

    /**
     * Get a voxel block.
     * @param position Position of the block.
     * @returns Block information.
     */
    GetBlock(position: Vector3Int): VoxelBlockInfo;
}

/**
 * Class for a water blocker entity.
 */
declare class WaterBlockerEntity extends BaseEntity {
    /** Constructor for water blocker entity. */
    constructor();
}

/**
 * Class for a water entity.
 */
declare class WaterEntity extends BaseEntity {
    /** Constructor for water entity. */
    constructor();
}

// ============================================================================
// Networking
// ============================================================================

/**
 * Fetch request options interface.
 */
declare interface FetchRequestOptions {
    /** Request body. */
    body?: string;
    /** Cache mode. */
    cache?: string;
    /** Credentials mode. */
    credentials?: string;
    /** Request headers. */
    headers?: string[];
    /** Keep alive flag. */
    keepalive?: boolean;
    /** HTTP method (GET, POST, PUT, DELETE, PATCH, MERGE, OPTIONS, CONNECT, QUERY). */
    method?: string;
    /** Request mode. */
    mode?: string;
    /** Request priority. */
    priority?: string;
    /** Redirect mode. */
    redirect?: string;
    /** Referrer. */
    referrer?: string;
    /** Referrer policy. */
    referrerPolicy?: string;
}

/**
 * Class for HTTP networking.
 */
declare class HTTPNetworking {
    /**
     * Class for HTTP request.
     */
    static Request: {
        new(input: string, options?: FetchRequestOptions): any;
    };

    /**
     * Fetch a resource.
     * @param input URL or Request object.
     * @param options Request options.
     * @param onComplete Callback function name.
     * @param onError Error callback function name.
     */
    static Fetch(input: string | any, options: FetchRequestOptions | null, onComplete: string, onError?: string): void;
}

/**
 * Class for MQTT client (available when USE_WEBINTERFACE is defined).
 */
declare class MQTTClient {
    /** Constructor for MQTT client. */
    constructor();

    /**
     * Connect to MQTT broker.
     * @param host Broker host.
     * @param port Broker port.
     * @param clientId Client ID.
     * @param onConnect Callback function name for connection.
     */
    Connect(host: string, port: number, clientId: string, onConnect: string): void;

    /**
     * Disconnect from MQTT broker.
     */
    Disconnect(): void;

    /**
     * Subscribe to a topic.
     * @param topic Topic to subscribe to.
     * @param onMessage Callback function name for messages.
     */
    Subscribe(topic: string, onMessage: string): void;

    /**
     * Unsubscribe from a topic.
     * @param topic Topic to unsubscribe from.
     */
    Unsubscribe(topic: string): void;

    /**
     * Publish a message.
     * @param topic Topic to publish to.
     * @param message Message to publish.
     */
    Publish(topic: string, message: string): void;
}

/**
 * Class for WebSocket (available when USE_WEBINTERFACE is defined).
 */
declare class WebSocket {
    /**
     * Constructor for WebSocket.
     * @param url WebSocket URL.
     */
    constructor(url: string);

    /**
     * Send data through the WebSocket.
     * @param data Data to send.
     */
    Send(data: string): void;

    /**
     * Close the WebSocket.
     */
    Close(): void;

    /**
     * Set the onopen callback.
     * @param callback Callback function name.
     */
    SetOnOpen(callback: string): void;

    /**
     * Set the onmessage callback.
     * @param callback Callback function name.
     */
    SetOnMessage(callback: string): void;

    /**
     * Set the onerror callback.
     * @param callback Callback function name.
     */
    SetOnError(callback: string): void;

    /**
     * Set the onclose callback.
     * @param callback Callback function name.
     */
    SetOnClose(callback: string): void;
}

// ============================================================================
// Input
// ============================================================================

/**
 * VR pointer mode enum.
 */
declare enum VRPointerMode {
    None = 0,
    Teleport = 1,
    UI = 2
}

/**
 * VR turn locomotion mode enum.
 */
declare enum VRTurnLocomotionMode {
    None = 0,
    Smooth = 1,
    Snap = 2
}

/**
 * Class for input methods.
 */
declare class Input {
    /** Whether or not VR is active. */
    static readonly IsVR: boolean;

    /**
     * Get the current move value.
     * @returns A Vector2 representation of the current move value.
     */
    static GetMoveValue(): Vector2;

    /**
     * Get the current look value.
     * @returns A Vector2 representation of the current look value.
     */
    static GetLookValue(): Vector2;

    /**
     * Get the current pressed state of a key.
     * @param key The key to check.
     * @returns Whether or not the key is pressed.
     */
    static GetKeyValue(key: string): boolean;

    /**
     * Get the current pressed state of a key by keycode.
     * @param keycode The keycode to check.
     * @returns Whether or not the key is pressed.
     */
    static GetKeyCodeValue(keycode: string): boolean;

    /**
     * Get the current pressed state of the left mouse button.
     * @returns Whether or not the left mouse button is pressed.
     */
    static GetLeft(): boolean;

    /**
     * Get the current pressed state of the middle mouse button.
     * @returns Whether or not the middle mouse button is pressed.
     */
    static GetMiddle(): boolean;

    /**
     * Get the current pressed state of the right mouse button.
     * @returns Whether or not the right mouse button is pressed.
     */
    static GetRight(): boolean;
}

// ============================================================================
// Environment
// ============================================================================

/**
 * Class for environment manipulation.
 */
declare class Environment {
    /**
     * Set the offset for the world.
     * @param worldOffset World offset.
     */
    static SetWorldOffset(worldOffset: Vector3): void;

    /**
     * Get the offset for the world.
     * @returns World offset.
     */
    static GetWorldOffset(): Vector3;

    /**
     * Set the threshold for updating world offset.
     * @param threshold Threshold distance.
     */
    static SetWorldOffsetUpdateThreshold(threshold: number): void;

    /**
     * Get the threshold for updating world offset.
     * @returns Threshold distance.
     */
    static GetWorldOffsetUpdateThreshold(): number;

    /**
     * Set the character entity to track for the camera.
     * @param entity Character entity to track, or null to stop tracking.
     */
    static SetTrackedCharacterEntity(entity: BaseEntity | null): void;

    /**
     * Set the sky to a texture.
     * @param skyTextureURI URI of the texture to set the sky to.
     */
    static SetSkyTexture(skyTextureURI: string): void;

    /**
     * Set the sky to a solid color.
     * @param color Color to set the sky to.
     * @returns Whether or not the operation was successful.
     */
    static SetSolidColorSky(color: Color): boolean;
}

// ============================================================================
// Data
// ============================================================================

/**
 * Class for asynchronous JSON operations.
 */
declare class AsyncJSON {
    /**
     * Parse JSON asynchronously.
     * @param rawText Raw JSON text.
     * @param onComplete Callback function name.
     * @param context Optional context parameter.
     */
    static Parse(rawText: string, onComplete: string, context?: any): void;

    /**
     * Stringify JSON asynchronously.
     * @param jsonObject JSON object to stringify.
     * @param onComplete Callback function name.
     * @param context Optional context parameter.
     */
    static Stringify(jsonObject: any, onComplete: string, context?: any): void;
}

// ============================================================================
// VOS Synchronization (available when USE_WEBINTERFACE is defined)
// ============================================================================

/**
 * VOS Synchronization transport enum.
 */
declare enum VSSTransport {
    TCP = 0,
    WebSocket = 1
}

/**
 * Class for VOS Synchronization (available when USE_WEBINTERFACE is defined).
 */
declare class VOSSynchronization {
    /**
     * Create a VOS Synchronization Session.
     * @param host Host of the connection to create the session on.
     * @param port Port of the connection to create the session on.
     * @param tls Whether to use TLS.
     * @param id RFC 4122-encoded UUID identifier for the session.
     * @param tag Tag for the session.
     * @param transport Transport to use.
     * @returns Whether or not the operation was successful.
     */
    static CreateSession(host: string, port: number, tls: boolean, id: string, tag: string, transport?: VSSTransport): boolean;

    /**
     * Create a VOS Synchronization Session with position.
     * @param host Host of the connection to create the session on.
     * @param port Port of the connection to create the session on.
     * @param tls Whether to use TLS.
     * @param id RFC 4122-encoded UUID identifier for the session.
     * @param tag Tag for the session.
     * @param position Initial position for the session.
     * @param transport Transport to use.
     * @returns Whether or not the operation was successful.
     */
    static CreateSession(host: string, port: number, tls: boolean, id: string, tag: string, position: Vector3, transport?: VSSTransport): boolean;
}

// ============================================================================
// World Browser Utilities
// ============================================================================

/**
 * Class for camera manipulation.
 */
declare class Camera {
    /**
     * Attach camera to an entity.
     * @param entityToAttachTo Entity to attach camera to, or null to make root.
     * @returns Whether or not the operation was successful.
     */
    static AttachToEntity(entityToAttachTo: BaseEntity | null): boolean;

    /**
     * Add a camera follower.
     * @param entity Entity to follow.
     * @returns Whether or not the operation was successful.
     */
    static AddCameraFollower(entity: BaseEntity): boolean;

    /**
     * Remove a camera follower.
     * @param entity Entity to stop following.
     * @returns Whether or not the operation was successful.
     */
    static RemoveCameraFollower(entity: BaseEntity): boolean;

    /**
     * Set position of the camera.
     * @param position Position to apply to camera.
     * @param local Whether or not the position is local.
     * @returns Whether or not the operation was successful.
     */
    static SetPosition(position: Vector3, local: boolean): boolean;

    /**
     * Get the position of the camera.
     * @param local Whether or not the position is local.
     * @returns The position of the camera.
     */
    static GetPosition(local: boolean): Vector3;

    /**
     * Set the rotation of the camera.
     * @param rotation Rotation to apply to camera.
     * @param local Whether or not the rotation is local.
     * @returns Whether or not the operation was successful.
     */
    static SetRotation(rotation: Quaternion, local: boolean): boolean;

    /**
     * Get the rotation of the camera.
     * @param local Whether or not the rotation is local.
     * @returns The rotation of the camera.
     */
    static GetRotation(local: boolean): Quaternion;
}

/**
 * Class for context management.
 */
declare class Context {
    /**
     * Set a context.
     * @param contextName Name of the context.
     * @param context Context.
     */
    static DefineContext(contextName: string, context: any): void;

    /**
     * Get a context.
     * @param contextName Name of the context.
     * @returns Context.
     */
    static GetContext(contextName: string): any;
}

/**
 * Class for date and time operations.
 */
declare class Date {
    /** Get a Date for the current millisecond. */
    static readonly now: Date;

    /** Year. */
    readonly year: number;
    /** Month. */
    readonly month: number;
    /** Day. */
    readonly day: number;
    /** Day of week (0=Sunday, 6=Saturday). */
    readonly dayOfWeek: number;
    /** Day of year. */
    readonly dayOfYear: number;
    /** Hour. */
    readonly hour: number;
    /** Minute. */
    readonly minute: number;
    /** Second. */
    readonly second: number;
    /** Millisecond. */
    readonly millisecond: number;

    /**
     * Constructor for a Date.
     * @param year Year.
     * @param month Month.
     * @param day Day.
     */
    constructor(year: number, month: number, day: number);
    /**
     * Constructor for a Date.
     * @param year Year.
     * @param month Month.
     * @param day Day.
     * @param hours Hours.
     */
    constructor(year: number, month: number, day: number, hours: number);
    /**
     * Constructor for a Date.
     * @param year Year.
     * @param month Month.
     * @param day Day.
     * @param hours Hours.
     * @param minutes Minutes.
     */
    constructor(year: number, month: number, day: number, hours: number, minutes: number);
    /**
     * Constructor for a Date.
     * @param year Year.
     * @param month Month.
     * @param day Day.
     * @param hours Hours.
     * @param minutes Minutes.
     * @param seconds Seconds.
     */
    constructor(year: number, month: number, day: number, hours: number, minutes: number, seconds: number);
    /**
     * Constructor for a Date.
     * @param year Year.
     * @param month Month.
     * @param day Day.
     * @param hours Hours.
     * @param minutes Minutes.
     * @param seconds Seconds.
     * @param milliseconds Milliseconds.
     */
    constructor(year: number, month: number, day: number, hours: number, minutes: number, seconds: number, milliseconds: number);
    /**
     * Constructor for a Date.
     * @param dateString Date string.
     */
    constructor(dateString: string);

    /**
     * Get a string representation of the complete date and time.
     * @returns A string representation of the complete date and time.
     */
    ToString(): string;

    /**
     * Get a string representation of the date.
     * @returns A string representation of the date.
     */
    ToDateString(): string;

    /**
     * Get a string representation of the time.
     * @returns A string representation of the time.
     */
    ToTimeString(): string;

    /**
     * Get a string representation of the complete UTC date and time.
     * @returns A string representation of the complete UTC date and time.
     */
    ToUTCString(): string;
}

/**
 * Class for local storage operations.
 */
declare class LocalStorage {
    /**
     * Set an item in local storage.
     * @param key Storage key.
     * @param value Value to store.
     */
    static SetItem(key: string, value: string): void;

    /**
     * Get an item from local storage.
     * @param key Storage key.
     * @returns The stored value, or null.
     */
    static GetItem(key: string): string | null;

    /**
     * Remove an item from local storage.
     * @param key Storage key.
     */
    static RemoveItem(key: string): void;
}

/**
 * Logging message type enum.
 */
declare enum LoggingType {
    Default = 0,
    Debug = 1,
    Warning = 2,
    Error = 3
}

/**
 * Class for logging operations.
 */
declare class Logging {
    /**
     * Log a message.
     * @param message Message to log.
     * @param type Type of the message.
     */
    static Log(message: string, type?: LoggingType): void;

    /**
     * Log a debug message.
     * @param message Debug message to log.
     */
    static LogDebug(message: string): void;

    /**
     * Log a warning message.
     * @param message Warning message to log.
     */
    static LogWarning(message: string): void;

    /**
     * Log an error message.
     * @param message Error message to log.
     */
    static LogError(message: string): void;
}

/**
 * Class for scripting operations.
 */
declare class Scripting {
    /**
     * Run a script.
     * @param scriptToRun Script to run.
     */
    static RunScript(scriptToRun: string): void;
}

/**
 * Class for time operations.
 */
declare class Time {
    /**
     * Set interval at which to run a function.
     * @param functionName Function to run.
     * @param interval Interval at which to run the function.
     * @returns ID of the registered function, or null.
     */
    static SetInterval(functionName: string, interval: number): UUID | null;

    /**
     * Call a function asynchronously.
     * @param functionName Function to call.
     * @returns Whether or not the operation was successful.
     */
    static CallAsynchronously(functionName: string): boolean;

    /**
     * Stop running a registered function.
     * @param id ID of the registered function to stop running.
     * @returns Whether or not the operation was successful.
     */
    static StopInterval(id: string): boolean;

    /**
     * Set timeout after which to run logic.
     * @param logic Logic to run.
     * @param timeout Timeout after which to run the specified logic.
     * @returns Whether or not the operation was successful.
     */
    static SetTimeout(logic: string, timeout: number): boolean;
}

/**
 * Class for world operations.
 */
declare class World {
    /**
     * Get a URL Query Parameter.
     * @param key Key of the Query Parameter.
     * @returns The value of the Query Parameter, or null.
     */
    static GetQueryParam(key: string): string | null;

    /**
     * Get the current World Load State.
     * @returns One of: unloaded, loadingworld, loadedworld, webpage, error.
     */
    static GetWorldLoadState(): "unloaded" | "loadingworld" | "loadedworld" | "webpage" | "error";

    /**
     * Load a World from a URL.
     * @param url The URL of the World to load.
     */
    static LoadWorld(url: string): void;

    /**
     * Load a Web Page from a URL.
     * @param url The URL of the Web Page to load.
     */
    static LoadWebPage(url: string): void;
}

/**
 * Class for world storage operations.
 */
declare class WorldStorage {
    /**
     * Set an item in world storage.
     * @param key Storage key.
     * @param value Value to store.
     */
    static SetItem(key: string, value: string): void;

    /**
     * Get an item from world storage.
     * @param key Storage key.
     * @returns The stored value, or null.
     */
    static GetItem(key: string): string | null;
}
